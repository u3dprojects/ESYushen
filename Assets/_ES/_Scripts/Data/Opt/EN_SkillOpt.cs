using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 类名 : 技能实体操作
/// 作者 : Canyon
/// 日期 : 2016-12-29 17:10:00
/// 功能 : 
/// </summary>
public class EN_SkillOpt
{
    static EN_SkillOpt _instance;
    static public EN_SkillOpt Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EN_SkillOpt();
            }
            return _instance;
        }
    }

    private EN_SkillOpt() { }

    int NumberOfRow = 0;
    NH_Sheet m_sheet = null;
    List<EN_Skill> list = null;

    public bool isInitSuccessed = false;

    public void DoInit(string path, int sheetIndex)
    {
        DoClear();
        try
        {
            this.m_sheet = new NH_Sheet(path, sheetIndex);
            this.list = new List<EN_Skill>();

            EN_Skill tmp = null;
            object obj = null;

            for (int i = 4; i < this.m_sheet.maxRow; i++)
            {
                obj = this.m_sheet.GetNCell(i, 0).val;
                if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                {
                    break;
                }

                tmp = EN_Skill.NewSkill(i, this.m_sheet);
                this.list.Add(tmp);
                this.NumberOfRow = i + 1;
            }

            isInitSuccessed = true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("你选取的Excel表正在编辑中，请关闭Excel表。" + ex);
        }

    }

    public EN_Skill GetEnSkill(int ID)
    {
        if (this.list == null || this.list.Count <= 0)
            return null;

        int lens = this.list.Count;
        for (int i = 0; i < lens; i++)
        {
            if (this.list[i].ID == ID)
                return this.list[i];
        }
        return null;
    }

    public EN_Skill GetOrNew(int ID)
    {
        EN_Skill ret = GetEnSkill(ID);
        if (ret == null)
        {
            ret = new EN_Skill();
            ret.sheet = m_sheet;
            ret.rowIndex = NumberOfRow;

            NumberOfRow++;

            this.list.Add(ret);
        }
        return ret;
    }

    void ToNSList()
    {
        if (this.list == null || this.list.Count <= 0)
            return;

        int lens = this.list.Count;
        for (int i = 0; i < lens; i++)
        {
            (list[i]).ToNSCell();
        }
    }

    public void Save(string savePath)
    {
        if (!isInitSuccessed)
            return;

        ToNSList();
        NPOIHssfEx.ToFile(m_sheet.ToWorkbook(), savePath);
    }

    public void DoClear()
    {
        this.m_sheet = null;
        if (this.list != null)
        {
            this.list.Clear();
            this.list = null;
        }

        NumberOfRow = 0;
        isInitSuccessed = false;
    }
}