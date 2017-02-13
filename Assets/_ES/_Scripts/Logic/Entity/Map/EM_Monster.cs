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

	public EM_Monster() : base(){
		m_sPrefixName = "Monster";
		m_iCoreCursorMonster = (CORE_CURSOR++);
	}

	protected override void OnResetGobjName ()
	{
		if (string.IsNullOrEmpty (m_sGName)) {
			m_sGName = m_sPrefixName + m_iCoreCursorMonster;
		}
		ResetGobjName();
	}

	new public static void DoClearStatic ()
	{
		EM_UnitCell.DoClearStatic ();
		CORE_CURSOR = 0;
	}
}
