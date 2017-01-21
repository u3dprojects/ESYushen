using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 类名 : 绘制时间事件 之 特效事件
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
		DrawEvents ();
	}

	void DrawEvents(){
		GUIStyle style = EditorStyles.label;
		style.alignment = TextAnchor.MiddleLeft;

		_DrawEvents4Audio ();

		EG_GUIHelper.FG_Space(10);

		_DrawEvents4Effect ();

		EG_GUIHelper.FG_Space(10);

		_DrawEvents4Shake ();

		EG_GUIHelper.FG_Space(10);

		_DrawEvents4Hurt ();

	}

	// 绘制 特效
	PS_EvtEffect m_psEffect;

	void _DrawEvents4Effect(){
		if (m_psEffect == null) {
			m_psEffect = new PS_EvtEffect("特效列表",m_isPlan,_NewEffect,RemoveEvent,true);
		}

		SpriteJoint m_eCsJoin = null;
		if (m_wSkill != null && m_wSkill.m_eCsJoin != null) {
			m_eCsJoin = m_wSkill.m_eCsJoin;
		}
		m_psEffect.DoDraw (duration, m_cEvents.GetLEffects(),m_eCsJoin);
	}

	void _NewEffect(){
		m_cEvents.NewEvent<EDT_Effect>();
	}
}

/// <summary>
/// 类名 : 绘制时间事件 之 技能音效
/// 作者 : Canyon
/// 日期 : 2017-01-17 17:30
/// 功能 : 
/// </summary>
public partial class PS_Events {
	// 绘制 音效
	PS_EvtAudio m_psAudio;
	void _DrawEvents4Audio(){
		if (m_psAudio == null) {
			m_psAudio = new PS_EvtAudio("音效列表",m_isPlan,_NewAudio,RemoveEvent,true);
		}
		m_psAudio.DoDraw (duration, m_cEvents.GetLAudios ());
	}

	void _NewAudio(){
		m_cEvents.NewEvent<EDT_Audio> ();
	}
}

/// <summary>
/// 类名 : 绘制时间事件 之 打击事件
/// 作者 : Canyon
/// 日期 : 2017-01-16 11:30
/// 功能 : 
/// </summary>
public partial class PS_Events {
	
	PS_EvtHurt m_psHitArea;

	void _DrawEvents4Hurt(){
		if (m_psHitArea == null) {
			m_psHitArea = new PS_EvtHurt("目标事件列表",m_isPlan,_NewHitArea,RemoveEvent,true);
		}
		m_psHitArea.DoDraw (beforeRoll,afterRoll, m_cEvents.GetLHurts ());
	}

	void _NewHitArea(){
		m_cEvents.NewEvent<EDT_Hurt> ();
	}
}


/// <summary>
/// 类名 : 绘制时间事件 之 技能震屏
/// 作者 : Canyon
/// 日期 : 2017-01-18 18:10
/// 功能 : 
/// </summary>
public partial class PS_Events {

	PS_EvtShake m_psShake;

	void _DrawEvents4Shake(){
		if (m_psShake == null) {
			m_psShake = new PS_EvtShake("震屏列表", m_isPlan, _NewShake, RemoveEvent,true);
		}
		m_psShake.DoDraw (duration, m_cEvents.GetLShakes());
	}

	void _NewShake(){
		m_cEvents.NewEvent<EDT_Shake> ();
	}
}