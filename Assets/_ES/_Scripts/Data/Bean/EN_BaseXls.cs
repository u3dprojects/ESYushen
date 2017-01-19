﻿using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : excel 实体对象 基础类
/// 作者 : Canyon
/// 日期 : 2017-01-19 11:30
/// 功能 : 
/// </summary>
public class EN_BaseXls : System.Object {
	public int rowIndex;
	public NH_Sheet sheet;

	public int ID;

	public EN_BaseXls(){}

	protected virtual object[] Columns
	{
		get { 
			return null;
		}
	}

	public void ToNSCell()
	{
		object[] columns = Columns;
		if (columns == null || columns.Length <= 0) {
			return;
		}

		int lens = columns.Length;
		for(int i = 0; i < lens; i++)
		{
			this.sheet.SaveValueToCache(this.rowIndex, i, columns[i]);
		}
	}

	public virtual void DoInit(int rowIndex, NH_Sheet sheet){
		this.rowIndex = rowIndex;
		this.sheet = sheet;
	}

	static public T NewTEntity<T>(int rowIndex, NH_Sheet sheet) where T : EN_BaseXls,new()
	{
		T one = new T();
		one.DoInit (rowIndex, sheet);
		return one;
	}
}
