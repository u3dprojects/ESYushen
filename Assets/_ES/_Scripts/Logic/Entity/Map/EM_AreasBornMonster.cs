using UnityEngine;
using System.Collections;
using LitJson;
using UnityEditor;

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
	public float m_fRadius = 1;

	// 是否循环(该group刷完后,再次刷新该组数据)
	public bool m_isRound = false;

	// 间隔时间
	public float m_fInterval = 1;

	// 刷新次数
	public int m_iNum = 1;


	// 区域颜色
	public Color m_cAreaColor = new Color(Random.Range(0f,1.0f),Random.Range(0f,1.0f),Random.Range(0f,1.0f),0.15f);

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

			if (map.Contains ("isRound")) {
				this.m_isRound = (bool)m_jdOrg ["isRound"];
			}

			if (map.Contains ("interval")) {
				this.m_fInterval = float.Parse (m_jdOrg ["interval"].ToString ());
			}

			if (map.Contains ("num")) {
				this.m_iNum = (int)m_jdOrg ["num"];
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
			ret ["isRound"] = this.m_isRound;
			ret ["interval"] = Round2D(this.m_fInterval,2);
			ret ["num"] = this.m_iNum;
		}
		return ret;
	}

	public override void OnSceneGUI ()
	{
		base.OnSceneGUI ();
		if (this.m_trsf != null) {
			Handles.color = this.m_cAreaColor;
			Vector3 posOrg = this.m_trsf.position;
			Vector3 pos = posOrg;
			Handles.DrawSolidDisc(pos,Vector3.up,this.m_fRadius);
		}
	}

	public static void DoClearStatic ()
	{
		CORE_CURSOR = 0;
	}
}
