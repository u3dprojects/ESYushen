using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 类名 : 绘制 特效事件
/// 作者 : Canyon
/// 日期 : 2017-01-10 10:10
/// 功能 : 
/// </summary>
public class PS_EvtEffect {

	System.Action m_callNew;
	System.Action<EDT_Effect> m_callRemove;
	List<EDT_Effect> list;
	bool m_isPlan;
	float duration;
	string m_title;
	SpriteJoint m_eCsJoin;

	bool isInit = false;

	List<bool> m_lFodeout = new List<bool>();

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

	bool isJoinTrsf = false;

	public void DoInit(string m_title,bool m_isPlan,System.Action m_callNew,System.Action<EDT_Effect> m_callRemove){
		if (isInit)
			return;
		
		isInit = true;
		this.m_title = m_title;
		this.m_callNew = m_callNew;
		this.m_callRemove = m_callRemove;
		this.m_isPlan = m_isPlan;
	}

	public void DoDraw(float duration,List<EDT_Effect> list,SpriteJoint m_eCsJoin = null){
		this.duration = duration;
		this.list = list;
		this.m_eCsJoin = m_eCsJoin;

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

	void _DrawOneEvnet(int index, EDT_Effect one)
	{

		bool isEmptyName = string.IsNullOrEmpty(one.m_sName);

		EG_GUIHelper.FEG_BeginV();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				m_lFodeout[index] = EditorGUILayout.Foldout(m_lFodeout[index], "特效 - " + (isEmptyName ? "未指定" : one.m_sName));
				GUI.color = Color.red;
				if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(50)))
				{
					if (this.m_callRemove != null) {
						this.m_callRemove (one);
					}
					m_lFodeout.RemoveAt(index);
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

	void _DrawOneEventAttrs(EDT_Effect one)
	{

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("特效文件:", GUILayout.Width(80));
			one.m_objOrg = EditorGUILayout.ObjectField(one.m_objOrg, typeof(GameObject), false) as GameObject;
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{	
			GUILayout.Label("触发时间:");
			if (m_isPlan) {
				one.m_fCastTime = EditorGUILayout.Slider(one.m_fCastTime, 0, duration);
			} else {
				one.m_fCastTime = EditorGUILayout.FloatField (one.m_fCastTime);
			}
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		if (this.m_isPlan) {
			EG_GUIHelper.FEG_BeginH ();
			{
				GUILayout.Label ("持续时间:", GUILayout.Width (80));
				one.m_fDuration = EditorGUILayout.FloatField (one.m_fDuration);
			}
			EG_GUIHelper.FEG_EndH ();

			EG_GUIHelper.FG_Space (5);
		}
		_DrawOneEffectJoinPos(one);
	}

	void _DrawOneEffectJoinPos(EDT_Effect one)
	{
		EG_GUIHelper.FEG_BeginH();
		one.m_iJoint = EditorGUILayout.Popup("挂节点:", one.m_iJoint, JoinType);
		if (m_eCsJoin == null) {
			isJoinTrsf = true;
		} else {
			if (!isJoinTrsf) {
				one.m_trsfParent = m_eCsJoin.jointArray [one.m_iJoint];
			}
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			one.m_isFollow = EditorGUILayout.Toggle("是否跟随:", one.m_isFollow);
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			EG_GUIHelper.FEG_BeginToggleGroup("手动位置??", ref isJoinTrsf);
			one.m_trsfParent = EditorGUILayout.ObjectField("位置:", one.m_trsfParent, typeof(Transform), isJoinTrsf) as Transform;
			EG_GUIHelper.FEG_EndToggleGroup();
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			one.m_v3OffsetPos = EditorGUILayout.Vector3Field("偏移:", one.m_v3OffsetPos);
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			one.m_v3EulerAngle = EditorGUILayout.Vector3Field("旋转:", one.m_v3EulerAngle);
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			EG_GUIHelper.FG_Label("缩放:");
			one.m_fScale = EditorGUILayout.FloatField(one.m_fScale);
		}
		EG_GUIHelper.FEG_EndH();
	}
}
