using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 类名 : 绘制 伤害区域事件
/// 作者 : Canyon
/// 日期 : 2017-01-21 09:30
/// 功能 : 
/// </summary>
public class PS_EvtHurtArea {

	System.Action m_callNew;
	System.Action<EDT_Hurt_Area> m_callRemove;
	List<EDT_Hurt_Area> list =  null;
	bool m_isPlan;
	float duration;
	string m_title;

	bool m_isDrawTime = false;

	List<bool> m_lFodeout = new List<bool>();

	public PS_EvtHurtArea(string m_title,bool m_isPlan,System.Action m_callNew,System.Action<EDT_Hurt_Area> m_callRemove,bool isDrawTime){
		this.m_title = m_title;
		this.m_callNew = m_callNew;
		this.m_callRemove = m_callRemove;
		this.m_isPlan = m_isPlan;
		this.m_isDrawTime = isDrawTime;
	}

	public void DoDraw(float duration,List<EDT_Hurt_Area> list){
		this.duration = duration;
		this.list = list;
		_DrawEvents ();
	}

	void _DrawEvents(){
		EG_GUIHelper.FG_BeginVAsArea();
		{
			{
				// 上
				EG_GUIHelper.FEG_Head(m_title,true,this.m_callNew);

				EditorGUILayout.LabelField(EMT_TBases.ToArrayJsonString(this.list), EditorStyles.textArea);
				EG_GUIHelper.FG_Space(8);
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

	void _DrawOneEvnet(int index, EDT_Hurt_Area one)
	{
		
		EG_GUIHelper.FEG_BeginV();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				m_lFodeout[index] = EditorGUILayout.Foldout(m_lFodeout[index], "伤害区域 - " + EnumExtension.GetDescription(one.m_emTag));
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

			if (m_lFodeout.Count > index && m_lFodeout[index])
			{
				_DrawOneEventAttrs(one);
			}
		}
		EG_GUIHelper.FEG_EndV();
	}

	void _DrawOneEventAttrs(EDT_Hurt_Area one)
	{
		if (this.m_isDrawTime) {
			EG_GUIHelper.FEG_BeginH ();
			{
				GUILayout.Label ("触发时间:");
				if (m_isPlan) {
					one.m_fCastTime = EditorGUILayout.Slider (one.m_fCastTime, 0, duration);
				} else {
					one.m_fCastTime = EditorGUILayout.FloatField (one.m_fCastTime);
				}
			}
			EG_GUIHelper.FEG_EndH ();
			EG_GUIHelper.FG_Space (5);
		}

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("类型:", GUILayout.Width(80));
			one.m_emTag = (EDT_Hurt_Area.HurtAreaType)EditorGUILayout.EnumPopup ((System.Enum)one.m_emTag);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			one.m_isShowArea = EditorGUILayout.Toggle ("是否绘制伤害区域??", one.m_isShowArea);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("区域颜色:", GUILayout.Width(80));
			one.m_cAreaColor = EditorGUILayout.ColorField (one.m_cAreaColor);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			one.m_v3Offset = EditorGUILayout.Vector3Field("偏移(y暂留):", one.m_v3Offset);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		string strDesc = one.m_emTag == EDT_Hurt_Area.HurtAreaType.Rectangle ? "长度:" : "半径:";

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label(strDesc, GUILayout.Width(80));
			one.m_fRange = EditorGUILayout.Slider (one.m_fRange,0f,100f);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		if (one.m_emTag != EDT_Hurt_Area.HurtAreaType.Arc && one.m_emTag != EDT_Hurt_Area.HurtAreaType.Rectangle) {
			return;
		}

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("旋转角度:", GUILayout.Width(80));
			one.m_fRotation = EditorGUILayout.Slider (one.m_fRotation,0f,360f);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		switch (one.m_emTag) {
		case EDT_Hurt_Area.HurtAreaType.Rectangle:
			EG_GUIHelper.FEG_BeginH();
			{
				GUILayout.Label("宽度:", GUILayout.Width(80));
				one.m_fWidth = EditorGUILayout.Slider (one.m_fWidth,0f,100f);
			}
			EG_GUIHelper.FEG_EndH();
			EG_GUIHelper.FG_Space(5);
			break;
		case EDT_Hurt_Area.HurtAreaType.Arc:
			EG_GUIHelper.FEG_BeginH();
			{
				GUILayout.Label("弧度的角度值:", GUILayout.Width(80));
				one.m_fAngle = EditorGUILayout.Slider (one.m_fAngle,0f,360f);
			}
			EG_GUIHelper.FEG_EndH();
			EG_GUIHelper.FG_Space(5);
			break;
		}
	}
}
