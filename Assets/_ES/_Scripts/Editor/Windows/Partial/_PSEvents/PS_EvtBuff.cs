﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 类名 : 绘制 Buff事件
/// 作者 : Canyon
/// 日期 : 2017-01-20 17:50
/// 功能 : 
/// </summary>
public class PS_EvtBuff {

	System.Action m_callNew;
	System.Action<EDT_Buff> m_callRemove;
	List<EDT_Buff> list;
	bool m_isPlan;
	float duration;
	string m_title;

	bool m_isDrawTime = false;

	List<bool> m_lFodeout = new List<bool>();

	public PS_EvtBuff(string m_title,bool m_isPlan,System.Action m_callNew,System.Action<EDT_Buff> m_callRemove,bool isDrawTime){
		this.m_title = m_title;
		this.m_callNew = m_callNew;
		this.m_callRemove = m_callRemove;
		this.m_isPlan = m_isPlan;
		this.m_isDrawTime = isDrawTime;
	}

	public void DoDraw(float duration,List<EDT_Buff> list){
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

	void _DrawOneEvnet(int index, EDT_Buff one)
	{

		bool isEmptyName = one.m_iBuffId <= 0;

		EG_GUIHelper.FEG_BeginV();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				m_lFodeout[index] = EditorGUILayout.Foldout(m_lFodeout[index], "Buff - " + (isEmptyName ? "未指定" : ""+one.m_iBuffId));
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

	void _DrawOneEventAttrs(EDT_Buff one)
	{
		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("BuffID:", GUILayout.Width(80));
			one.m_iBuffId = EditorGUILayout.IntField(one.m_iBuffId);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

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
			GUILayout.Label("Buff等级:", GUILayout.Width(80));
			one.m_iLevel = EditorGUILayout.IntField(one.m_iLevel);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("持续时间:", GUILayout.Width(80));
			one.m_fDuration = EditorGUILayout.Slider (one.m_fDuration,0f,60f);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("触发概率:", GUILayout.Width(80));
			one.m_iRate = EditorGUILayout.IntSlider (one.m_iRate,0,10000);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);
	}
}
