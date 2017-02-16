using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 类名 : excel技能实体对象
/// 作者 : Canyon
/// 日期 : 2016-12-29 17:10
/// 功能 : 
/// </summary>
public class EN_Skill : EN_BaseXls{
    // public int ID;
    public string Name;

	public string Desc;
	// 语言表里面的ID
	public int NameID;
	// 语言表里面的ID
	public int DescID;

    public int ActId;
    public int SkillType;
    public int ElementType_Int;
    public float DmgAdditional;
    public int SlotObjTp_Int;
    public int SlotIdx_Int;
    public int LockTp_Int;
    public float CastDistFarthest;
    public float CastDistNearest;
    public float CD;
    public float Duration;
    public string CastEvent_Str;
    public float PreCastTiming;
    public float PostCastTiming;

	public int CanMove;
	public int NextSkillID;

	// 速度比例, (仅在当前技能可以移动时有效), 为当前移动速度的百分比
	public float MoveSpeed;

	// 伤害结算范围类型, 0 以自己为中心, 1 以目标为中心, 2 以指定位置为中心
	public int DmgFieldType;

	public EN_Skill():base(){
	}

	protected override object[] Columns
    {
        get { 
            object[] ret = {
                this.ID,
                this.Name,

				this.Desc,
				this.NameID,
				this.DescID,

                this.ActId,
                this.SkillType,
                this.ElementType_Int,
                this.DmgAdditional,
                this.SlotObjTp_Int,
                this.SlotIdx_Int,
                this.LockTp_Int,
                this.CastDistFarthest,
                this.CastDistNearest,
                this.CD,
                this.Duration,
                this.CastEvent_Str,
                this.PreCastTiming,
                this.PostCastTiming,

				this.CanMove,
				this.NextSkillID,

				this.MoveSpeed,
				this.DmgFieldType
            };
            return ret;
        }
    }

	public override void DoInit (int rowIndex, NH_Sheet sheet)
	{
		base.DoInit (rowIndex, sheet);

		int colIndex = 0;
		this.ID = sheet.GetInt(rowIndex, colIndex++);
		this.Name = sheet.GetString(rowIndex, colIndex++);

		this.Desc = sheet.GetString(rowIndex, colIndex++);
		this.NameID = sheet.GetInt(rowIndex, colIndex++);
		this.DescID = sheet.GetInt(rowIndex, colIndex++);

		this.ActId = sheet.GetInt(rowIndex, colIndex++);
		this.SkillType = sheet.GetInt(rowIndex, colIndex++);
		this.ElementType_Int = sheet.GetInt(rowIndex, colIndex++);
		this.DmgAdditional = sheet.GetFloat(rowIndex, colIndex++);
		this.SlotObjTp_Int = sheet.GetInt(rowIndex, colIndex++);
		this.SlotIdx_Int = sheet.GetInt(rowIndex, colIndex++);
		this.LockTp_Int = sheet.GetInt(rowIndex, colIndex++);
		this.CastDistFarthest = sheet.GetFloat(rowIndex, colIndex++);
		this.CastDistNearest = sheet.GetFloat(rowIndex, colIndex++);
		this.CD = sheet.GetFloat(rowIndex, colIndex++);
		this.Duration = sheet.GetFloat(rowIndex, colIndex++);
		this.CastEvent_Str = sheet.GetString(rowIndex, colIndex++);
		this.PreCastTiming = sheet.GetFloat(rowIndex, colIndex++);
		this.PostCastTiming = sheet.GetFloat(rowIndex, colIndex++);

		this.CanMove = sheet.GetInt(rowIndex, colIndex++);
		this.NextSkillID = sheet.GetInt(rowIndex, colIndex++);

		this.MoveSpeed = sheet.GetFloat(rowIndex, colIndex++);
		this.DmgFieldType = sheet.GetInt(rowIndex, colIndex++);
	}

	public override void DoClone (EN_BaseXls org)
	{
		if (!(org is EN_Skill)) {
			return;
		}

		EN_Skill tmp = (EN_Skill)org;

		base.DoClone (tmp);
		this.ID = tmp.ID;
		this.Name = tmp.Name;

		this.Desc = tmp.Desc;
		this.NameID = tmp.NameID;
		this.DescID = tmp.DescID;

		this.ActId = tmp.ActId;
		this.SkillType = tmp.SkillType;
		this.ElementType_Int = tmp.ElementType_Int;
		this.DmgAdditional = tmp.DmgAdditional;
		this.SlotObjTp_Int = tmp.SlotObjTp_Int;
		this.SlotIdx_Int = tmp.SlotIdx_Int;
		this.LockTp_Int = tmp.LockTp_Int;
		this.CastDistFarthest = tmp.CastDistFarthest;
		this.CastDistNearest = tmp.CastDistNearest;
		this.CD = tmp.CD;
		this.Duration = tmp.Duration;
		this.CastEvent_Str = tmp.CastEvent_Str;
		this.PreCastTiming = tmp.PreCastTiming;
		this.PostCastTiming = tmp.PostCastTiming;

		this.CanMove = tmp.CanMove;
		this.NextSkillID = tmp.NextSkillID;

		this.MoveSpeed = tmp.MoveSpeed;
		this.DmgFieldType = tmp.DmgFieldType;
	}
}