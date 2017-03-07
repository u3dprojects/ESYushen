using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 类名 : 技能实体操作
/// 作者 : Canyon
/// 日期 : 2016-12-29 17:10
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

	EN_OptBaseXls<EN_Skill> _m_eOptXls = new EN_OptBaseXls<EN_Skill>();
	public EN_OptBaseXls<EN_Skill> m_eOptXls{
		get{
			return _m_eOptXls;
		}
	}

	public bool isInitSuccessed{
		get{
			return _m_eOptXls.isInitSuccessed;
		}
	}

    public void DoInit(string path, int sheetIndex)
    {
		_m_eOptXls.DoInit (path, sheetIndex);
    }

    public EN_Skill GetEnSkill(int ID)
    {
		return _m_eOptXls.GetEntity (ID);
    }

    public EN_Skill GetOrNew(int ID)
    {
		return _m_eOptXls.GetOrNew(ID);
    }

    public void Save(string savePath)
    {
		_m_eOptXls.Save (savePath);
    }

	public void DoClear(){
		_m_eOptXls.DoClear ();
	}
}