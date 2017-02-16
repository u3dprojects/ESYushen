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
	int m_iCoreCursorMonster = 0;

	// 类型脚本
	[System.NonSerialized]
	public EUM_Npc m_csCell;


	public EM_NPC() : base(){
		m_iCoreCursorMonster = (CORE_CURSOR++);
	}

	protected override void OnResetGobjName ()
	{
		SetGobjName ("NPC" + m_iCoreCursorMonster);
	}

	protected override bool OnClone (GameObject org)
	{
		bool ret = base.OnClone (org);
		if (ret) {
			EUM_Npc m_csCell = org.GetComponent<EUM_Npc> ();
			OnCloneData (m_csCell.m_entity);

			ToTrsfRotation ();
			ToData ();
		}
		return ret;
	}

	protected override void ResetCShape ()
	{
		if (m_gobj != null && m_csCell == null) {
			// 添加一个脚本作为类型判断
			m_csCell = m_gobj.GetComponent<EUM_Npc> ();
			if (m_csCell == null) {
				m_csCell = m_gobj.AddComponent<EUM_Npc> ();
			}
		}

		if (m_csCell != null) {
			m_csCell.m_entity = this;
		}
	}

	public static void DoClearStatic ()
	{
		CORE_CURSOR = 0;
	}
}
