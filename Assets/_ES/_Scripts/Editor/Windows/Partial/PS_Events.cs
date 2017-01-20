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

	List<bool> m_event_fodeOut = new List<bool>();

	// 特效挂节点
	// int ms_iJoin = 0;
	string[] JoinType = {
		"原点",
		"头部",
		"胸部",
		"腰部",
		"左手心",
		"右手心",
		"左武器攻击点",
		"右武器攻击点"
	};

	bool isEffectJoinSelf = false;

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
		m_cEvents.ResetOwner (this.m_wSkill.trsfEntity);
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

	void OnSceneGUI(SceneView sceneView){
		m_cEvents.ResetOwner (this.m_wSkill.trsfEntity);
		m_cEvents.OnSceneGUI ();
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

	void _DrawEvents4Effect(){
		EG_GUIHelper.FG_BeginVAsArea();
		{
			{
				// 上
				EG_GUIHelper.FEG_BeginH();
				Color def = GUI.backgroundColor;
				GUI.backgroundColor = Color.black;
				GUI.color = Color.white;

				EditorGUILayout.LabelField("特效列表", EditorStyles.textArea);

				GUI.backgroundColor = def;

				GUI.color = Color.green;
				if (GUILayout.Button("+", GUILayout.Width(50)))
				{
					m_cEvents.NewEffect();
				}
				GUI.color = Color.white;
				EG_GUIHelper.FEG_EndH();
			}

			{
				// 中
				List<EDT_Effect> list = m_cEvents.m_lEffects;
				int lens = list.Count;
				if (lens > 0)
				{
					for (int i = 0; i < lens; i++)
					{
						m_event_fodeOut.Add (false);
						_DrawOneEffect(i, list[i]);
					}
				}
				else
				{
					m_event_fodeOut.Clear();
				}
			}
		}
		EG_GUIHelper.FG_EndV();
	}

	void _DrawOneEffect(int index, EDT_Effect effect)
	{
		bool isEmptyName = string.IsNullOrEmpty(effect.m_sName);

		EG_GUIHelper.FEG_BeginV();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				m_event_fodeOut[index] = EditorGUILayout.Foldout(m_event_fodeOut[index], "特效 - " + (isEmptyName ? "未指定" : effect.m_sName));
				GUI.color = Color.red;
				if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(50)))
				{
					m_cEvents.RmEvent(effect);
					m_event_fodeOut.RemoveAt(index);
				}
				GUI.color = Color.white;
			}
			EG_GUIHelper.FEG_EndH();

			EG_GUIHelper.FG_Space(5);

			if (m_event_fodeOut[index])
			{
				_DrawOneEffectAttrs(effect);
			}
		}
		EG_GUIHelper.FEG_EndV();
	}

	void _DrawOneEffectAttrs(EDT_Effect effect)
	{

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("特效文件:", GUILayout.Width(80));
			effect.m_objOrg = EditorGUILayout.ObjectField(effect.m_objOrg, typeof(GameObject), false) as GameObject;
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{	
			GUILayout.Label("触发时间:");
			if (m_isPlan) {
				effect.m_fCastTime = EditorGUILayout.Slider(effect.m_fCastTime, 0, duration);
			} else {
				effect.m_fCastTime = EditorGUILayout.FloatField (effect.m_fCastTime);
			}
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		if (this.m_isPlan) {
			EG_GUIHelper.FEG_BeginH ();
			{
				GUILayout.Label ("持续时间:", GUILayout.Width (80));
				effect.m_fDuration = EditorGUILayout.FloatField (effect.m_fDuration);
			}
			EG_GUIHelper.FEG_EndH ();

			EG_GUIHelper.FG_Space (5);
		}
		_DrawOneEffectJoinPos(effect);
	}

	void _DrawOneEffectJoinPos(EDT_Effect effect)
	{
		EG_GUIHelper.FEG_BeginH();
		effect.m_iJoint = EditorGUILayout.Popup("挂节点:", effect.m_iJoint, JoinType);
		if (m_wSkill != null) {
			if (m_wSkill.m_eCsJoin == null) {
				isEffectJoinSelf = true;
			} else {
				if (!isEffectJoinSelf) {
					effect.m_trsfParent = m_wSkill.m_eCsJoin.jointArray [effect.m_iJoint];
				}
			}
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			effect.m_isFollow = EditorGUILayout.Toggle("是否跟随:", effect.m_isFollow);
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			EG_GUIHelper.FEG_BeginToggleGroup("手动位置??", ref isEffectJoinSelf);
			effect.m_trsfParent = EditorGUILayout.ObjectField("位置:", effect.m_trsfParent, typeof(Transform), isEffectJoinSelf) as Transform;
			EG_GUIHelper.FEG_EndToggleGroup();
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			effect.m_v3OffsetPos = EditorGUILayout.Vector3Field("偏移:", effect.m_v3OffsetPos);
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			effect.m_v3EulerAngle = EditorGUILayout.Vector3Field("旋转:", effect.m_v3EulerAngle);
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			EG_GUIHelper.FG_Label("缩放:");
			effect.m_fScale = EditorGUILayout.FloatField(effect.m_fScale);
		}
		EG_GUIHelper.FEG_EndH();
	}

}

