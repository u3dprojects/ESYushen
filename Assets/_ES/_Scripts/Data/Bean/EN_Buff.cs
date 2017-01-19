using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : excel Buff 实体对象
/// 作者 : Canyon
/// 日期 : 2017-01-19 11:10
/// 功能 : 
/// </summary>
public class EN_Buff : EN_BaseXls{
	// public int ID;
	public string Name;

	public string Desc;
	// 语言表里面的ID
	public int NameID;
	// 语言表里面的ID
	public int DescID;

	public string Icon;
	public string EffectResName;
	public int JoinId;
	public string MateChange;
	public int Tag;
	public int GID;
	public int IsResetWhenGet;

	public string strEvtInterval;
	public string strEvtOnce;
	public float Interval;
	public string strEvtDuration;

	public EN_Buff():base(){
	}

	protected override object[] Columns
	{
		get { 
			object[] ret = {
				this.ID,
				this.Name,

				this.Desc,
				this.NameID,
				this.DescID,

				this.Icon,
				this.EffectResName,
				this.JoinId,
				this.MateChange,
				this.Tag,
				this.GID,
				this.IsResetWhenGet,
				this.strEvtInterval,
				this.strEvtOnce,
				this.Interval,
				this.strEvtDuration
			};
			return ret;
		}
	}

	public override void DoInit (int rowIndex, NH_Sheet sheet)
	{
		base.DoInit (rowIndex, sheet);

		int colIndex = 0;
		this.ID = sheet.GetInt(rowIndex, colIndex++);
		this.Name = sheet.GetString(rowIndex, colIndex++);

		this.Desc = sheet.GetString(rowIndex, colIndex++);
		this.NameID = sheet.GetInt(rowIndex, colIndex++);
		this.DescID = sheet.GetInt(rowIndex, colIndex++);

		this.Icon = sheet.GetString(rowIndex, colIndex++);
		this.EffectResName = sheet.GetString(rowIndex, colIndex++);
		this.JoinId = sheet.GetInt(rowIndex, colIndex++);
		this.MateChange = sheet.GetString(rowIndex, colIndex++);
		this.Tag = sheet.GetInt(rowIndex, colIndex++);
		this.GID = sheet.GetInt(rowIndex, colIndex++);
		this.IsResetWhenGet = sheet.GetInt(rowIndex, colIndex++);
		this.strEvtInterval = sheet.GetString(rowIndex, colIndex++);
		this.strEvtOnce = sheet.GetString(rowIndex, colIndex++);
		this.Interval = sheet.GetFloat(rowIndex, colIndex++);
		this.strEvtDuration = sheet.GetString(rowIndex, colIndex++);
	}

	static public EN_Buff NewBuff(int rowIndex, NH_Sheet sheet)
	{
		EN_Buff one = new EN_Buff();
		one.DoInit (rowIndex, sheet);
		return one;
	}
}
