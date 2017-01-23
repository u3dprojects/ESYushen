using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 类名 : 绘制 子弹事件
/// 作者 : Canyon
/// 日期 : 2017-01-20 17:50
/// 功能 : 
/// </summary>
public class PS_EvtBullet {

	System.Action m_callNew;
	System.Action<EDT_Bullet> m_callRemove;
	List<EDT_Bullet> list;
	bool m_isPlan;
	float duration;
	string m_title;

	bool m_isDrawTime = false;

	List<bool> m_lFodeout = new List<bool>();

	public PS_EvtBullet(string m_title,bool m_isPlan,System.Action m_callNew,System.Action<EDT_Bullet> m_callRemove,bool isDrawTime){
		this.m_title = m_title;
		this.m_callNew = m_callNew;
		this.m_callRemove = m_callRemove;
		this.m_isPlan = m_isPlan;
		this.m_isDrawTime = isDrawTime;
	}

	public void DoDraw(float duration,List<EDT_Bullet> list){
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

	void _DrawOneEvnet(int index, EDT_Bullet one)
	{

		bool isEmptyName = one.m_iID <= 0;

		EG_GUIHelper.FEG_BeginV();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				m_lFodeout[index] = EditorGUILayout.Foldout(m_lFodeout[index], "Bullet - " + (isEmptyName ? "未指定" : ""+one.m_iID));
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

	void _DrawOneEventAttrs(EDT_Bullet one)
	{
		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("BulletID:", GUILayout.Width(80));
			one.m_iID = EditorGUILayout.IntField(one.m_iID);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

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
			one.m_v3OffsetPos = EditorGUILayout.Vector3Field("偏移:",one.m_v3OffsetPos);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("旋转角度:", GUILayout.Width(80));
			one.m_fRotation = EditorGUILayout.Slider (one.m_fRotation,0f,360f);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);
	}
}
