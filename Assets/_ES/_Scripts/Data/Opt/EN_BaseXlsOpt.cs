using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 类名 : excel表 <--> Class实例 转换类
/// 作者 : Canyon
/// 日期 : 2017-01-19 14:10
/// 功能 : 
/// </summary>
public class EN_BaseXlsOpt<T> where T : EN_BaseXls,new()
{
	int NumberOfRow = 0;
	NH_Sheet m_sheet = null;
	List<T> list = null;

	public bool isInitSuccessed = false;

	public void DoInit(string path, int sheetIndex)
	{
		DoClear();
		try
		{
			this.m_sheet = new NH_Sheet(path, sheetIndex);
			this.list = new List<T>();

			T tmp = null;
			object obj = null;

			for (int i = 4; i < this.m_sheet.maxRow; i++)
			{
				obj = this.m_sheet.GetNCell(i, 0).val;
				if (obj == null || string.IsNullOrEmpty(obj.ToString()))
				{
					break;
				}

				tmp = EN_BaseXls.NewTEntity<T>(i, this.m_sheet);
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

	public T GetEntity(int ID)
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

	public T GetOrNew(int ID)
	{
		T ret = GetEntity(ID);
		if (ret == null)
		{
			ret = new T();
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
