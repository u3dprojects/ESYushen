using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 类名 : Buff实体操作
/// 作者 : Canyon
/// 日期 : 2016-12-29 17:10
/// 功能 : 
/// </summary>
public class EN_OptBuff {

	static EN_OptBuff _instance;
	static public EN_OptBuff Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new EN_OptBuff();
			}
			return _instance;
		}
	}

	private EN_OptBuff() { }

	EN_OptBaseXls<EN_Buff> _m_eOptXls = new EN_OptBaseXls<EN_Buff>();
	public EN_OptBaseXls<EN_Buff> m_eOptXls{
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

	public EN_Buff GetEntity(int ID)
	{
		return _m_eOptXls.GetEntity (ID);
	}

	public EN_Buff GetOrNew(int ID)
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
