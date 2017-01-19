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

    EN_SkillOpt optSkill
    {
        get { return EN_SkillOpt.Instance; }
    }

	// 时间
	EN_Time m_curTime;

	// 事件
	PS_Events m_ePSEvents = new PS_Events(true);

    Vector2 scrollPos;
	float topDescH = 20;
	float botBtnH = 45;

    float minScrollH = 260;
    float curScrollH = 260;
    float minWidth = 440;
    float curWidth = 0;

    // 技能
    string pathOpenSkill = "";

    // 技能属性
    EN_Skill ms_curSkill;

    int ms_ID;
    string ms_name;

	string ms_desc;
	int ms_nameId;
	int ms_descId;

    int ms_actId;

	string[] SkillTypes = { 
		"暂未选择",
		"主角普攻",
		"主角技能_1",
		"主角技能_2",
		"主角技能_3",
		"主角技能_4",
		"主角技能_5",
		"雇佣兵大招" ,
		"雇佣兵普攻" ,
		"雇佣兵技能_1",
		"怪物技能"
	};
	int ms_skillType = 1;

    string[] ElementType = {"物理","冰","火"};
    int ms_elementType;

    float ms_elmDamageRate;

    int ms_belongType;
    string[] BelongType = {"玩家","雇佣兵","怪物"};

    int ms_slotIdx_Int;

    int ms_lockType;
    string[] LockType = { "无锁定", "锁定", "锁定怪物(预留暂无用)" };

    float maxDistance, minDistance,cooldown;

	// 总时长
	public float duration = 0;

	// 前摇结束点
	public float beforeRoll = 0;

	// 后摇开始点
	public float afterRoll = 0;

	string ms_sEvtStr;

	// 是否可以移动
	bool ms_isCanMove = false;

	int ms_iNextSkillId = 0;

	// 暂停按钮控制
	bool isPauseing = false;

	// 是否运行
	bool isRunnging = false;

    #endregion

    public void DoInit(EDW_Skill org)
    {
        this.m_wSkill = org;
		this.m_wSkill.AddCall4Update(OnUpdate);

		m_ePSEvents.DoInit(org);

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

                    _DrawSkill();

                }

                EG_GUIHelper.FEG_EndScroll();

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
			optSkill.DoInit(this.pathOpenSkill, 0);
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

    void _DrawSkill()
    {
        EG_GUIHelper.FEG_BeginH();
        ms_ID = EditorGUILayout.IntField("技能ID:",ms_ID);
        if (GUILayout.Button("SeachSkill"))
        {
            if (optSkill.isInitSuccessed)
            {
                ms_curSkill = optSkill.GetEnSkill(ms_ID);
                OnInitEntity2Attrs();
            }
            else{
                EditorUtility.DisplayDialog("Tips","没有选则技能表，不能进行查询搜索!","Okey");
            }
        }
        EG_GUIHelper.FEG_EndH();
        EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
        ms_name = EditorGUILayout.TextField("技能名称:", ms_name);
        EG_GUIHelper.FEG_EndH();
        EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_nameId = EditorGUILayout.IntField("技能名称ID:", ms_nameId);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_desc = EditorGUILayout.TextField("技能描述:", ms_desc);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_descId = EditorGUILayout.IntField("技能描述ID:", ms_descId);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
        ms_actId = EditorGUILayout.IntField("SkillIndex:", ms_actId);

        GUI.color = Color.yellow;
        EG_GUIHelper.FG_Label("触发该节能的条件,比如该技能是Animator\nController里面的Skill1,此地就填写数字:1");
        GUI.color = Color.white;
        EG_GUIHelper.FEG_EndH();
        EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
        ms_skillType = EditorGUILayout.Popup("释放类型:", ms_skillType, SkillTypes);
        EG_GUIHelper.FEG_EndH();
        EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
        ms_elementType = EditorGUILayout.Popup("元素类型:", ms_elementType, ElementType);
        EG_GUIHelper.FEG_EndH();
        EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
        ms_elmDamageRate = EditorGUILayout.FloatField("元素伤害系数:", ms_elmDamageRate);
        EG_GUIHelper.FEG_EndH();
        EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
        ms_belongType = EditorGUILayout.Popup("所属对象类型:", ms_belongType, BelongType);
        EG_GUIHelper.FEG_EndH();
        EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
        ms_slotIdx_Int = EditorGUILayout.IntField("技能槽位:", ms_slotIdx_Int);
        EG_GUIHelper.FEG_EndH();
        EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
        ms_lockType = EditorGUILayout.Popup("锁定类型:", ms_lockType, LockType);
        EG_GUIHelper.FEG_EndH();
        EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
        maxDistance = EditorGUILayout.FloatField("最大释放距离(单位米):", maxDistance);
        EG_GUIHelper.FEG_EndH();
        EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
        minDistance = EditorGUILayout.FloatField("最小释放距离(单位米):", minDistance);
        EG_GUIHelper.FEG_EndH();
        EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
        cooldown = EditorGUILayout.FloatField("冷却时间:", cooldown);
        EG_GUIHelper.FEG_EndH();
        EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
        duration = EditorGUILayout.FloatField("技能时长(单位秒):", duration);
        EG_GUIHelper.FEG_EndH();
        EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
		beforeRoll = EditorGUILayout.Slider("前摇结束点(单位秒):", beforeRoll,0,duration);
        EG_GUIHelper.FEG_EndH();
        EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
		afterRoll = EditorGUILayout.Slider("后摇开始点(单位秒):", afterRoll,beforeRoll,duration);
        EG_GUIHelper.FEG_EndH();
        EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_isCanMove = EditorGUILayout.Toggle("是否可移动? ",ms_isCanMove);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_iNextSkillId = EditorGUILayout.IntField ("下个技能ID:",ms_iNextSkillId);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

        // 绘制技能事件
        EG_GUIHelper.FG_BeginVAsArea();
        {
            EG_GUIHelper.FEG_BeginH();
            EditorGUILayout.LabelField("技能事件:",ms_sEvtStr, EditorStyles.textArea);
            EG_GUIHelper.FEG_EndH();
			EG_GUIHelper.FG_Space(5);

			_DrawEvents ();
        }
        EG_GUIHelper.FG_EndV();
    }

	void _DrawEvents(){
		m_ePSEvents.DrawEvents ();
		this.ms_sEvtStr = m_ePSEvents.ToJsonString ();
	}

    void _DrawOptExcelBtns()
    {
        EG_GUIHelper.FEG_BeginH();
        {
            if (GUILayout.Button("Save Excel"))
            {
                if (!optSkill.isInitSuccessed)
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

				this.ms_sEvtStr = m_ePSEvents.ToJsonString ();

                OnInitAttrs2Entity();

                optSkill.Save(savePath);
            }
        }
        EG_GUIHelper.FEG_EndH();
    }

    void OnInitEntity2Attrs()
    {
        if(ms_curSkill != null)
        {
            this.ms_ID = this.ms_curSkill.ID;
            this.ms_name = this.ms_curSkill.Name;
            this.ms_actId = this.ms_curSkill.ActId;
			this.ms_skillType = this.ms_curSkill.SkillType;
            this.ms_elementType = this.ms_curSkill.ElementType_Int;
            this.ms_elmDamageRate = this.ms_curSkill.DmgAdditional;
            this.ms_belongType = this.ms_curSkill.SlotObjTp_Int;
            this.ms_slotIdx_Int = this.ms_curSkill.SlotIdx_Int;
            this.ms_lockType = this.ms_curSkill.LockTp_Int;
            this.maxDistance = this.ms_curSkill.CastDistFarthest;
            this.minDistance = this.ms_curSkill.CastDistNearest;
            this.cooldown = this.ms_curSkill.CD;
            this.duration = this.ms_curSkill.Duration;
            this.beforeRoll = this.ms_curSkill.PreCastTiming;
            this.afterRoll = this.ms_curSkill.PostCastTiming;

            // 事件处理有点麻烦，单独出来写
            this.ms_sEvtStr = this.ms_curSkill.CastEvent_Str;

			m_ePSEvents.DoReInitEventJson (this.ms_sEvtStr);

			this.ms_nameId = this.ms_curSkill.NameID;
			this.ms_desc = this.ms_curSkill.Desc;
			this.ms_descId = this.ms_curSkill.DescID;

			this.ms_isCanMove = this.ms_curSkill.CanMove == 1;
			this.ms_iNextSkillId = this.ms_curSkill.NextSkillID;
        }
    }

    void OnInitAttrs2Entity()
    {
        ms_curSkill = optSkill.GetOrNew(ms_ID);

        this.ms_curSkill.ID = this.ms_ID;
        this.ms_curSkill.Name = this.ms_name;
        this.ms_curSkill.ActId = this.ms_actId;
		this.ms_curSkill.SkillType = this.ms_skillType;
        this.ms_curSkill.ElementType_Int = this.ms_elementType;
        this.ms_curSkill.DmgAdditional = this.ms_elmDamageRate;
        this.ms_curSkill.SlotObjTp_Int = this.ms_belongType;
        this.ms_curSkill.SlotIdx_Int = this.ms_slotIdx_Int;
        this.ms_curSkill.LockTp_Int = this.ms_lockType;
        this.ms_curSkill.CastDistFarthest = this.maxDistance;
        this.ms_curSkill.CastDistNearest = this.minDistance;
        this.ms_curSkill.CD = this.cooldown;
        this.ms_curSkill.Duration = this.duration;
        this.ms_curSkill.PreCastTiming = this.beforeRoll;
        this.ms_curSkill.PostCastTiming = this.afterRoll;

        this.ms_curSkill.CastEvent_Str = this.ms_sEvtStr;

		this.ms_curSkill.NameID = this.ms_nameId;
		this.ms_curSkill.Desc = this.ms_desc;
		this.ms_curSkill.DescID = this.ms_descId;
		this.ms_curSkill.CanMove = this.ms_isCanMove ? 1 : 0;
		this.ms_curSkill.NextSkillID = this.ms_iNextSkillId;
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
		if (!isRunnging || isPauseing)
			return;

		if (this.m_curAni == null)
			return;

		this.m_curTime.DoUpdateTime();
		this.m_curAni.DoUpdateAnimator(m_curTime.DeltaTime,1);
		m_ePSEvents.OnUpdate (m_curTime.DeltaTime, 1);

		// 设置位移
	}

	void DoPlay() {
		m_curTime.DoStart ();
		this.m_curAni.DoReady (ms_actId);
		this.m_curAni.DoStart();

		isRunnging = true;
		isPauseing = false;

		m_ePSEvents.DoStart();
	}

	void DoPause() {
		isPauseing = true;

		m_curTime.DoPause();
		m_ePSEvents.DoPause();
	}

	void DoResume() {
		isPauseing = false;
		m_curTime.DoResume();
		m_ePSEvents.DoResume();
	}

	void DoStop() {
		isRunnging = false;
		m_ePSEvents.DoEnd();
	}
}
