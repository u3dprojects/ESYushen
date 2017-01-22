using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : 绘制 命中事件集合
/// 作者 : Canyon
/// 日期 : 2017-01-21 15:10
/// 功能 : 
/// </summary>
public class PS_EvtHitEvent {
	
	// 事件
	EMT_HitEvent m_evtHit = new EMT_HitEvent();

	string m_sJson = "";
	string m_sJsonTemp = "";

	bool m_isPlan = false;
	bool m_isShowCastTime = false;
	float duration = 0;

	public void DoReInit(string json){
		m_evtHit.DoReInit (json,0);
		m_sJson = json;
	}

	public string ToJsonString(){
		m_sJsonTemp = m_evtHit.ToJsonString ();
		if (string.IsNullOrEmpty (m_sJsonTemp)) {
			m_sJson = "null";
		} else {
			m_sJson = m_sJsonTemp;
		}
		return m_sJson;
	}

	public void DoClear(){
		m_evtHit.DoClear ();
		m_sJson = "";
		m_sJsonTemp = "";
	}

	public void SetEvent(EMT_HitEvent hitEvent){
		if (hitEvent == null) {
			return;
		}

		if (this.m_evtHit != null) {
			this.m_evtHit.DoClear ();
		}
		this.m_evtHit = hitEvent;
	}

	public void DoDraw(){
		EG_GUIHelper.FEG_BeginVArea();
		{
			EditorGUILayout.LabelField(ToJsonString(), EditorStyles.textArea);
			EG_GUIHelper.FG_Space(5);

			_DrawAudio ();
			EG_GUIHelper.FG_Space(10);

			_DrawAttrs ();
			EG_GUIHelper.FG_Space(10);

			_DrawBuffs ();
			 EG_GUIHelper.FG_Space(10);

			_DrawShake ();
		}
		EG_GUIHelper.FEG_EndV();
		EG_GUIHelper.FG_Space(10);
	}

	void _RmEvent(EDT_Base one){
		m_evtHit.RmEvent (one);
	}

	// 绘制修改属性
	PS_EvtAttrs psEvtAttrs;
	void _DrawAttrs(){
		if (psEvtAttrs == null) {
			psEvtAttrs = new PS_EvtAttrs ("属性修改列表:", m_isPlan,_NewAttrs,_RmEvent, m_isShowCastTime);
		}

		psEvtAttrs.DoDraw (duration, m_evtHit.GetLAttrs());
	}

	void _NewAttrs(){
		m_evtHit.NewEvent<EDT_Property> ();
	}

	// 绘制 buff
	PS_EvtBuff psEvtBuff;
	void _DrawBuffs(){
		if (psEvtBuff == null) {
			psEvtBuff = new PS_EvtBuff ("Buff列表:", m_isPlan,_NewBuff,_RmEvent, m_isShowCastTime);
		}

		psEvtBuff.DoDraw (duration, m_evtHit.GetLBuffs());
	}

	void _NewBuff(){
		m_evtHit.NewEvent<EDT_Buff> ();
	}

	// 绘制 音效
	PS_EvtAudio psEvtAudio;
	void _DrawAudio(){
		if (psEvtBuff == null) {
			psEvtAudio = new PS_EvtAudio("音效列表:",m_isPlan,_NewAudio,_RmEvent,m_isShowCastTime);
		}

		psEvtAudio.DoDraw (duration, m_evtHit.GetLAudios());
	}

	void _NewAudio(){
		m_evtHit.NewEvent<EDT_Audio> ();
	}

	// 震屏
	PS_EvtShake m_psShake;

	void _DrawShake(){
		if (m_psShake == null) {
			m_psShake = new PS_EvtShake("震屏列表", m_isPlan, _NewShake, _RmEvent,m_isShowCastTime);
		}
		m_psShake.DoDraw (duration, m_evtHit.GetLShakes());
	}

	void _NewShake(){
		m_evtHit.NewEvent<EDT_Shake> ();
	}
}
