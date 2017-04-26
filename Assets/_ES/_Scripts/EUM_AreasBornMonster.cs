using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : 地图元素 - 触发刷怪区域
/// 作者 : Canyon
/// 日期 : 2017-04-26 15:40
/// 功能 : 用于服务器刷怪
/// </summary>
public class EUM_AreasBornMonster : EUM_Cell {
	public EM_AreasBornMonster m_entity;

	public override void ToTrsfData ()
	{
		m_entity.ToTrsfData ();
	}
}
