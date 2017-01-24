using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : 绘制 事件集合
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

	public float m_fDuration = 0;

	public SpriteJoint m_eCsJoin = null;

	public PS_EvtHitEvent(){}

	public PS_EvtHitEvent(bool isPlan,bool isShowCastTime){
		this.m_isPlan = isPlan;
		this.m_isShowCastTime = isShowCastTime;
	}

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

			_DrawEffects();
			EG_GUIHelper.FG_Space(10);

			_DrawAudio ();
			EG_GUIHelper.FG_Space(10);

			_DrawStay ();
			EG_GUIHelper.FG_Space (10);

			_DrawShake ();
			EG_GUIHelper.FG_Space (10);

			if (!this.m_isShowCastTime) {
				_DrawHitStatus ();
				EG_GUIHelper.FG_Space (10);

				_DrawAttrs ();
				EG_GUIHelper.FG_Space (10);

				_DrawBuffs ();
				EG_GUIHelper.FG_Space (10);
			}

			_DrawBullets ();
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

		psEvtAttrs.DoDraw (m_fDuration, m_evtHit.GetLAttrs());
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

		psEvtBuff.DoDraw (m_fDuration, m_evtHit.GetLBuffs());
	}

	void _NewBuff(){
		m_evtHit.NewEvent<EDT_Buff> ();
	}

	// 绘制 音效
	PS_EvtAudio psEvtAudio;
	void _DrawAudio(){
		if (psEvtAudio == null) {
			psEvtAudio = new PS_EvtAudio("音效列表:",m_isPlan,_NewAudio,_RmEvent,m_isShowCastTime);
		}

		psEvtAudio.DoDraw (m_fDuration, m_evtHit.GetLAudios());
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
		m_psShake.DoDraw (m_fDuration, m_evtHit.GetLShakes());
	}

	void _NewShake(){
		m_evtHit.NewEvent<EDT_Shake> ();
	}

	// 绘制 特效
	PS_EvtEffect m_psEffect;

	void _DrawEffects(){
		if (m_psEffect == null) {
			m_psEffect = new PS_EvtEffect("特效列表",m_isPlan,_NewEffect,_RmEvent,m_isShowCastTime);
		}
		m_psEffect.DoDraw (m_fDuration, m_evtHit.GetLEffects(),m_eCsJoin);
	}

	void _NewEffect(){
		m_evtHit.NewEvent<EDT_Effect>();
	}

	// 绘制 子弹
	PS_EvtBullet m_psBullet;

	void _DrawBullets(){
		if (m_psBullet == null) {
			m_psBullet = new PS_EvtBullet("子弹Bullet列表",m_isPlan,_NewBullet,_RmEvent,m_isShowCastTime);
		}
		m_psBullet.DoDraw(m_fDuration, m_evtHit.GetLBullets());
	}

	void _NewBullet(){
		m_evtHit.NewEvent<EDT_Bullet>();
	}

	// 顿帧
	PS_EvtStay m_psStay;

	void _DrawStay(){
		if (m_psStay == null) {
			m_psStay = new PS_EvtStay("顿帧列表", m_isPlan, _NewStay, _RmEvent,m_isShowCastTime);
		}
		m_psStay.DoDraw (m_fDuration, m_evtHit.GetLStays());
	}

	void _NewStay(){
		m_evtHit.NewEvent<EDT_Stay> ();
	}

	// 受击者受击时表现状态
	PS_EvtHitStatus m_psBeHitStatus;

	void _DrawHitStatus(){
		if (m_psBeHitStatus == null) {
			m_psBeHitStatus = new PS_EvtHitStatus("受击表现状态列表", m_isPlan, _NewHitStatus, _RmEvent,m_isShowCastTime);
		}
		m_psBeHitStatus.DoDraw (m_fDuration, m_evtHit.GetLHitStatuses());
	}

	void _NewHitStatus(){
		m_evtHit.NewEvent<EDT_HitStatus> ();
	}

}
