using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 类名 : 绘制 受击状态(无，退，飞，倒，晕)
/// 作者 : Canyon
/// 日期 : 2017-01-24 10:50
/// 功能 : 
/// </summary>
public class PS_EvtHitStatus {

	System.Action m_callNew;
	System.Action<EDT_HitStatus> m_callRemove;
	List<EDT_HitStatus> list;
	bool m_isPlan;
	float duration;
	string m_title;

	bool m_isDrawTime = false;

	List<bool> m_lFodeout = new List<bool>();

	int m_hitStatus = 0;
	string[] StatusType = { 
		"默认",
		"击退",
		"击飞",
		// "击倒",
		// "击晕"
	};

	public PS_EvtHitStatus(string m_title,bool m_isPlan,System.Action m_callNew,System.Action<EDT_HitStatus> m_callRemove,bool isDrawTime){
		this.m_title = m_title;
		this.m_callNew = m_callNew;
		this.m_callRemove = m_callRemove;
		this.m_isPlan = m_isPlan;
		this.m_isDrawTime = isDrawTime;
	}

	public void DoDraw(float duration,List<EDT_HitStatus> list){
		this.duration = duration;
		this.list = list;
		_DrawEvents ();
	}

	void _DrawEvents(){
		EG_GUIHelper.FG_BeginVAsArea();
		{
			{
				// 上
				EG_GUIHelper.FEG_Head(m_title,true,this.m_callNew);
			}

			{
				// 中
				int lens = list.Count;
				if (lens > 0) {
					for (int i = 0; i < lens; i++) {
						if (m_lFodeout.Count <= i) {
							m_lFodeout.Add (false);
						}
						_DrawOneEvnet (i, list [i]);
					}
				} else {
					m_lFodeout.Clear();
				}
			}
		}
		EG_GUIHelper.FG_EndV();
	}

	void _DrawOneEvnet(int index, EDT_HitStatus one)
	{
		
		EG_GUIHelper.FEG_BeginV();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				m_lFodeout[index] = EditorGUILayout.Foldout(m_lFodeout[index], "受击表现状态 - " + EnumExtension.GetDescription(one.m_emType));
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

			if (m_lFodeout.Count > index && m_lFodeout[index])
			{
				_DrawOneEventAttrs(one);
			}
		}
		EG_GUIHelper.FEG_EndV();
	}

	void _DrawOneEventAttrs(EDT_HitStatus one)
	{
		if (this.m_isDrawTime) {
			EG_GUIHelper.FEG_BeginH ();
			{
				GUILayout.Label ("触发时间:");
				if (m_isPlan) {
					one.m_fCastTime = EditorGUILayout.Slider (one.m_fCastTime, 0, duration);
				} else {
					one.m_fCastTime = EditorGUILayout.FloatField (one.m_fCastTime);
				}
			}
			EG_GUIHelper.FEG_EndH ();
			EG_GUIHelper.FG_Space (5);
		}

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("受击者的受击时表现:", GUILayout.Width(80));
			m_hitStatus = EditorGUILayout.Popup (m_hitStatus, StatusType);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);
		switch (m_hitStatus) {
		case 1:
			one.m_emType = EDT_Base.EventType.BeHitBack;
			break;
		case 2:
			one.m_emType = EDT_Base.EventType.BeHitFly;
			break;
		default:
			one.m_emType = EDT_Base.EventType.BeHitDefault;
			break;
		}
	}
}
