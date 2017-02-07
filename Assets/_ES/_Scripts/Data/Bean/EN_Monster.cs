using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : excel Monster 实体对象
/// 作者 : Canyon
/// 日期 : 2017-02-07 14:01
/// 功能 : 
/// </summary>
public class EN_Monster : EN_BaseXls{
	// public int ID;
	// 语言表里面的ID
	public int NameID;

	// 资源名
	public string ModeRes;

	// 头像资源
	public string HeadRes;
	
	// 等级
	public int Level;
	
	// 生命
	public int LifeHP;

	// 攻击力
	public int Attack;

	// 防御力
	public int Defense;

	// 移动速度
	public float MoveSpeed;

	// 旋转速度
	public float RotationSpeed;

	// 体宽
	public float Width;

	// 身高
	public float Height;

	// AI
	public string AI;

	// 选中时候的选中框
	public string SelectBox;

	// 分类
	public int Types;

	public EN_Monster():base(){
	}

	protected override object[] Columns
	{
		get { 
			object[] ret = {
				this.ID,
				this.NameID,
				this.ModeRes,
				this.HeadRes,
				this.Level,
				this.LifeHP,
				this.Attack,
				this.Defense,
				this.MoveSpeed,
				this.RotationSpeed,
				this.Width,
				this.Height,
				this.AI,
				this.SelectBox,
				this.Types
			};
			return ret;
		}
	}

	public override void DoInit (int rowIndex, NH_Sheet sheet)
	{
		base.DoInit (rowIndex, sheet);

		int colIndex = 0;
		this.ID = sheet.GetInt(rowIndex, colIndex++);
		this.NameID = sheet.GetInt(rowIndex, colIndex++);
		this.ModeRes = sheet.GetString(rowIndex, colIndex++);
		this.HeadRes = sheet.GetString(rowIndex, colIndex++);

		this.Level = sheet.GetInt(rowIndex, colIndex++);
		this.LifeHP = sheet.GetInt(rowIndex, colIndex++);
		this.Attack = sheet.GetInt(rowIndex, colIndex++);
		this.Defense = sheet.GetInt(rowIndex, colIndex++);
		this.MoveSpeed = sheet.GetFloat(rowIndex, colIndex++);
		this.RotationSpeed = sheet.GetFloat(rowIndex, colIndex++);
		this.Width = sheet.GetFloat(rowIndex, colIndex++);
		this.Height = sheet.GetFloat(rowIndex, colIndex++);

		this.AI = sheet.GetString(rowIndex, colIndex++);
		this.SelectBox = sheet.GetString(rowIndex, colIndex++);
		this.Types = sheet.GetInt(rowIndex, colIndex++);
	}

	public override void DoClone (EN_BaseXls org)
	{
		if (!(org is EN_Monster)) {
			return;
		}

		EN_Monster tmp = (EN_Monster)org;

		base.DoClone (tmp);

		this.ID = tmp.ID;
		this.NameID = tmp.NameID;
		this.ModeRes = tmp.ModeRes;
		this.HeadRes = tmp.HeadRes;
		this.Level = tmp.Level;
		this.LifeHP = tmp.LifeHP;
		this.Attack = tmp.Attack;
		this.Defense = tmp.Defense;
		this.MoveSpeed = tmp.MoveSpeed;
		this.RotationSpeed = tmp.RotationSpeed;
		this.Width = tmp.Width;
		this.Height = tmp.Height;
		this.AI = tmp.AI;
		this.SelectBox = tmp.SelectBox;
		this.Types = tmp.Types;
	}
}
