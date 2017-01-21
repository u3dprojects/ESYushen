using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 类名 : 绘制 命中事件
/// 作者 : Canyon
/// 日期 : 2017-01-21 09:30
/// 功能 : 
/// </summary>
public class PS_EvtHurt {

	System.Action m_callNew;
	System.Action<EDT_Hurt> m_callRemove;
	List<EDT_Hurt> list;
	bool m_isPlan;
	float begtime;
	float duration;
	string m_title;

	bool m_isDrawTime = false;

	List<bool> m_lFodeout = new List<bool>();

	// 管理
	Dictionary<int,PS_EvtHurtArea> mapArea = new Dictionary<int, PS_EvtHurtArea> ();
	Dictionary<int,PS_EvtAudio> mapAudio = new Dictionary<int, PS_EvtAudio> ();
	Dictionary<int,PS_EvtAttrs> mapAttrs = new Dictionary<int, PS_EvtAttrs> ();
	Dictionary<int,PS_EvtBuff> mapBuff = new Dictionary<int, PS_EvtBuff> ();

	public PS_EvtHurt(string m_title,bool m_isPlan,System.Action m_callNew,System.Action<EDT_Hurt> m_callRemove,bool isDrawTime){
		this.m_title = m_title;
		this.m_callNew = m_callNew;
		this.m_callRemove = m_callRemove;
		this.m_isPlan = m_isPlan;
		this.m_isDrawTime = isDrawTime;
	}

