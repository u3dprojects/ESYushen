using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 类名 : excel技能实体对象
/// 作者 : Canyon
/// 日期 : 2016-12-29 17:10:00
/// 功能 : 
/// </summary>
public class EN_Skill{
    public int rowIndex;
    public NH_Sheet sheet;

    public int ID;
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

    object[] Columns
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
				this.NextSkillID
            };
            return ret;
        }
    }

    public void ToNSCell()
    {
        object[] columns = Columns;
        int lens = columns.Length;
        for(int i = 0; i < lens; i++)
        {
            this.sheet.SaveValueToCache(this.rowIndex, i, columns[i]);
        }
    }
    
    static public EN_Skill NewSkill(int rowIndex, NH_Sheet sheet)
    {
        EN_Skill one = new EN_Skill();
        one.rowIndex = rowIndex;
        one.sheet = sheet;

		int colIndex = 0;
		one.ID = sheet.GetInt(rowIndex, colIndex++);
		one.Name = sheet.GetString(rowIndex, colIndex++);

		one.Desc = sheet.GetString(rowIndex, colIndex++);
		one.NameID = sheet.GetInt(rowIndex, colIndex++);
		one.DescID = sheet.GetInt(rowIndex, colIndex++);

		one.ActId = sheet.GetInt(rowIndex, colIndex++);
		one.SkillType = sheet.GetInt(rowIndex, colIndex++);
		one.ElementType_Int = sheet.GetInt(rowIndex, colIndex++);
		one.DmgAdditional = sheet.GetFloat(rowIndex, colIndex++);
		one.SlotObjTp_Int = sheet.GetInt(rowIndex, colIndex++);
		one.SlotIdx_Int = sheet.GetInt(rowIndex, colIndex++);
		one.LockTp_Int = sheet.GetInt(rowIndex, colIndex++);
		one.CastDistFarthest = sheet.GetFloat(rowIndex, colIndex++);
		one.CastDistNearest = sheet.GetFloat(rowIndex, colIndex++);
		one.CD = sheet.GetFloat(rowIndex, colIndex++);
		one.Duration = sheet.GetFloat(rowIndex, colIndex++);
		one.CastEvent_Str = sheet.GetString(rowIndex, colIndex++);
		one.PreCastTiming = sheet.GetFloat(rowIndex, colIndex++);
		one.PostCastTiming = sheet.GetFloat(rowIndex, colIndex++);

		one.CanMove = sheet.GetInt(rowIndex, colIndex++);
		one.NextSkillID = sheet.GetInt(rowIndex, colIndex++);
        return one;
    }
}