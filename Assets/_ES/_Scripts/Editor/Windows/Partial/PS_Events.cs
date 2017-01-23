using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 类名 : 绘制 技能的 时间事件
/// 作者 : Canyon
/// 日期 : 2017-01-10 10:10
/// 功能 : 
/// </summary>
public partial class PS_Events {
	#region  == Member Attribute ===

	EDW_Skill m_wSkill;

	// 是否是策划配置数据
	bool m_isPlan = false;

	EMT_Event m_cEvents = new EMT_Event ();

	SpriteJoint m_eCsJoin = null;
	PS_EvtHitEvent m_psCastEvt;

	// 总时长
	float duration = 0;

	// 前摇结束点
	float beforeRoll = 0;

	// 后摇开始点
	float afterRoll = 0;

	#endregion

	public PS_Events(){
	}

	public PS_Events(bool isPlan){
		m_isPlan = isPlan;
	}

	public void DoInit(EDW_Skill org){
		this.m_wSkill = org;
		this.m_wSkill.AddCall4SceneGUI (OnSceneGUI);
	}

	public void DoReInitEventJson(string json){
		m_cEvents.DoReInit (json);
	}

	public void DoStart(){
		m_cEvents.DoStart ();
	}

	public void DoPause(){
		m_cEvents.DoPause ();
	}

	public void DoResume(){
		m_cEvents.DoResume ();
	}

	public void OnUpdate(float deltatime,float speed){
		m_cEvents.SetSpeed (speed);
		m_cEvents.OnUpdate (deltatime);
	}

	public void DoEnd(){
		m_cEvents.DoEnd ();
	}

	public void DoClear(){
		m_cEvents.DoClear ();
		m_eCsJoin = null;

		_OnClearHit ();

		_OnClearCastEvt();
	}

	public string ToJsonString(){
		return m_cEvents.ToJsonString ();
	}

	void RemoveEvent(EDT_Base one){
		m_cEvents.RmEvent (one);
	}

	void OnSceneGUI(SceneView sceneView){
		m_cEvents.OnSceneGUI (this.m_wSkill.trsfEntity);
		sceneView.Repaint();
	}

	public void DrawEvents(float duration,float bef,float aft){
		this.duration = duration;
		this.beforeRoll = bef;
		this.afterRoll = aft;

		if (m_psCastEvt == null) {
			m_psCastEvt = new PS_EvtHitEvent (this.m_isPlan,true);
			m_psCastEvt.SetEvent (m_cEvents);
		}


		if (m_wSkill != null && m_wSkill.m_eCsJoin != null) {
			m_eCsJoin = m_wSkill.m_eCsJoin;
		} else {
			m_eCsJoin = null;
		}

		m_psCastEvt.m_fDuration = this.duration;
		m_psCastEvt.m_eCsJoin = m_eCsJoin;

		DrawEvents ();
	}

	void DrawEvents(){
		GUIStyle style = EditorStyles.label;
		style.alignment = TextAnchor.MiddleLeft;

		m_psCastEvt.DoDraw ();

		_DrawEvents4Hurt ();
	}

	void _OnClearCastEvt(){
		if (m_psCastEvt != null) {
			m_psCastEvt.DoClear ();
			m_psCastEvt = null;
		}
	}

	// 类名 : 绘制时间事件 之 打击事件

	PS_EvtHurt m_psHit;
	void _DrawEvents4Hurt(){
		if (m_psHit == null) {
			m_psHit = new PS_EvtHurt("目标事件列表",m_isPlan,_NewHitArea,RemoveEvent,true);
		}
		m_psHit.DoDraw (beforeRoll,afterRoll, m_cEvents.GetLHurts ());
	}

	void _NewHitArea(){
		m_cEvents.NewEvent<EDT_Hurt> ();
	}

	void _OnClearHit(){
		if (m_psHit != null) {
			m_psHit.DoClear ();
			m_psHit = null;
		}
	}
}