	public void DoDraw(float begtime,float endtime,List<EDT_Hurt> list){
		this.begtime = begtime;
		this.duration = endtime;
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

	void _DrawOneEvnet(int index, EDT_Hurt one)
	{
		
		EG_GUIHelper.FEG_BeginV();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				m_lFodeout[index] = EditorGUILayout.Foldout(m_lFodeout[index], "列表 - " + EnumExtension.GetDescription(one.m_emType));
				GUI.color = Color.red;
				if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(50)))
				{
					m_lFodeout.RemoveAt(index);
					_RemoveOneHurt (one);
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

	void _DrawOneEventAttrs(EDT_Hurt one)
	{
		if (this.m_isDrawTime) {
			EG_GUIHelper.FEG_BeginH ();
			{
				GUILayout.Label ("触发时间:");
				if (m_isPlan) {
					one.m_fCastTime = EditorGUILayout.Slider (one.m_fCastTime, begtime, duration);
				} else {
					one.m_fCastTime = EditorGUILayout.FloatField (one.m_fCastTime);
				}
			}
			EG_GUIHelper.FEG_EndH ();
			EG_GUIHelper.FG_Space (5);
		}

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("类型:", GUILayout.Width(80));
			one.m_emType = (EDT_Hurt.HurtType)EditorGUILayout.EnumPopup ((System.Enum)one.m_emType);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		if (one.m_emType == EDT_Hurt.HurtType.MoveTarget) {
			EG_GUIHelper.FEG_BeginH ();
			{
				GUILayout.Label ("优先目标:", GUILayout.Width (80));
				one.m_iTargetFilter = EditorGUILayout.IntField (one.m_iTargetFilter);
			}
			EG_GUIHelper.FEG_EndH ();
			EG_GUIHelper.FG_Space (5);

			EG_GUIHelper.FEG_BeginH ();
			{
				GUILayout.Label ("目标数量:", GUILayout.Width (80));
				one.m_iTargetCount = EditorGUILayout.IntField (one.m_iTargetCount);
			}
			EG_GUIHelper.FEG_EndH ();
			EG_GUIHelper.FG_Space (5);

			// 伤害区域
			_DrawOneHurt_HurtAreas (one);

		}
		// 受击者
		_DrawOneHurt_BeHitter(one);
	}

	void _RemoveOneHurt(EDT_Hurt one){
		if (this.m_callRemove != null) {
			this.m_callRemove (one);
		}

		mapArea.Remove (one.m_iCurID);
		mapAudio.Remove (one.m_iCurID);
		mapAttrs.Remove (one.m_iCurID);
		mapBuff.Remove (one.m_iCurID);
	}

	// ==== 伤害区域 ====
	void _DrawOneHurt_HurtAreas(EDT_Hurt hurt){
		PS_EvtHurtArea psEvt = null;
		if (mapArea.ContainsKey (hurt.m_iCurID)) {
			psEvt = mapArea [hurt.m_iCurID];
		}
		else{	
			psEvt = new PS_EvtHurtArea ("伤害区域列表:", m_isPlan, delegate {
				hurt.NewHurtArea ();
			}, delegate (EDT_Hurt_Area one) {
				hurt.RemoveHurtArea (one);
			}, false);

			mapArea [hurt.m_iCurID] = psEvt;
		}

		psEvt.DoDraw (duration, hurt.GetAreaList());
	}

	#region ==== 受击者 - 命中事件 ====

	void _DrawOneHurt_BeHitter(EDT_Hurt hurt){
		EG_GUIHelper.FG_BeginVAsArea();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				// 上
				EG_GUIHelper.FEG_BeginH();
				Color def = GUI.backgroundColor;
				GUI.backgroundColor = Color.black;
				GUI.color = Color.white;

				GUILayout.Label("命中事件", EditorStyles.textArea);
				GUI.backgroundColor = def;		
				GUI.color = Color.white;
				EG_GUIHelper.FEG_EndH();
			}
			EG_GUIHelper.FEG_EndH();
			EG_GUIHelper.FG_Space(5);

			EG_GUIHelper.FEG_BeginH();
			{
				hurt.m_isShowBeHitterWhenPlay = EditorGUILayout.Toggle ("播放命中的事件??", hurt.m_isShowBeHitterWhenPlay);
			}
			EG_GUIHelper.FEG_EndH();
			EG_GUIHelper.FG_Space(5);

			_DrawBeHitAudio (hurt);
			EG_GUIHelper.FG_Space(5);

			_DrawBeHitStatus (hurt);

			EG_GUIHelper.FG_Space(5);
			_DrawBeHitBuffs (hurt);
		}
		EG_GUIHelper.FG_EndV();
	}

	// 绘制 音效
	void _DrawBeHitAudio(EDT_Hurt hurt){
		PS_EvtAudio psEvt;
		if (mapAudio.ContainsKey (hurt.m_iCurID)) {
			psEvt = mapAudio [hurt.m_iCurID];
		}
		else{	
			psEvt = new PS_EvtAudio("命中音效:",m_isPlan,delegate {
				hurt.NewBeHitAudio();
			},delegate (EDT_Audio one){
				hurt.RemoveEvent(one);
			},false);
			mapAudio [hurt.m_iCurID] = psEvt;
		}

		psEvt.DoDraw (duration, hurt.GetHitAudioList ());
	}

	// 绘制修改属性
	void _DrawBeHitStatus(EDT_Hurt hurt){
		PS_EvtAttrs psEvt;
		if (mapAttrs.ContainsKey (hurt.m_iCurID)) {
			psEvt = mapAttrs [hurt.m_iCurID];
		}
		else{
			psEvt = new PS_EvtAttrs("命中属性修改列表:",m_isPlan,delegate {
				hurt.NewBeHitStatus();
			},delegate (EDT_Property one){
				hurt.RemoveEvent(one);
			},false);
			mapAttrs [hurt.m_iCurID] = psEvt;
		}

		psEvt.DoDraw (duration, hurt.GetHitStatusList ());
	}

	// 绘制 buff
	void _DrawBeHitBuffs(EDT_Hurt hurt){
		PS_EvtBuff psEvt;
		if (mapBuff.ContainsKey (hurt.m_iCurID)) {
			psEvt = mapBuff [hurt.m_iCurID];
		}
		else{
			psEvt = new PS_EvtBuff("命中Buff列表:",m_isPlan,delegate {
				hurt.NewHitBuff();
			},delegate (EDT_Buff one){
				hurt.RemoveEvent(one);
			},false);

			mapBuff [hurt.m_iCurID] = psEvt;
		}

		psEvt.DoDraw (duration, hurt.GetHitBuffList());
	}

	#endregion

}