/// <summary>
/// 类名 : 绘制时间事件 之 技能音效
/// 作者 : Canyon
/// 日期 : 2017-01-17 17:30
/// 功能 : 
/// </summary>
public partial class PS_Events {
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

				EditorGUILayout.LabelField("音效列表", EditorStyles.textArea);

				GUI.backgroundColor = def;

				GUI.color = Color.green;
				if (GUILayout.Button("+", GUILayout.Width(50)))
				{
					m_cEvents.NewAudio();
				}
				GUI.color = Color.white;
				EG_GUIHelper.FEG_EndH();
			}

			{
				// 中
				List<EDT_Audio> list = m_cEvents.m_lAudios;
				int lens = list.Count;
				if (lens > 0)
				{
					for (int i = 0; i < lens; i++)
					{
						m_audio_fodeOut.Add (false);
						_DrawOneAudio(i, list[i]);
					}
				}
				else
				{
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
					m_cEvents.RmEvent(audio);
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

/// <summary>
/// 类名 : 绘制时间事件 之 打击事件
/// 作者 : Canyon
/// 日期 : 2017-01-16 11:30
/// 功能 : 
/// </summary>
public partial class PS_Events {

	#region ==== 打击点 ====

	List<bool> m_hurt_fodeOut = new List<bool>();

	List<bool> m_hurtArea_fodeOut = new List<bool>();

	List<bool> m_beHitStatus_fodeOut = new List<bool>();

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
					m_cEvents.NewHurt ();
				}
				GUI.color = Color.white;
				EG_GUIHelper.FEG_EndH();
			}

			{
				// 
				List<EDT_Hurt> list = m_cEvents.m_lHurts;
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

	void _DrawOneHurt_HurtAreas(EDT_Hurt hurt){
		EG_GUIHelper.FEG_BeginVAsArea ();
		{
			{
				// 上
				EG_GUIHelper.FEG_BeginH();
				Color def = GUI.backgroundColor;
				GUI.backgroundColor = Color.black;
				GUI.color = Color.white;

				EditorGUILayout.LabelField("伤害区域列表", EditorStyles.textArea);

				GUI.backgroundColor = def;

				GUI.color = Color.green;
				if (GUILayout.Button("+", GUILayout.Width(50)))
				{
					hurt.NewHurtArea ();
				}
				GUI.color = Color.white;
				EG_GUIHelper.FEG_EndH();
			}

			{
				List<EDT_Hurt_Area> list = hurt.GetAreaList();
				int lens = list.Count;
				if (lens > 0)
				{
					for (int i = 0; i < lens; i++)
					{
						m_hurtArea_fodeOut.Add (false);
						_DrawOneHurtArea(i, list[i],hurt);
					}
				}
				else
				{
					m_hurtArea_fodeOut.Clear();
				}
			}
		}
		EG_GUIHelper.FEG_EndV ();
		EG_GUIHelper.FG_Space(5);
	}

	void _DrawOneHurtArea(int index, EDT_Hurt_Area hurtArea,EDT_Hurt hurt)
	{

		EG_GUIHelper.FEG_BeginV();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				m_hurtArea_fodeOut[index] = EditorGUILayout.Foldout(m_hurtArea_fodeOut[index], "伤害区域 - " + index);
				GUI.color = Color.red;
				if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(50)))
				{
					hurt.RemoveHurtArea(hurtArea);
					m_hurtArea_fodeOut.RemoveAt(index);
				}
				GUI.color = Color.white;
			}
			EG_GUIHelper.FEG_EndH();

			EG_GUIHelper.FG_Space(5);

			if (m_hurtArea_fodeOut[index])
			{
				_DrawOneHurtAreaAttrs(hurtArea);
			}
		}
		EG_GUIHelper.FEG_EndV();
	}

	void _DrawOneHurtAreaAttrs(EDT_Hurt_Area hurtArea)
	{

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("类型:", GUILayout.Width(80));
			hurtArea.m_emType = (EDT_Hurt_Area.HurtAreaType)EditorGUILayout.EnumPopup ((System.Enum)hurtArea.m_emType);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			hurtArea.m_isShowArea = EditorGUILayout.Toggle ("是否绘制伤害区域??", hurtArea.m_isShowArea);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("区域颜色:", GUILayout.Width(80));
			hurtArea.m_cAreaColor = EditorGUILayout.ColorField (hurtArea.m_cAreaColor);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			hurtArea.m_v3Offset = EditorGUILayout.Vector3Field("偏移(y暂留):", hurtArea.m_v3Offset);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		string strDesc = hurtArea.m_emType == EDT_Hurt_Area.HurtAreaType.Rectangle ? "长度:" : "半径:";

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label(strDesc, GUILayout.Width(80));
			hurtArea.m_fRange = EditorGUILayout.Slider (hurtArea.m_fRange,0f,100f);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		if (hurtArea.m_emType != EDT_Hurt_Area.HurtAreaType.Arc && hurtArea.m_emType != EDT_Hurt_Area.HurtAreaType.Rectangle) {
			return;
		}

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("旋转角度:", GUILayout.Width(80));
			hurtArea.m_fRotation = EditorGUILayout.Slider (hurtArea.m_fRotation,0f,360f);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		switch (hurtArea.m_emType) {
		case EDT_Hurt_Area.HurtAreaType.Rectangle:
			EG_GUIHelper.FEG_BeginH();
			{
				GUILayout.Label("宽度:", GUILayout.Width(80));
				hurtArea.m_fWidth = EditorGUILayout.Slider (hurtArea.m_fWidth,0f,100f);
			}
			EG_GUIHelper.FEG_EndH();
			EG_GUIHelper.FG_Space(5);
			break;
		case EDT_Hurt_Area.HurtAreaType.Arc:
			EG_GUIHelper.FEG_BeginH();
			{
				GUILayout.Label("弧度的角度值:", GUILayout.Width(80));
				hurtArea.m_fAngle = EditorGUILayout.Slider (hurtArea.m_fAngle,0f,360f);
			}
			EG_GUIHelper.FEG_EndH();
			EG_GUIHelper.FG_Space(5);
			break;
		}
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

			EG_GUIHelper.FG_BeginVAsArea();
			{
				EG_GUIHelper.FEG_BeginH ();
				{
					GUILayout.Label ("命中音效:", GUILayout.Width (80));
				}
				EG_GUIHelper.FEG_EndH ();
				EG_GUIHelper.FG_Space (5);
				_DrawOneAudioAttrs (hurt.GetBeHitAudio ());
			}
			EG_GUIHelper.FG_EndV();
			EG_GUIHelper.FG_Space(5);

			_DrawBeHitStatus (hurt);
		}
		EG_GUIHelper.FG_EndV();
	}

	void _DrawBeHitStatus(EDT_Hurt hurt){
		EG_GUIHelper.FG_BeginVAsArea();
		{
			{
				// 上
				EG_GUIHelper.FEG_BeginH();
				Color def = GUI.backgroundColor;
				GUI.backgroundColor = Color.black;
				GUI.color = Color.white;

				EditorGUILayout.LabelField("命中状态列表", EditorStyles.textArea);

				GUI.backgroundColor = def;

				GUI.color = Color.green;
				if (GUILayout.Button("+", GUILayout.Width(50)))
				{
					hurt.NewBeHitStatus ();
				}
				GUI.color = Color.white;
				EG_GUIHelper.FEG_EndH();
			}

			{
				// 中
				List<EDT_Hurt_BeHitStatus> list = hurt.GetHitStatusList();
				int lens = list.Count;
				if (lens > 0)
				{
					for (int i = 0; i < lens; i++)
					{
						m_beHitStatus_fodeOut.Add (false);
						_DrawOneBeHitStatus(i, list[i],hurt);
					}
				}
				else
				{
					m_beHitStatus_fodeOut.Clear();
				}
			}
		}
		EG_GUIHelper.FG_EndV();
	}

	void _DrawOneBeHitStatus(int index, EDT_Hurt_BeHitStatus beStatus,EDT_Hurt hurt)
	{
		
		EG_GUIHelper.FEG_BeginV();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				m_beHitStatus_fodeOut[index] = EditorGUILayout.Foldout(m_beHitStatus_fodeOut[index], "命中状态 - " + beStatus.m_iGid);
				GUI.color = Color.red;
				if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(50)))
				{
					hurt.RemoveBeHitStatus (beStatus);
					m_beHitStatus_fodeOut.RemoveAt(index);
				}
				GUI.color = Color.white;
			}
			EG_GUIHelper.FEG_EndH();

			EG_GUIHelper.FG_Space(5);

			if (m_beHitStatus_fodeOut[index])
			{
				_DrawOneBeHitStatusAttrs(beStatus);
			}
		}
		EG_GUIHelper.FEG_EndV();
	}

	void _DrawOneBeHitStatusAttrs(EDT_Hurt_BeHitStatus beStatus)
	{

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("分类:", GUILayout.Width(80));
			beStatus.m_iGid = EditorGUILayout.IntField (beStatus.m_iGid);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("参数:", GUILayout.Width(80));
			beStatus.m_sPars = EditorGUILayout.TextArea (beStatus.m_sPars);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);
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
	List<bool> m_shake_fodeOut = new List<bool>();

	void _DrawEvents4Shake(){
		EG_GUIHelper.FG_BeginVAsArea();
		{
			{
				// 上
				EG_GUIHelper.FEG_BeginH();
				Color def = GUI.backgroundColor;
				GUI.backgroundColor = Color.black;
				GUI.color = Color.white;

				EditorGUILayout.LabelField("震屏列表", EditorStyles.textArea);

				GUI.backgroundColor = def;

				GUI.color = Color.green;
				if (GUILayout.Button("+", GUILayout.Width(50)))
				{
					m_cEvents.NewShake ();
				}
				GUI.color = Color.white;
				EG_GUIHelper.FEG_EndH();
			}

			{
				// 中
				List<EDT_Shake> list = m_cEvents.m_lShakes;
				int lens = list.Count;
				if (lens > 0)
				{
					for (int i = 0; i < lens; i++)
					{
						m_shake_fodeOut.Add (false);
						_DrawOneShake(i, list[i]);
					}
				}
				else
				{
					m_shake_fodeOut.Clear();
				}
			}
		}
		EG_GUIHelper.FG_EndV();
	}

	void _DrawOneShake(int index, EDT_Shake shake)
	{

		EG_GUIHelper.FEG_BeginV();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				m_shake_fodeOut[index] = EditorGUILayout.Foldout(m_shake_fodeOut[index], "震屏 - " + index);
				GUI.color = Color.red;
				if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(50)))
				{
					m_cEvents.RmEvent(shake);
					m_shake_fodeOut.RemoveAt(index);
				}
				GUI.color = Color.white;
			}
			EG_GUIHelper.FEG_EndH();

			EG_GUIHelper.FG_Space(5);

			if (m_shake_fodeOut[index])
			{
				_DrawOneShakeAttrs(shake);
			}
		}
		EG_GUIHelper.FEG_EndV();
	}

	void _DrawOneShakeAttrs(EDT_Shake shake)
	{
		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("触发时间:");
			if (m_isPlan) {
				shake.m_fCastTime = EditorGUILayout.Slider(shake.m_fCastTime, 0, duration);
			} else {
				shake.m_fCastTime = EditorGUILayout.FloatField (shake.m_fCastTime);
			}
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("持续时间:", GUILayout.Width(80));
			shake.m_fDuration = EditorGUILayout.Slider (shake.m_fDuration,0f,60f);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("震动强度:", GUILayout.Width(80));
			shake.m_fStrength = EditorGUILayout.Slider (shake.m_fStrength,0f,1f);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);
	}
}