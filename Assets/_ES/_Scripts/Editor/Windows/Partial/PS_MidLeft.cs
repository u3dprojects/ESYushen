using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 类名 : P-Partial,S-Skill 美术工具:动作-特效预览
/// 作者 : Canyon
/// 日期 : 2017-01-06 09:10
/// 功能 : 
/// </summary>
public class PS_MidLeft{

    #region  == Member Attribute ===

    EDW_Skill m_wSkill;

    ED_Ani_YGame m_curAni
    {
        get
        {
            return m_wSkill.me_ani;
        }
    }

    CharacterController m_myCtrl
    {
        get { return m_wSkill.m_myCtrl; }
    }

    Transform trsfEntity
    {
        get { return m_wSkill.trsfEntity; }
    }

    EN_Time m_curTime;
    
    Vector2 scrollPos;
    float minScrollH = 260;
    float curScrollH = 260;
    float minWidth = 440;
    float curWidth = 0;

	float botBtnH = 45;

    // popup 列表选择值
    int ind_popup = 0;
    int pre_ind_popup = -1;

    // 速度控制
    float cur_speed = 1.0f;
    bool isCanSetMinMaxSpeed = false;
    float min_speed = 0.0f;
    float max_speed = 3.0f;

    // 动作进度
	bool isCanCanCtrlProgress = false;
    bool isCtrlProgress = false;
    float cur_progress = 0.0f;
    float min_progress = 0.0f;
    float max_progress = 1.0f;

    // 循环次数
    bool isRound = false;
    int round_times = 1;

	// 动作位移
	PS_StateMachineMove m_ePSMove = new PS_StateMachineMove();

    // 事件
	PS_Events m_ePSEvents = new PS_Events();

    // 暂停按钮控制
    bool isPauseing = false;

    // 是否运行
    bool isRunnging = false;

    #endregion

	public PS_MidLeft(){
		Messenger.AddListener ( EDW_Skill.MSG_Stop_Left, DoStop);
	}

	~ PS_MidLeft(){
		Messenger.RemoveListener( EDW_Skill.MSG_Stop_Left, DoStop);
	}

    public void DoInit(EDW_Skill org)
    {
        this.m_wSkill = org;
        this.m_wSkill.AddCall4Update(OnUpdate);

		m_ePSEvents.DoInit(org);

        OnInitTime();
    }


    void OnInitTime()
    {
        if (m_curTime == null)
            m_curTime = new EN_Time();

        m_curTime.DoReInit(false);
    }

    public void DoReset()
    {
        ind_popup = 0;
        pre_ind_popup = -1;
        
		m_ePSEvents.DoClear();
    }

    void OnResetProgress()
    {
        max_progress = m_curAni.CurLens;
        max_progress = max_progress > 0 ? max_progress : 1.0f;
    }

