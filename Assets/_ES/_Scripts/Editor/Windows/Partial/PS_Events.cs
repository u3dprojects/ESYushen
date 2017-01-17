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

	public void DrawEvents(){
		GUIStyle style = EditorStyles.label;
		style.alignment = TextAnchor.MiddleLeft;

		_DrawEvents4Effect ();

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
				EG_GUIHelper.FEG_EndV();
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
			GUILayout.Label("触发时间:", GUILayout.Width(80));
			effect.m_fCastTime = EditorGUILayout.Slider(effect.m_fCastTime, 0, this.m_wSkill.m_midRight.duration);
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
		if (m_wSkill.m_eCsJoin == null) {
			isEffectJoinSelf = true;
		} else {
			if (!isEffectJoinSelf) {
				effect.m_trsfParent = m_wSkill.m_eCsJoin.jointArray[effect.m_iJoint];
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
/// 类名 : 绘制时间事件 之 打击事件
/// 作者 : Canyon
/// 日期 : 2017-01-16 11:30
/// 功能 : 
/// </summary>
public partial class PS_Events {

	List<bool> m_hurt_fodeOut = new List<bool>();

	List<bool> m_hurtArea_fodeOut = new List<bool>();

	void _DrawEvents4Hurt(){
		EG_GUIHelper.FG_BeginVAsArea();
		{
			{
				// 上
				EG_GUIHelper.FEG_BeginH();
				Color def = GUI.backgroundColor;
				GUI.backgroundColor = Color.black;
				GUI.color = Color.white;

				EditorGUILayout.LabelField("打击点列表", EditorStyles.textArea);

				GUI.backgroundColor = def;

				GUI.color = Color.green;
				if (GUILayout.Button("+", GUILayout.Width(50)))
				{
					m_cEvents.NewHurt ();
				}
				GUI.color = Color.white;
				EG_GUIHelper.FEG_EndV();
			}

			{
				// 中
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
				m_hurt_fodeOut[index] = EditorGUILayout.Foldout(m_hurt_fodeOut[index], "打击点 - ");
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
			GUILayout.Label("触发时间:", GUILayout.Width(80));
			hurt.m_fCastTime = EditorGUILayout.Slider(hurt.m_fCastTime, this.m_wSkill.m_midRight.beforeRoll, this.m_wSkill.m_midRight.afterRoll);
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

		EG_GUIHelper.FEG_BeginVAsArea ();
		{
			//伤害区域
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
				EG_GUIHelper.FEG_EndV();
			}

			{
				// 中
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
				m_hurtArea_fodeOut[index] = EditorGUILayout.Foldout(m_hurtArea_fodeOut[index], "伤害区域 - ");
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
			hurtArea.m_fRange = EditorGUILayout.Slider (hurtArea.m_fRange,0f,1000f);
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
				hurtArea.m_fWidth = EditorGUILayout.Slider (hurtArea.m_fWidth,0f,1000f);
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
}