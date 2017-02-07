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
	EM_Cell m_entity;
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		m_entity = target as EM_Cell;

		if (GUILayout.Button ("SyncToInspector")) {
			if(m_entity.m_entity != null)
				m_entity.m_entity.ToTrsfData ();
		}
	}
}
