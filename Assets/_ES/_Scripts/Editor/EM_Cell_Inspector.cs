using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : 绘制EM_Cell的对象
/// 作者 : Canyon
/// 日期 : 2017-02-07 18:10
/// 功能 : 添加一个按钮进行同步
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(EM_Cell),true)]
public class EM_Cell_Inspector : Editor {

	static Hashtable mapIDS = new Hashtable ();

	static bool m_isChanged = false;

	ArrayList list = new ArrayList ();
	ArrayList rmList = new ArrayList ();

	EM_Cell m_entity;

	void OnEnable ()
	{
		m_entity = target as EM_Cell;
		int id = m_entity.GetInstanceID ();
		mapIDS [id] = m_entity;
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
			isHas = IsInTargets ((EM_Cell)mapIDS [key]);
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
		base.OnInspectorGUI ();

		if (GUILayout.Button ("SyncToInspector")) {
			SyncAll();
		}
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

	void SyncTarget(){
		m_entity = target as EM_Cell;
		m_isChanged = true;
	}

	void SyncOne(Object objTarget){
		EM_Cell temp = objTarget as EM_Cell;
		if (m_isChanged) {
			temp.m_entity.ToTrsfData ();
		}
	}

	void SyncAll(){
		SyncTarget();
		SyncObjects ();
	}

	void SyncObjects(){
		Object[] objs = targets;
		if (objs == null || objs.Length <= 0) {
			return;
		}

		int lens = objs.Length;
		Object obj = null;
		for (int i = 0; i < lens; i++) {
			obj = objs [i];
			SyncOne (obj);
		}

		m_isChanged = false;
	}
}
