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

	#region ==== 打击点 ====

	List<bool> m_hurt_fodeOut = new List<bool>();

	void _DrawEvents4Hurt(){
		EG_GUIHelper.FG_BeginVAsArea();
		{
			{
				// 上
				EG_GUIHelper.FEG_BeginH();
				Color def = GUI.backgroundColor;
				GUI.backgroundColor = Color.black;
				GUI.color = Color.white;

				EditorGUILayout.LabelField("依靠碰撞区域检测来确定攻击目标的列表", EditorStyles.textArea);

				GUI.backgroundColor = def;

				GUI.color = Color.green;
				if (GUILayout.Button("+", GUILayout.Width(50)))
				{
					m_cEvents.NewEvent<EDT_Hurt> ();
				}
				GUI.color = Color.white;
				EG_GUIHelper.FEG_EndH();
			}

			{
				// 
				List<EDT_Hurt> list = m_cEvents.GetLHurts();
				int lens = list.Count;
				if (lens > 0)
				{
					for (int i = 0; i < lens; i++)
					{
						m_hurt_fodeOut.Add (false);
						_DrawOneHurt(i, list[i]);
					}
				}
				else
				{
					m_hurt_fodeOut.Clear();
				}
			}
		}
		EG_GUIHelper.FG_EndV();
	}

	void _DrawOneHurt(int index, EDT_Hurt hurt)
	{
		
		EG_GUIHelper.FEG_BeginV();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				m_hurt_fodeOut[index] = EditorGUILayout.Foldout(m_hurt_fodeOut[index], "列表 - " + index);
				GUI.color = Color.red;
				if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(50)))
				{
					m_cEvents.RmEvent(hurt);
					m_hurt_fodeOut.RemoveAt(index);
				}
				GUI.color = Color.white;
			}
			EG_GUIHelper.FEG_EndH();

			EG_GUIHelper.FG_Space(5);

			if (m_hurt_fodeOut[index])
			{
				_DrawOneHurtAttrs(hurt);
			}
		}
		EG_GUIHelper.FEG_EndV();
	}

	void _DrawOneHurtAttrs(EDT_Hurt hurt)
	{
		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("触发时间:");
			if (m_isPlan) {
				hurt.m_fCastTime = EditorGUILayout.Slider(hurt.m_fCastTime, beforeRoll, afterRoll);
			} else {
				hurt.m_fCastTime = EditorGUILayout.FloatField (hurt.m_fCastTime);
			}
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("优先目标:", GUILayout.Width(80));
			hurt.m_iTargetFilter = EditorGUILayout.IntField (hurt.m_iTargetFilter);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("目标数量:", GUILayout.Width(80));
			hurt.m_iTargetCount = EditorGUILayout.IntField (hurt.m_iTargetCount);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		// 伤害区域
		_DrawOneHurt_HurtAreas(hurt);

		// 受击者
		_DrawOneHurt_BeHitter(hurt);
	}

	#endregion

	#region ==== 伤害区域 ====
	PS_EvtHurtArea m_psBeHitArea;
	void _DrawOneHurt_HurtAreas(EDT_Hurt hurt){
		if (m_psBeHitArea == null) {
			m_psBeHitArea = new PS_EvtHurtArea("伤害区域列表:",m_isPlan,delegate {
				hurt.NewHurtArea();
			},delegate (EDT_Hurt_Area one){
				hurt.RemoveHurtArea(one);
			},false);
		}

		m_psBeHitArea.DoDraw (duration, hurt.GetAreaList());
	}
	#endregion

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
	PS_EvtAudio m_psBeHitAudio;
	void _DrawBeHitAudio(EDT_Hurt hurt){
		if (m_psBeHitAudio == null) {
			m_psBeHitAudio = new PS_EvtAudio("命中音效:",m_isPlan,delegate {
				hurt.NewBeHitAudio();
			},delegate (EDT_Audio one){
				hurt.RemoveEvent(one);
			},false);
		}

		m_psBeHitAudio.DoDraw (duration, hurt.GetHitAudioList ());
	}

	// 绘制修改属性
	PS_EvtAttrs m_psBeHitAttrs;
	void _DrawBeHitStatus(EDT_Hurt hurt){
		if (m_psBeHitAttrs == null) {
			m_psBeHitAttrs = new PS_EvtAttrs("命中属性修改列表:",m_isPlan,delegate {
				hurt.NewBeHitStatus();
			},delegate (EDT_Property one){
				hurt.RemoveEvent(one);
			},false);
		}

		m_psBeHitAttrs.DoDraw (duration, hurt.GetHitStatusList ());
	}

	// 绘制 buff
	PS_EvtBuff m_psBeHitBuff;
	void _DrawBeHitBuffs(EDT_Hurt hurt){
		if (m_psBeHitBuff == null) {
			m_psBeHitBuff = new PS_EvtBuff("命中Buff列表:",m_isPlan,delegate {
				hurt.NewHitBuff();
			},delegate (EDT_Buff one){
				hurt.RemoveEvent(one);
			},false);
		}

		m_psBeHitBuff.DoDraw (duration, hurt.GetHitBuffList());
	}

	#endregion
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