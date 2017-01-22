using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 类名 : 技能实体操作
/// 作者 : Canyon
/// 日期 : 2016-12-29 17:10:00
/// 功能 : 
/// </summary>
public class EN_OptSkill
{
    static EN_OptSkill _instance;
    static public EN_OptSkill Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EN_OptSkill();
            }
            return _instance;
        }
    }

    private EN_OptSkill() { }

	EN_OptBaseXls<EN_Skill> m_eOptXls = new EN_OptBaseXls<EN_Skill>();

    public bool isInitSuccessed = false;

    public void DoInit(string path, int sheetIndex)
    {
		m_eOptXls.DoInit (path, sheetIndex);
		isInitSuccessed = m_eOptXls.isInitSuccessed;
    }

    public EN_Skill GetEnSkill(int ID)
    {
		return m_eOptXls.GetEntity (ID);
    }

    public EN_Skill GetOrNew(int ID)
    {
		return m_eOptXls.GetOrNew(ID);
    }

    public void Save(string savePath)
    {
		m_eOptXls.Save (savePath);
    }

	public void DoClear(){
		m_eOptXls.DoClear ();
		isInitSuccessed = false;
	}
}