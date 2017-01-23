using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

/// <summary>
/// 类名 : skill 在 windows的视图
/// 作者 : Canyon
/// 日期 : 2017-01-19 17:10
/// 功能 : 
/// </summary>
public class EG_Skill {
	#region  == Member Attribute ===

	EN_OptSkill m_opt
	{
		get { return EN_OptSkill.Instance; }
	}

	// 事件
	public PS_Events m_ePSEvents = new PS_Events(true);

	// 技能属性
	public EN_Skill ms_enity = new EN_Skill ();

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

	string[] ElementType = {"物理","冰","火"};
	string[] BelongType = {"玩家","雇佣兵","怪物"};
	string[] LockType = { "无锁定", "锁定目标", "锁定位置" };
	// 是否可以移动
	bool ms_isCanMove = false;

	#endregion

	public void DoInit(string path){
		m_opt.DoInit (path, 0);
	}

	public bool isInited{
		get{
			return m_opt.isInitSuccessed;
		}
	}

	public void DrawShow()
	{
		EG_GUIHelper.FEG_HeadTitMid ("Skill Excel 表",Color.cyan);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.ID = EditorGUILayout.IntField("ID:",ms_enity.ID);
		if (GUILayout.Button("SeachSkill"))
		{
			if (isInited)
			{
				EN_Skill tmp = m_opt.GetEnSkill(ms_enity.ID);
				OnInitEntity2Attrs(tmp);
			}
			else{
				EditorUtility.DisplayDialog("Tips","没有选则技能表，不能进行查询搜索!","Okey");
			}
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.Name = EditorGUILayout.TextField("名称:", ms_enity.Name);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.NameID = EditorGUILayout.IntField("名称ID:", ms_enity.NameID);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.Desc = EditorGUILayout.TextField("描述:", ms_enity.Desc);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.DescID = EditorGUILayout.IntField("描述ID:", ms_enity.DescID);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.ActId = EditorGUILayout.IntField("SkillIndex:", ms_enity.ActId);

		GUI.color = Color.yellow;
		EG_GUIHelper.FG_Label("触发该节能的条件,比如该技能是Animator\nController里面的Skill1,此地就填写数字:1");
		GUI.color = Color.white;
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.SkillType = EditorGUILayout.Popup("释放类型:", ms_enity.SkillType, SkillTypes);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.ElementType_Int = EditorGUILayout.Popup("元素类型:", ms_enity.ElementType_Int, ElementType);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.DmgAdditional = EditorGUILayout.FloatField("元素伤害系数:", ms_enity.DmgAdditional);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.SlotObjTp_Int = EditorGUILayout.Popup("所属对象类型:", ms_enity.SlotObjTp_Int, BelongType);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.SlotIdx_Int = EditorGUILayout.IntField("技能槽位:", ms_enity.SlotIdx_Int);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.LockTp_Int = EditorGUILayout.Popup("锁定类型:", ms_enity.LockTp_Int, LockType);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.CastDistFarthest = EditorGUILayout.FloatField("最大释放距离(单位米):", ms_enity.CastDistFarthest);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.CastDistNearest = EditorGUILayout.FloatField("最小释放距离(单位米):", ms_enity.CastDistNearest);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.CD = EditorGUILayout.FloatField("冷却时间:", ms_enity.CD);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.Duration = EditorGUILayout.FloatField("技能时长(单位秒):", ms_enity.Duration);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.PreCastTiming = EditorGUILayout.Slider("前摇结束点(单位秒):", ms_enity.PreCastTiming,0, ms_enity.Duration);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.PostCastTiming = EditorGUILayout.Slider("后摇开始点(单位秒):", ms_enity.PostCastTiming,ms_enity.PreCastTiming, ms_enity.Duration);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_isCanMove = EditorGUILayout.Toggle("是否可移动? ",ms_isCanMove);
		ms_enity.CanMove = ms_isCanMove ? 1 : 0;
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH();
		ms_enity.NextSkillID = EditorGUILayout.IntField ("下个技能ID:",ms_enity.NextSkillID);
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		// 绘制技能事件
		EG_GUIHelper.FG_BeginVAsArea();
		{
			// EG_GUIHelper.FEG_BeginH();
			// EditorGUILayout.LabelField("技能事件:",ms_enity.CastEvent_Str, EditorStyles.textArea);
			// EG_GUIHelper.FEG_EndH();
			// EG_GUIHelper.FG_Space(5);
			EG_GUIHelper.FEG_HeadTitMid ("技能事件",Color.magenta);

			_DrawEvents ();
		}
		EG_GUIHelper.FG_EndV();
	}

	void _DrawEvents(){
		m_ePSEvents.DrawEvents (ms_enity.Duration,ms_enity.PreCastTiming,ms_enity.PostCastTiming);
		ms_enity.CastEvent_Str = m_ePSEvents.ToJsonString ();
	}

	void OnInitEntity2Attrs(EN_Skill entity)
	{
		if(entity != null)
		{
			ms_enity.DoClone (entity);
			m_ePSEvents.DoReInitEventJson (ms_enity.CastEvent_Str);
		}
	}

	void OnInitAttrs2Entity()
	{
		EN_Skill entity = m_opt.GetOrNew(ms_enity.ID);
		ms_enity.rowIndex = entity.rowIndex;
		entity.DoClone (ms_enity);
	}

	public void SaveExcel(string savePath){
		OnInitAttrs2Entity ();
		m_opt.Save (savePath);
	}

	public void DoClear(){
		m_opt.DoClear ();
	}
}
