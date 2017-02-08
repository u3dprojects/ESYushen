using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 类名 : Map 地图 实体操作
/// 作者 : Canyon
/// 日期 : 2017-02-03 10:30
/// 功能 : 
/// </summary>
public class EN_OptMap {

	static EN_OptMap _instance;
	static public EN_OptMap Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new EN_OptMap();
			}
			return _instance;
		}
	}

	private EN_OptMap() { }

	EN_OptBaseXls<EN_Map> _m_eOptXls = new EN_OptBaseXls<EN_Map>();
	public EN_OptBaseXls<EN_Map> m_eOptXls{
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

	public EN_Map GetEntity(int ID)
	{
		return _m_eOptXls.GetEntity (ID);
	}

	public EN_Map GetOrNew(int ID)
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
