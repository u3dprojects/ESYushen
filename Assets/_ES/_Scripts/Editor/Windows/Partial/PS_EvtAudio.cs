using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 类名 : 绘制 音效
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

	bool isInit = false;
	public void DoInit(string m_title,bool m_isPlan,System.Action m_callNew,System.Action<EDT_Audio> m_callRemove){
		if (isInit)
			return;
		
		isInit = true;
		this.m_title = m_title;
		this.m_callNew = m_callNew;
		this.m_callRemove = m_callRemove;
		this.m_isPlan = m_isPlan;
	}

	public void DoDraw(float duration,List<EDT_Audio> list){
		this.duration = duration;
		this.list = list;
		_DrawEvents4Audio ();
	}

	List<bool> m_audio_fodeOut = new List<bool>();
	void _DrawEvents4Audio(){
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
						m_audio_fodeOut.Add (false);
						_DrawOneAudio (i, list [i]);
					}
				} else {
					m_audio_fodeOut.Clear();
				}
			}
		}
		EG_GUIHelper.FG_EndV();
	}

	void _DrawOneAudio(int index, EDT_Audio audio)
	{

		bool isEmptyName = string.IsNullOrEmpty(audio.m_sName);

		EG_GUIHelper.FEG_BeginV();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				m_audio_fodeOut[index] = EditorGUILayout.Foldout(m_audio_fodeOut[index], "音效 - " + (isEmptyName ? "未指定" : audio.m_sName));
				GUI.color = Color.red;
				if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(50)))
				{
					if (this.m_callRemove != null) {
						this.m_callRemove (audio);
					}
					m_audio_fodeOut.RemoveAt(index);
				}
				GUI.color = Color.white;
			}
			EG_GUIHelper.FEG_EndH();

			EG_GUIHelper.FG_Space(5);

			if (m_audio_fodeOut[index])
			{
				_DrawOneAudioAttrs(audio);
			}
		}
		EG_GUIHelper.FEG_EndV();
	}

	void _DrawOneAudioAttrs(EDT_Audio audio)
	{

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("音效文件:", GUILayout.Width(80));
			audio.m_objOrg = EditorGUILayout.ObjectField(audio.m_objOrg, typeof(AudioClip), false) as AudioClip;
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("触发时间:");
			if (m_isPlan) {
				audio.m_fCastTime = EditorGUILayout.Slider(audio.m_fCastTime, 0, duration);
			} else {
				audio.m_fCastTime = EditorGUILayout.FloatField (audio.m_fCastTime);
			}
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("音量:", GUILayout.Width(80));
			audio.m_fVolume = EditorGUILayout.Slider (audio.m_fVolume,0f,1f);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("是否循环:", GUILayout.Width(80));
			audio.m_isLoop = EditorGUILayout.Toggle (audio.m_isLoop);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);
	}
}