    void RecokenWH()
    {
        if (this.m_wSkill != null)
        {
            // 100 - 是主界面顶部高度 20 - 是误差偏移
			curScrollH = (m_wSkill.position.height - 100) - botBtnH;
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

                _DrawFreshBtn();

                if (this.m_wSkill._DrawAniJudged())
                {
                    _DrawAniList();

                    _DrawAniStateInfo();

                    _DrawAniMinMaxSpeed();

                    _DrawAniCurSpeed();

                    _DrawCtrlAniStateProgress();

                    _DrawRoundTimes();

					m_ePSMove.DrawMvStateMachine ();

                    _DrawEvents();
                }

                EG_GUIHelper.FEG_EndScroll();

				_DrawOptBtns();
            }
        }
        EG_GUIHelper.FEG_EndV();
    }

    void _DrawDesc()
    {
        EG_GUIHelper.FEG_BeginH();

        GUIStyle style = EditorStyles.label;
        style.alignment = TextAnchor.MiddleCenter;
        string txtDecs = "美术工具:动作预览(可添加特效)";
        GUILayout.Label(txtDecs, style);

        EG_GUIHelper.FEG_EndH();
    }

    void _DrawFreshBtn()
    {
        EG_GUIHelper.FEG_BeginH();
        if (GUILayout.Button("刷新Animator动作列表List"))
        {
            m_wSkill.OnInitEnAni();
        }
        EG_GUIHelper.FEG_EndH();
    }
    
    void _DrawAniList()
    {
        EG_GUIHelper.FEG_BeginH();
        GUIStyle style = EditorStyles.label;
        style.alignment = TextAnchor.MiddleLeft;
        EditorGUILayout.LabelField("动画列表:",style);

        style = EditorStyles.popup;
        style.alignment = TextAnchor.MiddleRight;
        ind_popup = EditorGUILayout.Popup(ind_popup, m_curAni.Keys.ToArray(), style);
        if (pre_ind_popup != ind_popup)
        {
            pre_ind_popup = ind_popup;
            m_curAni.SetSpeed(1.0f);
            m_curAni.ResetAniState(ind_popup);

            OnResetProgress();
        }
        EG_GUIHelper.FEG_EndH();
    }

    void _DrawAniStateInfo()
    {
        EG_GUIHelper.FEG_BeginH();
        GUIStyle style = EditorStyles.label;
        style.alignment = TextAnchor.MiddleLeft;

        GUILayoutOption minW = GUILayout.MinWidth(90);
        GUILayout.Label("动画帧数: " + m_curAni.CurFrameCount, style, minW);

        GUILayout.Label("动画时长: " + m_curAni.CurLens + " s", style, minW);

        style.alignment = TextAnchor.MiddleRight;
        EditorGUILayout.LabelField("动画帧率: " + m_curAni.CurFrameRate + " 帧/s", style);
        EG_GUIHelper.FEG_EndH();
    }

    void _DrawAniMinMaxSpeed()
    {
        EG_GUIHelper.FEG_BeginH();
        {
            EG_GUIHelper.FEG_BeginToggleGroup("MinMax速度??", ref isCanSetMinMaxSpeed);
            min_speed = EditorGUILayout.FloatField("Min速度:", min_speed);
            EG_GUIHelper.FG_Space(3);
            max_speed = EditorGUILayout.FloatField("Max速度:", max_speed);
            EG_GUIHelper.FEG_EndToggleGroup();

            if (max_speed < min_speed)
            {
                max_speed = min_speed;
            }
        }
        EG_GUIHelper.FEG_EndH();
    }

    void _DrawAniCurSpeed()
    {
        EG_GUIHelper.FEG_BeginH();
        {
            GUIStyle style = EditorStyles.label;
            style.alignment = TextAnchor.MiddleLeft;
            EditorGUILayout.LabelField("当前速度:", style);
            
            cur_speed = EditorGUILayout.Slider(cur_speed, min_speed, max_speed);
        }
        EG_GUIHelper.FEG_EndH();
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
    }

    void _DrawRoundTimes()
    {
        EG_GUIHelper.FEG_BeginH();
        {
            EG_GUIHelper.FEG_BeginToggleGroup("控制循环次数??", ref isRound);
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleRight;
            style.normal.textColor = Color.cyan;

            string desc = string.Format("已播放了{0:D2}次!", m_curAni.CurLoopCount);
            EditorGUILayout.LabelField(desc, style);

            EG_GUIHelper.FG_Space(3);

            round_times = EditorGUILayout.IntField("循环次数:", round_times);

            EG_GUIHelper.FEG_EndToggleGroup();
            
        }
        EG_GUIHelper.FEG_EndH();
    }

    void _DrawEvents()
    {
		m_ePSEvents.DrawEvents(m_curAni.CurLens,0,m_curAni.CurLens);
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


    void OnUpdate()
    {
        if (!isRunnging || isPauseing || isCtrlProgress)
            return;

        if (this.m_curAni == null)
            return;

        this.m_curTime.DoUpdateTime();

		this.m_curAni.m_LoopTimes = this.isRound ? this.round_times : 0;
        this.m_curAni.DoUpdateAnimator(m_curTime.DeltaTime,cur_speed);

		m_ePSEvents.OnUpdate (m_curTime.DeltaTime, cur_speed);

        // 设置位移
		m_ePSMove.OnUpdate(this.m_curAni.nt01);
    }

    void DoPlay() {
		Messenger.Invoke (EDW_Skill.MSG_Stop_Right);

		m_ePSMove.Init (m_curAni,trsfEntity, m_myCtrl);

		m_curAni.DoStart(null,(isloop) => {
			if(trsfEntity != null)
				trsfEntity.position = Vector3.zero;
		});
		m_curTime.DoStart ();

        isRunnging = true;
        isPauseing = false;
		isCtrlProgress = false;
		isCanCanCtrlProgress = true;

		m_ePSEvents.DoStart();
    }

    void DoPause() {
        isPauseing = true;

		m_ePSEvents.DoPause();
    }

    void DoResume() {
        isPauseing = false;
		m_ePSEvents.DoResume();
    }

    void DoStop() {
        isRunnging = false;
		isCtrlProgress = false;
		isCanCanCtrlProgress = false;
		if(trsfEntity != null)
			trsfEntity.position = Vector3.zero;
		m_ePSEvents.DoEnd();
    }
}
