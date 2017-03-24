using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 类名 : NPC 实体操作
/// 作者 : Canyon
/// 日期 : 2017-03-24 10:30
/// 功能 : 
/// </summary>
public class EN_OptNpc {

	static EN_OptNpc _instance;
	static public EN_OptNpc Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new EN_OptNpc();
			}
			return _instance;
		}
	}

	private EN_OptNpc() { }

	EN_OptBaseXls<EN_Npc> _m_eOptXls = new EN_OptBaseXls<EN_Npc>();
	public EN_OptBaseXls<EN_Npc> m_eOptXls
	{
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

	public EN_Npc GetEntity(int ID)
	{
		return _m_eOptXls.GetEntity (ID);
	}

	public EN_Npc GetOrNew(int ID)
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
