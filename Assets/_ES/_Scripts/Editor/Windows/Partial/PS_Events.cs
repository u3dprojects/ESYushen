using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class PS_Events {
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

	public void DrawEvents(){
		_DrawEvents4Effect ();
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
				List<EDT_Effect> lstEffect = m_cEvents.m_lEffects;
				int lens = lstEffect.Count;
				if (lens > 0)
				{
					for (int i = 0; i < lens; i++)
					{
						m_event_fodeOut.Add (false);
						_DrawOneEffect(i, lstEffect[i]);
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
		GUIStyle style = EditorStyles.label;
		style.alignment = TextAnchor.MiddleLeft;

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
