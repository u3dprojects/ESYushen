using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : excel Map 实体对象
/// 作者 : Canyon
/// 日期 : 2017-02-03 09:20
/// 功能 : 
/// 触发器点 triggers 没有用了
/// </summary>
public class EN_Map : EN_BaseXls{
	// public int ID;
	// 语言表里面的ID
	public int NameID;
	// 语言表里面的ID
	public int DescID;

	// 地图场景资源名
	public int SceneResId;
	public string SceneName;

	// 雷达UI资源名
	public string UIResName;

	// 雷达起始点X
	public float radarOffsetX;

	// 雷达起始点Z
	public float radarOffsetZ;

	// 雷达长度
	public float radarLength;

	// 雷达宽度
	public float radarWidth;

	// 出生x坐标
	public float PosX;

	// 出生z坐标
	public float PosZ;

	// 出生朝向（度数）
	public float Rotation;

	// 地图宽
	public float Width;

	// 地图长
	public float Length;

	// 背景音乐
	public string BgMusic;

	// 分块列数
	public int NodeColumn;

	// 分块行数
	public int NodeRow;

	// 刷怪点
	public string strMonsters;

	// UI上面显示怪物聚集的中心点
	public string strMonsterCenters;

	// 刷npc点
	public string strNpcs;

	// 区域类型
	public int AreaStatus;

	// 区域json
	// public string strAreas;

	public EN_Map():base(){
	}

	protected override object[] Columns
	{
		get { 
			object[] ret = {
				this.ID,
				this.NameID,
				this.DescID,
				this.SceneResId,
				this.UIResName,
				this.radarOffsetX,
				this.radarOffsetZ,
				this.radarLength,
				this.radarWidth,
				this.PosX,
				this.PosZ,
				this.Rotation,
				this.Width,
				this.Length,
				this.BgMusic,
				this.NodeColumn,
				this.NodeRow,
				this.strMonsters,
				this.strMonsterCenters,
				this.strNpcs,
				this.AreaStatus
				// this.strAreas
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
		this.DescID = sheet.GetInt(rowIndex, colIndex++);
		this.SceneResId = sheet.GetInt(rowIndex, colIndex++);
		this.UIResName = sheet.GetString(rowIndex, colIndex++);
		this.radarOffsetX = sheet.GetFloat(rowIndex, colIndex++);
		this.radarOffsetZ = sheet.GetFloat(rowIndex, colIndex++);
		this.radarLength = sheet.GetFloat(rowIndex, colIndex++);
		this.radarWidth = sheet.GetFloat(rowIndex, colIndex++);

		this.PosX = sheet.GetFloat(rowIndex, colIndex++);
		this.PosZ = sheet.GetFloat(rowIndex, colIndex++);
		this.Rotation = sheet.GetFloat(rowIndex, colIndex++);
		this.Width = sheet.GetFloat(rowIndex, colIndex++);
		this.Length = sheet.GetFloat(rowIndex, colIndex++);

		this.BgMusic = sheet.GetString(rowIndex, colIndex++);
		this.NodeColumn = sheet.GetInt(rowIndex, colIndex++);
		this.NodeRow = sheet.GetInt(rowIndex, colIndex++);
		this.strMonsters = sheet.GetString(rowIndex, colIndex++);
		this.strMonsterCenters = sheet.GetString(rowIndex, colIndex++);
		this.strNpcs = sheet.GetString(rowIndex, colIndex++);
		this.AreaStatus = sheet.GetInt(rowIndex, colIndex++);
		// this.strAreas = sheet.GetString(rowIndex, colIndex++);

		this.SceneName = "Map_" + this.SceneResId;
	}

	public override void DoClone (EN_BaseXls org)
	{
		if (!(org is EN_Map)) {
			return;
		}

		EN_Map tmp = (EN_Map)org;

		base.DoClone (tmp);

		this.ID = tmp.ID;
		this.NameID = tmp.NameID;
		this.DescID = tmp.DescID;
		this.SceneResId = tmp.SceneResId;
		this.SceneName = tmp.SceneName;
		this.UIResName = tmp.UIResName;

		this.radarOffsetX = tmp.radarOffsetX;
		this.radarOffsetZ = tmp.radarOffsetZ;
		this.radarLength = tmp.radarLength;
		this.radarWidth = tmp.radarWidth;

		this.PosX = tmp.PosX;
		this.PosZ = tmp.PosZ;
		this.Rotation = tmp.Rotation;
		this.Width = tmp.Width;
		this.Length = tmp.Length;

		this.BgMusic = tmp.BgMusic;
		this.NodeColumn = tmp.NodeColumn;
		this.NodeRow = tmp.NodeRow;
		this.strMonsters = tmp.strMonsters;
		this.strMonsterCenters = tmp.strMonsterCenters;
		this.strNpcs = tmp.strNpcs;
		this.AreaStatus = tmp.AreaStatus;
		// this.strAreas = tmp.strAreas;
	}
}
