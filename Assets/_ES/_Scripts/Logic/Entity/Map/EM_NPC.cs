using UnityEngine;
using System.Collections;
using LitJson;

/// <summary>
/// 类名 : 地图基础类之NPC
/// 作者 : Canyon
/// 日期 : 2017-02-04 09:36
/// 功能 : 主要是处理NPC信息等
/// </summary>
[System.Serializable]
public class EM_NPC : EM_UnitCell {
	// 对象的唯一标识 计数器
	static int CORE_CURSOR = 0;
	int m_iCoreCursorNpc = 0;

	public EM_NPC() : base(){
		m_iCoreCursorNpc = (CORE_CURSOR++);
	}

	protected override void OnResetGobjName ()
	{
		SetGobjName ("NPC" + CORE_CURSOR);
	}

	new public static void DoClearStatic ()
	{
		CORE_CURSOR = 0;
	}
}
