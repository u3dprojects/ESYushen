﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 类名 : 绘制 音效事件
/// 作者 : Canyon
/// 日期 : 2017-01-17 17:30
/// 功能 : 
/// </summary>
public class PS_EvtAudio {

	System.Action m_callNew;
	System.Action<EDT_Audio> m_callRemove;
	List<EDT_Audio> list;
	bool m_isPlan;
	float duration;
	string m_title;

	public bool m_isDrawTime = false;

	List<bool> m_lFodeout = new List<bool>();

	public PS_EvtAudio(string m_title,bool m_isPlan,System.Action m_callNew,System.Action<EDT_Audio> m_callRemove,bool isDrawTime){
		this.m_title = m_title;
		this.m_callNew = m_callNew;
		this.m_callRemove = m_callRemove;
		this.m_isPlan = m_isPlan;
		this.m_isDrawTime = isDrawTime;
	}

	public void DoDraw(float duration,List<EDT_Audio> list){
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

	void _DrawOneEvnet(int index, EDT_Audio one)
	{

		bool isEmptyName = string.IsNullOrEmpty(one.m_sName);

		EG_GUIHelper.FEG_BeginV();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				m_lFodeout[index] = EditorGUILayout.Foldout(m_lFodeout[index], "音效 - " + (isEmptyName ? "未指定" : one.m_sName));
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

	void _DrawOneEventAttrs(EDT_Audio one)
	{

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("音效文件:", GUILayout.Width(80));
			one.m_objOrg = EditorGUILayout.ObjectField(one.m_objOrg, typeof(AudioClip), false) as AudioClip;
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
			GUILayout.Label("音量:", GUILayout.Width(80));
			one.m_fVolume = EditorGUILayout.Slider (one.m_fVolume,0f,1f);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("是否循环:", GUILayout.Width(80));
			one.m_isLoop = EditorGUILayout.Toggle (one.m_isLoop);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);
	}
}
