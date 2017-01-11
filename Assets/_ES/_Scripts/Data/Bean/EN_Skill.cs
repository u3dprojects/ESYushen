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

    public int c00_ID;
    public string c01_Name;
    public int c02_ActId;
    public int c03_CastType_Int;
    public int c04_ElementType_Int;
    public float c05_DmgAdditional;
    public int c06_SlotObjTp_Int;
    public int c07_SlotIdx_Int;
    public int c08_LockTp_Int;
    public float c09_CastDistFarthest;
    public float c10_CastDistNearest;
    public float c11_CD;
    public float c12_Duration;
    public string c13_CastEvent_Str;
    public float c14_PreCastTiming;
    public float c15_PostCastTiming;

    object[] Columns
    {
        get { 
            object[] ret = {
                this.c00_ID,
                this.c01_Name,
                this.c02_ActId,
                this.c03_CastType_Int,
                this.c04_ElementType_Int,
                this.c05_DmgAdditional,
                this.c06_SlotObjTp_Int,
                this.c07_SlotIdx_Int,
                this.c08_LockTp_Int,
                this.c09_CastDistFarthest,
                this.c10_CastDistNearest,
                this.c11_CD,
                this.c12_Duration,
                this.c13_CastEvent_Str,
                this.c14_PreCastTiming,
                this.c15_PostCastTiming
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

        one.c00_ID = sheet.GetInt(rowIndex, 0);
        one.c01_Name = sheet.GetString(rowIndex, 1);
        one.c02_ActId = sheet.GetInt(rowIndex, 2);
        one.c03_CastType_Int = sheet.GetInt(rowIndex, 3);
        one.c04_ElementType_Int = sheet.GetInt(rowIndex, 4);
        one.c05_DmgAdditional = sheet.GetFloat(rowIndex, 5);
        one.c06_SlotObjTp_Int = sheet.GetInt(rowIndex, 6);
        one.c07_SlotIdx_Int = sheet.GetInt(rowIndex, 7);
        one.c08_LockTp_Int = sheet.GetInt(rowIndex, 8);
        one.c09_CastDistFarthest = sheet.GetFloat(rowIndex, 9);
        one.c10_CastDistNearest = sheet.GetFloat(rowIndex, 10);
        one.c11_CD = sheet.GetFloat(rowIndex, 11);
        one.c12_Duration = sheet.GetFloat(rowIndex, 12);
        one.c13_CastEvent_Str = sheet.GetString(rowIndex, 13);
        one.c14_PreCastTiming = sheet.GetFloat(rowIndex, 14);
        one.c15_PostCastTiming = sheet.GetFloat(rowIndex, 15);

        return one;
    }
}