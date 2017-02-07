using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// 类名 : P-Partial,S-Skill 策划工具:技能数据表
/// 作者 : Canyon
/// 日期 : 2017-01-09 09:10
/// 功能 : 
/// </summary>
public partial class PS_MidRight{

    #region  == Member Attribute ===

    EDW_Skill m_wSkill;

	ED_Ani_YGame m_curAni;

	// 时间
	EN_Time m_curTime;

    Vector2 scrollPos;
	float topDescH = 20;
	float botBtnH = 130;

    float minScrollH = 200;
    float curScrollH = 260;
    float minWidth = 440;
    float curWidth = 0;

    // 技能
    string pathOpenSkill = "";

    // 技能属性
	EG_Skill m_egSkill = new EG_Skill ();

	// 动作进度
	bool isCanCanCtrlProgress = false;
	bool isCtrlProgress = false;
	float cur_progress = 0.0f;
	float min_progress = 0.0f;
	float max_progress = 1.0f;

	// 速度控制
	float cur_speed = 1.0f;
	// bool isCanSetMinMaxSpeed = false;
	float min_speed = 0.0f;
	float max_speed = 3.0f;

	// 暂停按钮控制
	bool isPauseing = false;

	// 是否运行
	bool isRunnging = false;

    #endregion

	public PS_MidRight(){
		Messenger.AddListener ( EDW_Skill.MSG_Stop_Right, DoStop);
	}

	~ PS_MidRight(){
		Messenger.AddListener ( EDW_Skill.MSG_Stop_Right, DoStop);
	}

    public void DoInit(EDW_Skill org)
    {
        this.m_wSkill = org;
		this.m_wSkill.AddCall4Update(OnUpdate);

		m_egSkill.m_ePSEvents.DoInit(org);

		OnInitTime ();
    }

	void OnInitTime()
	{
		if (m_curTime == null)
			m_curTime = new EN_Time();

		m_curTime.DoReInit(false);
	}

	public void DoReset()
	{
		if (this.m_curAni == null) {
			this.m_curAni = new ED_Ani_YGame ();
		}

		this.m_curAni.DoReInit(this.m_wSkill.gobjEntity);
	}
    
    void RecokenWH()
    {
        if (this.m_wSkill != null)
        {
            // 100 - 是主界面顶部高度 20 - 是误差偏移
			curScrollH = (m_wSkill.position.height - 100) - topDescH - botBtnH;
            curScrollH = Mathf.Max(curScrollH, minScrollH);

            curWidth = (m_wSkill.position.width - 10) / 2;
            curWidth = Mathf.Max(curWidth, minWidth);
        }
    }

    public void DrawShow()
    {
        RecokenWH();

        EG_GUIHelper.FEG_BeginVArea(curWidth);
        {
            _DrawDesc();
            
            if (this.m_wSkill.gobjEntity)
            {
                EG_GUIHelper.FEG_BeginScroll(ref scrollPos, 0, 0, curScrollH);

                if (this.m_wSkill._DrawAniJudged()) { 
                    _DrawSearchSkillExcel();

					m_egSkill.DrawShow();
                }

                EG_GUIHelper.FEG_EndScroll();

				_DrawAniCurSpeed();

				_DrawCtrlAniStateProgress ();

				_DrawOptBtns();

				_DrawOptExcelBtns ();
            }
        }
        EG_GUIHelper.FEG_EndV();
    }

    void _DrawDesc()
    {
        EG_GUIHelper.FEG_BeginH();

        GUIStyle style = EditorStyles.label;
        style.alignment = TextAnchor.MiddleCenter;
        string txtDecs = "策划工具:技能数据表";
        GUILayout.Label(txtDecs, style);

        EG_GUIHelper.FEG_EndH();
    }

    void _DrawSearchSkillExcel()
    {
        EG_GUIHelper.FEG_BeginH();
        if (GUILayout.Button("选取Excel(Skill)"))
        {
            this.pathOpenSkill = UnityEditor.EditorUtility.OpenFilePanel("选取excel文件", "", "xls");
			m_egSkill.DoInit(this.pathOpenSkill);
        }
        EG_GUIHelper.FEG_EndH();

        EG_GUIHelper.FG_Space(5);

        if (string.IsNullOrEmpty(this.pathOpenSkill))
        {
            EG_GUIHelper.FEG_BeginH();
            EditorGUILayout.HelpBox("请单击选取Excel,目前不能根据ID进行查询?", MessageType.Error);
            EG_GUIHelper.FEG_EndH();
        }else
        {
            EG_GUIHelper.FEG_BeginH();
            EG_GUIHelper.FG_Label("SkillPath:" + this.pathOpenSkill);
            EG_GUIHelper.FEG_EndH();
        }

        EG_GUIHelper.FG_Space(5);
    }

