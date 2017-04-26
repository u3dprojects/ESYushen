using UnityEngine;
using System.Collections;
using LitJson;

/// <summary>
/// 类名 : 地图元素 - 触发刷怪区域
/// 作者 : Canyon
/// 日期 : 2017-04-26 15:40
/// 功能 : 用于服务器刷怪
/// </summary>
[System.Serializable]
public class EM_AreasBornMonster : EM_UnitCell {

	// 对象的唯一标识 计数器
	static int CORE_CURSOR = 0;
	int m_iCoreCursorMonster = 0;

	// 类型脚本
	[System.NonSerialized]
	public EUM_AreasBornMonster m_csCell;

	// 半径
	public float m_fRadius;

	public EM_AreasBornMonster() : base(){
		m_iCoreCursorMonster = (CORE_CURSOR++);
	}

	protected override bool OnInit ()
	{
		bool isOkey = base.OnInit ();
		if (isOkey) {
			IDictionary map = (IDictionary)m_jdOrg;
			if (map.Contains ("radius")) {
				this.m_fRadius = float.Parse (m_jdOrg ["radius"].ToString ());
			}
		}
		return isOkey;
	}

	protected override void OnResetGobjName ()
	{
		SetGobjName ("AreasBornMonster" + m_iCoreCursorMonster);
	}

	protected override bool OnClone (GameObject org)
	{
		bool ret = base.OnClone (org);
		if (ret) {
			EUM_AreasBornMonster m_csCell = org.GetComponent<EUM_AreasBornMonster> ();
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
			m_csCell = m_gobj.GetComponent<EUM_AreasBornMonster> ();
			if (m_csCell == null) {
				m_csCell = m_gobj.AddComponent<EUM_AreasBornMonster> ();
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
			map.Remove ("level");

			ret ["radius"] = Round2D(this.m_fRadius,2);
		}
		return ret;
	}

	public static void DoClearStatic ()
	{
		CORE_CURSOR = 0;
	}
}
