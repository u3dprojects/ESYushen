using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : excel NPC 实体对象
/// 作者 : Canyon
/// 日期 : 2017-03-24 10:31
/// 功能 : 
/// </summary>
public class EN_Npc : EN_BaseXls{
	// public int ID;
	// 语言表里面的ID
	public int NameID;

	// 资源名
	public string ModeRes;

	// 头像资源
	public string HeadRes;

	// 移动速度
	public float MoveSpeed;

	// 旋转速度
	public float RotateSpeed;

	// 体宽
	public float Width;

	// 身高
	public float Height;

	// ui 视图中的位置
	public string UiPos;

	// ui 视图中的旋转
	public string UiRot;

	// ui 视图中的缩放
	public string UiScale;

	// 模型场景中的选中框
	public string SelectBox;

	public EN_Npc():base(){
	}

	protected override object[] Columns
	{
		get { 
			object[] ret = {
				this.ID,
				this.NameID,
				this.ModeRes,
				this.HeadRes,
				this.MoveSpeed,
				this.RotateSpeed,
				this.Width,
				this.Height,
				this.UiPos,
				this.UiRot,
				this.UiScale,
				this.SelectBox
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

		this.MoveSpeed = sheet.GetFloat(rowIndex, colIndex++);
		this.RotateSpeed = sheet.GetFloat(rowIndex, colIndex++);
		this.Width = sheet.GetFloat(rowIndex, colIndex++);
		this.Height = sheet.GetFloat(rowIndex, colIndex++);

		this.UiPos = sheet.GetString(rowIndex, colIndex++);
		this.UiRot = sheet.GetString(rowIndex, colIndex++);
		this.UiScale = sheet.GetString(rowIndex, colIndex++);
		this.SelectBox = sheet.GetString(rowIndex, colIndex++);
	}

	public override void DoClone (EN_BaseXls org)
	{
		if (!(org is EN_Npc)) {
			return;
		}

		EN_Npc tmp = (EN_Npc)org;

		base.DoClone (tmp);

		this.ID = tmp.ID;
		this.NameID = tmp.NameID;
		this.ModeRes = tmp.ModeRes;
		this.HeadRes = tmp.HeadRes;
		this.MoveSpeed = tmp.MoveSpeed;
		this.RotateSpeed = tmp.RotateSpeed;
		this.Width = tmp.Width;
		this.Height = tmp.Height;
		this.UiPos = tmp.UiPos;
		this.UiRot = tmp.UiRot;
		this.UiScale = tmp.UiScale;
		this.SelectBox = tmp.SelectBox;
	}
}
