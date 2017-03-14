using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : 地图元素 - 怪物聚集中心点
/// 作者 : Canyon
/// 日期 : 2017-03-14 11:10
/// 功能 : 用于UI缩略图展示上面用
/// </summary>
public class EUM_MonsterCenter : EUM_Cell {
	public EM_MonsterCenter m_entity;

	public override void ToTrsfData ()
	{
		m_entity.ToTrsfData ();
	}
}
