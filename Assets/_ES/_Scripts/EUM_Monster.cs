using UnityEngine;
using System.Collections;

/// 类名 : Map Cell 单元 - 怪物
/// 作者 : Canyon
/// 日期 : 2017-02-06 16:06
/// 功能 : 创建类型分类判断
/// </summary>
public class EUM_Monster : EUM_Cell {
	public EM_Monster m_entity;

	public override void ToTrsfData ()
	{
		m_entity.ToTrsfData ();
	}
}
