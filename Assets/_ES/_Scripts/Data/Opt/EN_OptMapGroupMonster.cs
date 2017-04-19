using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 类名 : Map 地图 实体操作
/// 作者 : Canyon
/// 日期 : 2017-02-03 10:30
/// 功能 : 
/// </summary>
public class EN_OptMapGroupMonster {

	static EN_OptMapGroupMonster _instance;
	static public EN_OptMapGroupMonster Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new EN_OptMapGroupMonster();
			}
			return _instance;
		}
	}

	private EN_OptMapGroupMonster() { }

	EN_OptBaseXls<EN_MapGroupMonster> _m_eOptXls = new EN_OptBaseXls<EN_MapGroupMonster>();
	public EN_OptBaseXls<EN_MapGroupMonster> m_eOptXls{
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

	public EN_MapGroupMonster GetEntity(int ID)
	{
		return _m_eOptXls.GetEntity (ID);
	}

	public EN_MapGroupMonster GetOrNew(int ID)
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

	public void SaveReplace(){
		_m_eOptXls.SaveReplace ();
	}

	// 取得列表
	public List<EN_MapGroupMonster> GetListByMapId(int mapId){
		if (_m_eOptXls.list == null || _m_eOptXls.list.Count <= 0)
			return null;

		List<EN_MapGroupMonster> ret = new List<EN_MapGroupMonster> ();
		foreach (var item in _m_eOptXls.list) {
			if (item.MapId == mapId) {
				ret.Add (item);
			}
		}
		return ret;
	}
}
