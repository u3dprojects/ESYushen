using UnityEngine;
using System.Collections;
using LitJson;

public class EM_Monster : EM_Cube {

	// 对象的唯一标识 计数器
	static int CORE_CURSOR = 0;
	int m_iCoreCursorMonster = 0;

	// monsterID 数据表结构里面的唯一标识ID
	public int m_iID;

	// 出生点
	public Vector3 m_v3Pos = Vector3.zero;

	// 旋转角度偏移量,就是y轴值
	public float m_fRotation;

	// 刷怪时间间隔
	public float m_fReliveInv;

	public EM_Monster() : base(){
		m_sPrefixName = "Monster";
		m_iCoreCursorMonster = (CORE_CURSOR++);
	}

	protected override bool OnInit ()
	{
		if (m_jdOrg != null) {
			this.m_iID = (int)m_jdOrg ["monsterID"];
			this.m_fRotation = float.Parse (m_jdOrg ["rotateDegree"].ToString ());
			this.m_fReliveInv = float.Parse (m_jdOrg ["reliveInterval"].ToString ());
			m_v3Pos.x = float.Parse (m_jdOrg ["positionX"].ToString ());
			m_v3Pos.z = float.Parse (m_jdOrg ["positionZ"].ToString ());

			DoNew ();
			ToTrsfData ();
			return true;
		}
		return false;
	}

	protected override void OnResetGobjName ()
	{
		if (string.IsNullOrEmpty (m_sGName)) {
			m_sGName = m_sPrefixName + m_iCoreCursorMonster;
		}
		ResetGobjName();
	}

	public override JsonData ToJsonData ()
	{
		if (this.m_iID <= 0)
			return null;

		JsonData ret = new JsonData ();
		ret ["monsterID"] = this.m_iID;
		ret ["rotateDegree"] = Round2D(this.m_fRotation,2);
		ret ["reliveInterval"] = Round2D(this.m_fReliveInv,2);
		ret ["positionX"] = Round2D(this.m_v3Pos.x,2);
		ret ["positionZ"] = Round2D(this.m_v3Pos.z,2);

		return ret;
	}

	public override void OnChangePosition (Transform trsf)
	{
		base.OnChangePosition (trsf);
		if (trsf == m_trsf) {
			ToData ();
		}
	}

	public override void OnChangeRotation (Transform trsf)
	{
		base.OnChangeRotation (trsf);
		if (trsf == m_trsf) {
			ToData ();
		}
	}

	public void DoMakeNew(){
		DoNew ();
		ToData ();
	}

	public void ToData(){
		m_v3Pos = m_trsf.position;
		m_fRotation = m_trsf.eulerAngles.y;
	}

	public void ToTrsfData(){
		m_trsf.position = m_v3Pos;
		Vector3 v3Rotation = m_trsf.eulerAngles;
		v3Rotation.y = m_fRotation;
		m_trsf.eulerAngles = v3Rotation;
	}

	public static void DoClearStatic ()
	{
		EM_Cube.DoClearStatic ();

		CORE_CURSOR = 0;
	}
}
