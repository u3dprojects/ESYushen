using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

/// <summary>
/// 类名 : Transform 消息监听
/// 作者 : yusong
/// 出处 : http://www.xuanyusong.com/archives/4018
/// 修改 : Canyon
/// 日期 : 2017-02-06 10:10
/// 功能 : 处理多个选中对象的处理
/// </summary>
[CanEditMultipleObjects]
[CustomEditor (typeof(Transform), true)]
public class TransformEditor :Editor
{

	static public System.Action<Transform> onChangeTransform;

	[InitializeOnLoadMethod]
	static void IInitializeOnLoadMethod ()
	{
		TransformEditor.onChangeTransform = delegate(Transform transform) {
			// Debug.Log(string.Format("transform = {0}  positon = {1}",transform.name,transform.localPosition));
		};
	}

	static Hashtable mapIDS = new Hashtable ();

	Editor editor;
	ArrayList list = new ArrayList ();
	ArrayList rmList = new ArrayList ();

	void OnEnable ()
	{
		editor = Editor.CreateEditor (target, Assembly.GetAssembly (typeof(Editor)).GetType ("UnityEditor.TransformInspector", true));

		Transform m_trsf = target as Transform;
		int id = m_trsf.GetInstanceID ();
		mapIDS [id] = m_trsf;
	}

	void OnDisable(){
		list.Clear ();
		rmList.Clear ();

		list.AddRange(mapIDS.Keys);
		int lens = list.Count;
		bool isHas = false;
		object key = null;

		for (int i = 0; i < lens; i++) {
			key = list [i];
			isHas = IsInTargets ((Transform)mapIDS [key]);
			if (!isHas) {
				rmList.Add (list [i]);
			}
		}

		lens = rmList.Count;
		for (int i = 0; i < lens; i++) {
			key = rmList [i];
			mapIDS.Remove (key);
		}
	}

	public override void OnInspectorGUI ()
	{
		editor.OnInspectorGUI ();
		SyncAll (target);
		// Repaint ();
	}

	bool IsInTargets(Object org){
		Object[] objs = targets;
		if (objs == null || objs.Length <= 0) {
			return false;
		}

		int lens = objs.Length;
		Object obj = null;
		for (int i = 0; i < lens; i++) {
			obj = objs [i];
			if (org == obj) {
				return true;
			}
		}
		return false;
	}

	void SyncOne(Object objTarget,Object orgActive){
		Transform m_trsfTemp = objTarget as Transform;
		Transform org = orgActive as Transform;
		if (m_trsfTemp != org) {
			m_trsfTemp.localPosition = org.localPosition;
			m_trsfTemp.localScale = org.localScale;
			m_trsfTemp.localEulerAngles = org.localEulerAngles;

			m_trsfTemp.hasChanged = true;
		}

		if (GUI.changed || m_trsfTemp.hasChanged) {
			
			if (onChangeTransform != null)
				onChangeTransform (m_trsfTemp);
			
			m_trsfTemp.hasChanged = false;
		}
	}

	void SyncAll(Object active){
		Object[] objs = targets;
		if (objs == null || objs.Length <= 0) {
			return;
		}

		int lens = objs.Length;
		Object obj = null;
		for (int i = 0; i < lens; i++) {
			obj = objs [i];
			SyncOne (obj,active);
		}
	}
}