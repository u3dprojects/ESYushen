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
	Dictionary<int,PS_EvtHitEvent> mapHitEvt = new Dictionary<int, PS_EvtHitEvent> ();

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

	public void DoClear(){
		mapArea.Clear ();
		mapHitEvt.Clear ();
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

	void _DrawOneEvnet(int index, EDT_Hurt one)
	{
		
		EG_GUIHelper.FEG_BeginV();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				m_lFodeout[index] = EditorGUILayout.Foldout(m_lFodeout[index], "列表 - " + EnumExtension.GetDescription(one.m_emTag));
				GUI.color = Color.red;
				if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(50)))
				{
					m_lFodeout.RemoveAt(index);
					_RemoveOneHurt (one);
				}
				GUI.color = Color.white;
			}
			EG_GUIHelper.FEG_EndH();

			EG_GUIHelper.FG_Space(8);

			if (m_lFodeout.Count > index && m_lFodeout[index])
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
			EG_GUIHelper.FG_Space (8);
		}

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("类型:", GUILayout.Width(80));
			one.m_emTag = (EDT_Hurt.HurtType)EditorGUILayout.EnumPopup ((System.Enum)one.m_emTag);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(8);

		EG_GUIHelper.FEG_BeginH();
		{
			one.m_isCanShow = EditorGUILayout.Toggle ("Play时播放事件??", one.m_isCanShow);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(8);

		if (one.m_emTag == EDT_Hurt.HurtType.MoveTarget) {
			EG_GUIHelper.FEG_BeginH ();
			{
				GUILayout.Label ("优先目标:", GUILayout.Width (80));
				Color def = GUI.color;
				GUI.color = Color.yellow;
				GUILayout.Label ("("+EnumExtension.GetDescription(one.m_iTargetFilter)+")");
				GUI.color = def;
				one.m_iTargetFilter = (EDT_Hurt.FitlerTargetType)EditorGUILayout.EnumPopup ((System.Enum)one.m_iTargetFilter);
			}
			EG_GUIHelper.FEG_EndH ();
			EG_GUIHelper.FG_Space (8);

			EG_GUIHelper.FEG_BeginH ();
			{
				GUILayout.Label ("目标数量:", GUILayout.Width (100));
				one.m_iTargetCount = EditorGUILayout.IntField (one.m_iTargetCount);
			}
			EG_GUIHelper.FEG_EndH ();
			EG_GUIHelper.FG_Space (8);

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
		mapHitEvt.Remove(one.m_iCurID);
	}

	// ==== 伤害区域 ====
	void _DrawOneHurt_HurtAreas(EDT_Hurt one){
		PS_EvtHurtArea psEvt = null;
		if (mapArea.ContainsKey (one.m_iCurID)) {
			psEvt = mapArea [one.m_iCurID];
		}
		else{	
			psEvt = new PS_EvtHurtArea ("伤害区域列表:", m_isPlan, delegate {
				one.NewHitArea ();
			}, delegate (EDT_Hurt_Area rm) {
				one.RemoveHitArea (rm);
			}, false);

			mapArea [one.m_iCurID] = psEvt;
		}

		psEvt.DoDraw (duration, one.GetAreaList());
	}

	#region ==== 受击者 - 命中事件 ====

	void _DrawOneHurt_BeHitter(EDT_Hurt one){
		EG_GUIHelper.FG_BeginVAsArea();
		{
			EG_GUIHelper.FEG_Head("命中事件",new Color(0.1f,0.5f,0.3f,0.6f));

			_DrawOneHurtEvent (one);
		}
		EG_GUIHelper.FG_EndV();
	}

	void _DrawOneHurtEvent(EDT_Hurt one){
		PS_EvtHitEvent psEvt;
		if (mapHitEvt.ContainsKey (one.m_iCurID)) {
			psEvt = mapHitEvt [one.m_iCurID];
		}
		else{	
			psEvt = new PS_EvtHitEvent ();
			psEvt.SetEvent (one.HitEvent);
			mapHitEvt [one.m_iCurID] = psEvt;
		}
		psEvt.DoDraw ();
	}
	#endregion

}
