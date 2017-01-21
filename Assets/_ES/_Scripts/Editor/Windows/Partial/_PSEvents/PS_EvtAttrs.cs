using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 类名 : 绘制 属性事件
/// 作者 : Canyon
/// 日期 : 2017-01-21 09:30
/// 功能 : 
/// </summary>
public class PS_EvtAttrs {

	System.Action m_callNew;
	System.Action<EDT_Property> m_callRemove;
	List<EDT_Property> list;
	bool m_isPlan;
	float duration;
	string m_title;

	bool m_isDrawTime = false;

	List<bool> m_lFodeout = new List<bool>();

	public PS_EvtAttrs(string m_title,bool m_isPlan,System.Action m_callNew,System.Action<EDT_Property> m_callRemove,bool isDrawTime){
		this.m_title = m_title;
		this.m_callNew = m_callNew;
		this.m_callRemove = m_callRemove;
		this.m_isPlan = m_isPlan;
		this.m_isDrawTime = isDrawTime;
	}

	public void DoDraw(float duration,List<EDT_Property> list){
		this.duration = duration;
		this.list = list;
		_DrawEvents ();
	}

	void _DrawEvents(){
		EG_GUIHelper.FG_BeginVAsArea();
		{
			{
				// 上
				EG_GUIHelper.FEG_BeginH();
				Color def = GUI.backgroundColor;
				GUI.backgroundColor = Color.black;
				GUI.color = Color.white;

				EditorGUILayout.LabelField(m_title, EditorStyles.textArea);

				GUI.backgroundColor = def;

				GUI.color = Color.green;
				if (GUILayout.Button("+", GUILayout.Width(50)))
				{
					if (this.m_callNew != null) {
						this.m_callNew ();
					}
				}
				GUI.color = Color.white;
				EG_GUIHelper.FEG_EndH();
			}

			{
				// 中
				int lens = list.Count;
				if (lens > 0) {
					for (int i = 0; i < lens; i++) {
						m_lFodeout.Add (false);
						_DrawOneEvnet (i, list [i]);
					}
				} else {
					m_lFodeout.Clear();
				}
			}
		}
		EG_GUIHelper.FG_EndV();
	}

	void _DrawOneEvnet(int index, EDT_Property one)
	{
		
		EG_GUIHelper.FEG_BeginV();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				m_lFodeout[index] = EditorGUILayout.Foldout(m_lFodeout[index], "属性修改 - " + EnumExtension.GetDescription(one.m_emTag));
				GUI.color = Color.red;
				if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(50)))
				{
					if (this.m_callRemove != null) {
						this.m_callRemove (one);
					}
					m_lFodeout.RemoveAt(index);
				}
				GUI.color = Color.white;
			}
			EG_GUIHelper.FEG_EndH();

			EG_GUIHelper.FG_Space(5);

			if (m_lFodeout[index])
			{
				_DrawOneEventAttrs(one);
			}
		}
		EG_GUIHelper.FEG_EndV();
	}

	void _DrawOneEventAttrs(EDT_Property one)
	{
		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("主分类:", GUILayout.Width(80));
			one.m_emTag = (EDT_Property.PropretyTag)EditorGUILayout.EnumPopup ((System.Enum)one.m_emTag);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("子分类:", GUILayout.Width(80));
			one.m_iGID = EditorGUILayout.IntField (one.m_iGID);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("参数:", GUILayout.Width(80));
			one.m_sPars = EditorGUILayout.TextArea (one.m_sPars);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);
	}
}
