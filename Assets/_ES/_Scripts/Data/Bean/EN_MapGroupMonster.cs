using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : excel 怪物组数据表 实体对象
/// 作者 : Canyon
/// 日期 : 2017-04-18 13:50
/// 功能 : 
/// </summary>
[System.Serializable]
public class EN_MapGroupMonster : EN_BaseXls{
	
	// 地图 ID
	public int MapId;

	// 创建条件枚举
	public int ConditionEnum;

	// 是否重生
	public int IsReborn;

	// 重生组ID
	public int RebornGroupId;

	// 重生时间间隔
	public float Duration;

	// 死亡多少只后进行刷新
	public int DeadNum;

	// 刷怪点
	public string strMonsters;

	public EN_MapGroupMonster():base(){
	}

	protected override object[] Columns
	{
		get { 
			object[] ret = {
				this.ID,
				this.MapId,
				this.ConditionEnum,
				this.IsReborn,
				this.RebornGroupId,
				this.Duration,
				this.DeadNum,
				this.strMonsters,
			};
			return ret;
		}
	}

	public override void DoInit (int rowIndex, NH_Sheet sheet)
	{
		base.DoInit (rowIndex, sheet);

		int colIndex = 0;
		this.ID = sheet.GetInt(rowIndex, colIndex++);
		this.MapId = sheet.GetInt(rowIndex, colIndex++);
		this.ConditionEnum = sheet.GetInt(rowIndex, colIndex++);
		this.IsReborn = sheet.GetInt(rowIndex, colIndex++);
		this.RebornGroupId = sheet.GetInt(rowIndex, colIndex++);
		this.Duration = sheet.GetFloat(rowIndex, colIndex++);
		this.DeadNum = sheet.GetInt(rowIndex, colIndex++);
		this.strMonsters = sheet.GetString(rowIndex, colIndex++);
	}

	public override void DoClone (EN_BaseXls org)
	{
		if (!(org is EN_MapGroupMonster)) {
			return;
		}

		EN_MapGroupMonster tmp = (EN_MapGroupMonster)org;

		base.DoClone (tmp);

		this.ID = tmp.ID;
		this.MapId = tmp.MapId;
		this.ConditionEnum = tmp.ConditionEnum;
		this.IsReborn = tmp.IsReborn;
		this.RebornGroupId = tmp.RebornGroupId;
		this.Duration = tmp.Duration;
		this.DeadNum = tmp.DeadNum;
		this.strMonsters = tmp.strMonsters;
	}
}
