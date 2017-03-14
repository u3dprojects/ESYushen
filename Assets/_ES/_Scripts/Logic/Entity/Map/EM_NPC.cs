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

	// 对话-语言表ID
	public int m_iTalkId = 0;

	public EM_NPC() : base(){
		m_iCoreCursorMonster = (CORE_CURSOR++);
	}

	protected override bool OnInit ()
	{
		bool isOkey = base.OnInit ();
		if (isOkey) {
			IDictionary map = (IDictionary)m_jdOrg;
			if (map.Contains ("talkID")) {
				this.m_iTalkId = (int)m_jdOrg ["talkID"];
			}
		}
		return isOkey;
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

	protected Vector3 ToV3(string str,Vector3 def){
		if (string.IsNullOrEmpty (str) || "".Equals(str.Trim())) {
			return def;
		}

		string[] arrs = str.Split (",".ToCharArray ());
		int lens = arrs.Length;
		if (lens > 0)
			def.x = float.Parse (arrs [0]);
		if (lens > 1)
			def.y = float.Parse (arrs [1]);
		if (lens > 2)
			def.z = float.Parse (arrs [2]);

		return def;
	}

	protected string ToStr(Vector3 v3){
		return Round2D (v3.x, 2) + "," + Round2D (v3.y, 2) + "," + Round2D (v3.z, 2);
	}

	public override JsonData ToJsonData ()
	{
		JsonData ret = base.ToJsonData ();
		if(ret != null){
			IDictionary map = (IDictionary)ret;
			map.Remove ("reliveInterval");
			map.Remove ("level");

			ret ["talkID"] = m_iTalkId;
		}
		return ret;
	}

	public static void DoClearStatic ()
	{
		CORE_CURSOR = 0;
	}
}
