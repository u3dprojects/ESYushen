using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : 动作位移
/// 作者 : Canyon
/// 日期 : 2017-04-21 09:20
/// 功能 : 
/// </summary>
public class PS_StateMachineMove {
	// 位移
	bool isOpenMovPos = false;
	// AnimationCurve x_curve;
	// AnimationCurve y_curve;
	AnimationCurve z_curve;

	Vector3 preMovPos = Vector3.zero;
	Vector3 movPos = Vector3.zero;

	ED_Ani_YGame m_curAni = null;

	CharacterController m_myCtrl = null;

	Transform trsfEntity = null;

	void InitMovPosCurve()
	{
		m_curAni.cur_state_mache = m_curAni.GetStateMache<SpriteAniCurve>();
		bool isNotNull = m_curAni.cur_state_mache != null;
		isOpenMovPos = isNotNull;
		syncMache(true);

		if (!isNotNull)
		{
			DefCurve();
		}
	}

	void DefCurve()
	{
		// x_curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0));
		// y_curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0));
		z_curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0));
	}

	void syncMache(bool isReverse = false)
	{
		if (m_curAni.cur_state_mache == null)
		{
			return;
		}

		SpriteAniCurve temp = m_curAni.cur_state_mache as SpriteAniCurve;
		if (isReverse)
		{
			// x_curve = temp.x;
			// y_curve = temp.y;
			z_curve = temp.z;
		}
		else
		{
			// temp.x = x_curve;
			// temp.y = y_curve;
			temp.z = z_curve;
		}
	}


	void SaveMache() { 
		if(m_curAni.cur_state_mache == null)
		{
			m_curAni.cur_state_mache = m_curAni.AddStateMache<SpriteAniCurve>();
		}
		syncMache();
	}

	void RemoveMache() {
		m_curAni.RemoveStateMache<SpriteAniCurve>();
		m_curAni.cur_state_mache = null;
		DefCurve();
	}

	void _DrawMovPos()
	{
		EG_GUIHelper.FEG_BeginH();
		{
			EG_GUIHelper.FEG_BeginToggleGroup("开启位移??", ref isOpenMovPos);
			{
				InitMovPosCurve();

				EG_GUIHelper.FEG_BeginV();
				{
					EG_GUIHelper.FEG_BeginH();
					GUI.color = Color.cyan;
					if (GUILayout.Button("SaveCurveMache"))
					{
						SaveMache();
					}

					GUI.color = Color.red;
					if (GUILayout.Button("RemoveCurveMache"))
					{
						RemoveMache();
					}
					GUI.color = Color.white;
					EG_GUIHelper.FEG_EndH();

					EG_GUIHelper.FG_Space(6);

					//				x_curve = EditorGUILayout.CurveField("x", x_curve);
					//				EG_GUIHelper.FG_Space(7);
					
					//				y_curve = EditorGUILayout.CurveField("y", y_curve);
					//				EG_GUIHelper.FG_Space(7);

					z_curve = EditorGUILayout.CurveField("z", z_curve);
				}
				EG_GUIHelper.FEG_EndV();
			}
			EG_GUIHelper.FEG_EndToggleGroup();
		}
		EG_GUIHelper.FEG_EndH();

		syncMache();
	}

	public void Init(ED_Ani_YGame ani,Transform trsf,CharacterController ctrl = null,bool isOpen = false){
		this.m_curAni = ani;
		this.trsfEntity = trsf;
		this.m_myCtrl = ctrl;
		this.isOpenMovPos = isOpen;
		preMovPos = Vector3.zero;
	}

	public void DrawMvStateMachine(){
		_DrawMovPos ();
	}

	public void OnUpdate(float time){
		// 设置位移
		if (isOpenMovPos)
		{
			movPos = Vector3.zero;
			// movPos.x = x_curve.Evaluate(this.m_curAni.nt01);
			// movPos.y = y_curve.Evaluate(this.m_curAni.nt01);
			movPos.z = z_curve.Evaluate(time);
			// Debug.Log (z_curve.length);

			if(m_myCtrl != null && m_myCtrl.enabled)
			{
				m_myCtrl.Move(movPos - preMovPos);
			}else
			{
				trsfEntity.Translate(movPos - preMovPos);
			}
			preMovPos = movPos;
		}
	}
}
