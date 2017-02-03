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

	EN_OptBaseXls<EN_Buff> m_eOptXls = new EN_OptBaseXls<EN_Buff>();

	public bool isInitSuccessed = false;

	public void DoInit(string path, int sheetIndex)
	{
		m_eOptXls.DoInit (path, sheetIndex);
		isInitSuccessed = m_eOptXls.isInitSuccessed;
	}

	public EN_Buff GetEntity(int ID)
	{
		return m_eOptXls.GetEntity (ID);
	}

	public EN_Buff GetOrNew(int ID)
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
