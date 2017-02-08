using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 类名 : Bullet子弹实体操作
/// 作者 : Canyon
/// 日期 : 2017-01-22 10:30
/// 功能 : 
/// </summary>
public class EN_OptBullet {

	static EN_OptBullet _instance;
	static public EN_OptBullet Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new EN_OptBullet();
			}
			return _instance;
		}
	}

	private EN_OptBullet() { }

	EN_OptBaseXls<EN_Bullet> _m_eOptXls = new EN_OptBaseXls<EN_Bullet>();
	public EN_OptBaseXls<EN_Bullet> m_eOptXls
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

	public EN_Bullet GetEntity(int ID)
	{
		return _m_eOptXls.GetEntity (ID);
	}

	public EN_Bullet GetOrNew(int ID)
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
