using UnityEngine;
using System.Collections;
using LitJson;

/// <summary>
/// 类名 : 地图元素 - 怪物聚集中心点
/// 作者 : Canyon
/// 日期 : 2017-03-14 11:10
/// 功能 : 用于UI缩略图展示上面用
/// </summary>
[System.Serializable]
public class EM_MonsterCenter : EM_UnitCell {

	// 对象的唯一标识 计数器
	static int CORE_CURSOR = 0;
	int m_iCoreCursorMonster = 0;

	// 类型脚本
	[System.NonSerialized]
	public EUM_MonsterCenter m_csCell;


	public EM_MonsterCenter() : base(){
		m_iCoreCursorMonster = (CORE_CURSOR++);
	}

	protected override void OnResetGobjName ()
	{
		SetGobjName ("MonsterCenter" + m_iCoreCursorMonster);
	}

	protected override bool OnClone (GameObject org)
	{
		bool ret = base.OnClone (org);
		if (ret) {
			EUM_MonsterCenter m_csCell = org.GetComponent<EUM_MonsterCenter> ();
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
			m_csCell = m_gobj.GetComponent<EUM_MonsterCenter> ();
			if (m_csCell == null) {
				m_csCell = m_gobj.AddComponent<EUM_MonsterCenter> ();
			}
		}

		if (m_csCell != null) {
			m_csCell.m_entity = this;
		}
	}

	public override JsonData ToJsonData ()
	{
		JsonData ret = base.ToJsonData ();
		if(ret != null){
			IDictionary map = (IDictionary)ret;
			map.Remove ("reliveInterval");
			map.Remove ("rotateDegree");
		}
		return ret;
	}

	public static void DoClearStatic ()
	{
		CORE_CURSOR = 0;
	}
}