    void _DrawOptExcelBtns()
    {
        EG_GUIHelper.FEG_BeginH();
        {
            if (GUILayout.Button("Save Excel"))
            {
				if (!m_egSkill.isInited)
                {
                    EditorUtility.DisplayDialog("Tips", "没有选则技能表，不能进行保存!", "Okey");
                    return;
                }

                string folder = Path.GetDirectoryName(this.pathOpenSkill);
                string fileName = Path.GetFileNameWithoutExtension(this.pathOpenSkill);
                string suffix = Path.GetExtension(this.pathOpenSkill);
                suffix = suffix.Replace(".", "");
                string savePath = UnityEditor.EditorUtility.SaveFilePanel("保存Excel文件", folder, fileName, suffix);

                if (string.IsNullOrEmpty(savePath))
                {
                    UnityEditor.EditorUtility.DisplayDialog("Tips", "The save path is Empty !!!", "Okey");
                    return;
                }

				m_egSkill.SaveExcel (savePath);
            }
        }
        EG_GUIHelper.FEG_EndH();
    }

	void _DrawAniCurSpeed()
	{
		EG_GUIHelper.FG_Space (8);
		EG_GUIHelper.FEG_BeginH();
		{
			GUIStyle style = EditorStyles.label;
			style.alignment = TextAnchor.MiddleLeft;
			EditorGUILayout.LabelField("当前速度:", style);

			cur_speed = EditorGUILayout.Slider(cur_speed, min_speed, max_speed);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space (8);
	}

	void OnResetProgress()
	{
		max_progress = m_curAni.CurLens;
		max_progress = max_progress > 0 ? max_progress : 1.0f;
	}

	void ReckonProgress(float normalizedTime)
	{
		cur_progress = (normalizedTime % 1);
		cur_progress = cur_progress * max_progress;
		// cur_progress = EDW_Skill.Round(cur_progress, 6);
	}

	void _DrawCtrlAniStateProgress()
	{
		EG_GUIHelper.FEG_BeginH();
		{
			EG_GUIHelper.FEG_BeginToggleGroup("控制动作进度??", ref isCtrlProgress);
			isCanCanCtrlProgress = isCtrlProgress;
			float cur_progress01 = cur_progress / max_progress;
			if (isCanCanCtrlProgress) {
				if (isCtrlProgress) {
					ReckonProgress (cur_progress01);
					m_curAni.isCompletedRound = false;
					m_curAni.DoPlayCurr (cur_progress01);
				} else {
					ReckonProgress (m_curAni.normalizedTime);
				}
			}

			GUIStyle style = EditorStyles.label;
			style.alignment = TextAnchor.MiddleRight;
			EditorGUILayout.LabelField("当前动作进度: " + cur_progress01, style);

			EG_GUIHelper.FG_Space(3);

			cur_progress = EditorGUILayout.Slider(cur_progress, min_progress, max_progress);
			EG_GUIHelper.FEG_EndToggleGroup();

			//GUIStyle style = new GUIStyle();
			//style.alignment = TextAnchor.MiddleRight;
			//style.normal.textColor = Color.yellow;
			//EditorGUILayout.LabelField("(勾选时，才可控制 [当前进度]！！！)", style);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space (8);
	}

	void _DrawOptBtns()
	{
		EG_GUIHelper.FEG_BeginH();
		{
			if (GUILayout.Button("Play"))
			{
				DoPlay();
			}

			if (GUILayout.Button(isPauseing ? "ReGo" : "Pause"))
			{
				isPauseing = !isPauseing;
				if (isPauseing)
				{
					DoPause();
				}
				else
				{
					DoResume();
				}
			}

			if (GUILayout.Button("Stop"))
			{
				DoStop();
			}
		}
		EG_GUIHelper.FEG_EndH();
	}

	void OnUpdate(){
		if (!isRunnging || isPauseing || isCtrlProgress)
			return;

		if (this.m_curAni == null)
			return;

		this.m_curTime.DoUpdateTime();
		this.m_curAni.DoUpdateAnimator(m_curTime.DeltaTime,cur_speed);
		m_egSkill.m_ePSEvents.OnUpdate (m_curTime.DeltaTime,cur_speed);

		// 设置位移
	}

	void DoPlay() {
		Messenger.Invoke (EDW_Skill.MSG_Stop_Left);

		m_curTime.DoStart ();
		this.m_curAni.DoReady (m_egSkill.ms_enity.ActId);
		OnResetProgress ();
		this.m_curAni.DoStart();

		isRunnging = true;
		isPauseing = false;
		isCtrlProgress = false;
		isCanCanCtrlProgress = true;

		m_egSkill.m_ePSEvents.DoStart();
	}

	void DoPause() {
		isPauseing = true;

		m_curTime.DoPause();
		m_egSkill.m_ePSEvents.DoPause();
	}

	void DoResume() {
		isPauseing = false;
		m_curTime.DoResume();
		m_egSkill.m_ePSEvents.DoResume();
	}

	void DoStop() {
		isRunnging = false;
		isCtrlProgress = false;
		isCanCanCtrlProgress = false;
		m_egSkill.m_ePSEvents.DoEnd();
	}
}
