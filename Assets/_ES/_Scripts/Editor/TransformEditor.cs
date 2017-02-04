using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(Transform))]
public class TransformEditor :Editor {

	static public System.Action<Transform> onChangePosition;
	static public System.Action<Transform> onChangeRotation;
	static public System.Action<Transform> onChangeScale;

	[InitializeOnLoadMethod]
	static void IInitializeOnLoadMethod()
	{
		TransformEditor.onChangePosition = delegate(Transform transform){
			// Debug.Log(string.Format("transform = {0}  positon = {1}",transform.name,transform.localPosition));
		};

		TransformEditor.onChangeRotation = delegate(Transform transform) {
			// Debug.Log(string.Format("transform = {0}   rotation = {1}",transform.name,transform.localRotation.eulerAngles));
		};

		TransformEditor.onChangeScale = delegate(Transform transform) {
			// Debug.Log(string.Format("transform = {0}   scale = {1}",transform.name,transform.localScale));
		};
	}

	private Editor editor;
	private Transform m_trsf;
	private Vector3 startPostion =Vector3.zero;
	private Vector3 startRotation =Vector3.zero;
	private Vector3 startScale =Vector3.zero;

	void OnEnable()
	{
		editor = Editor.CreateEditor(target, Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.TransformInspector",true));

		m_trsf = target as Transform;
		startPostion = m_trsf.localPosition;
		startRotation = m_trsf.localRotation.eulerAngles;
		startScale = m_trsf.localScale;
	}

	public override void OnInspectorGUI ()
	{
		editor.OnInspectorGUI();
		if(GUI.changed || m_trsf.hasChanged)
		{

			if(startPostion != m_trsf.localPosition)
			{
				if(onChangePosition != null)
					onChangePosition(m_trsf);
			}

			if(startRotation !=  m_trsf.localRotation.eulerAngles)
			{
				if(onChangeRotation != null)
					onChangeRotation(m_trsf);
			}

			if(startScale !=  m_trsf.localScale)
			{
				if(onChangeScale != null)
					onChangeScale(m_trsf);
			}

			startPostion = m_trsf.localPosition;
			startRotation = m_trsf.localRotation.eulerAngles;
			startScale = m_trsf.localScale;

			m_trsf.hasChanged = false;
		}

	}


}