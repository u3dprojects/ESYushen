﻿using UnityEngine;
using System.Collections;
using LitJson;

/// <summary>
/// 类名 : 地图基础类之怪兽
/// 作者 : Canyon
/// 日期 : 2017-02-04 09:36
/// 功能 : 主要是处理怪兽信息等
/// </summary>
[System.Serializable]
public class EM_Monster : EM_UnitCell {

	// 对象的唯一标识 计数器
	static int CORE_CURSOR = 0;
	int m_iCoreCursorMonster = 0;

	// 类型脚本
	[System.NonSerialized]
	public EUM_Monster m_csCell;


	public EM_Monster() : base(){
		m_iCoreCursorMonster = (CORE_CURSOR++);
	}

	protected override void OnResetGobjName ()
	{
		SetGobjName ("Monster" + m_iCoreCursorMonster);
	}

	protected override bool OnClone (GameObject org)
	{
		bool ret = base.OnClone (org);
		if (ret) {
			EUM_Monster m_csCell = org.GetComponent<EUM_Monster> ();
			OnCloneData (m_csCell.m_entity);
		}
		return ret;
	}

	protected override void ResetCShape ()
	{
		if (m_gobj != null && m_csCell == null) {
			// 添加一个脚本作为类型判断
			m_csCell = m_gobj.GetComponent<EUM_Monster> ();
			if (m_csCell == null) {
				m_csCell = m_gobj.AddComponent<EUM_Monster> ();
			}
		}

		if (m_csCell != null) {
			m_csCell.m_entity = this;
		}
	}

	new public static void DoClearStatic ()
	{
		CORE_CURSOR = 0;
	}
}
