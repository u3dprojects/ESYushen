using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 类名 : Monster 怪物 实体操作
/// 作者 : Canyon
/// 日期 : 2017-02-07 14:30
/// 功能 : 
/// </summary>
public class EN_OptMonster {

	static EN_OptMonster _instance;
	static public EN_OptMonster Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new EN_OptMonster();
			}
			return _instance;
		}
	}

	private EN_OptMonster() { }

	EN_OptBaseXls<EN_Monster> m_eOptXls = new EN_OptBaseXls<EN_Monster>();

	public bool isInitSuccessed = false;

	public void DoInit(string path, int sheetIndex)
	{
		m_eOptXls.DoInit (path, sheetIndex);
		isInitSuccessed = m_eOptXls.isInitSuccessed;
	}

	public EN_Monster GetEntity(int ID)
	{
		return m_eOptXls.GetEntity (ID);
	}

	public EN_Monster GetOrNew(int ID)
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
