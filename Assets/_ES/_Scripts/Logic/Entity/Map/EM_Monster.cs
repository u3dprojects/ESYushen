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
public class EM_Monster : EM_Cube {

	// 对象的唯一标识 计数器
	static int CORE_CURSOR = 0;
	int m_iCoreCursorMonster = 0;

	// monsterID 数据表结构里面的唯一标识ID
	public int m_iUnqID;

	// 刷怪时间间隔
	public float m_fReliveInv;
	
	// 出生点
	public Vector3 m_v3Pos = new Vector3(0,10,0);

	// 旋转角度偏移量,就是y轴值
	public float m_fRotation;

	float m_fDefaultY = 999;

	public EM_Monster() : base(){
		m_sPrefixName = "Monster";
		m_iCoreCursorMonster = (CORE_CURSOR++);
	}

	protected override bool OnInit ()
	{
		if (m_jdOrg != null) {
			this.m_iUnqID = (int)m_jdOrg ["monsterID"];
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
		if (this.m_iUnqID <= 0)
			return null;

		JsonData ret = new JsonData ();
		ret ["monsterID"] = this.m_iUnqID;
		ret ["rotateDegree"] = Round2D(this.m_fRotation,2);
		ret ["reliveInterval"] = Round2D(this.m_fReliveInv,2);
		ret ["positionX"] = Round2D(this.m_v3Pos.x,2);
		ret ["positionZ"] = Round2D(this.m_v3Pos.z,2);

		return ret;
	}

	public override void OnChangeTransform (Transform trsf)
	{
		base.OnChangeTransform (trsf);
		if (trsf == m_trsf) {
			ToData ();
		}
	}

	protected override void ResetCShape ()
	{
		base.ResetCShape ();
		if (m_csCell != null) {
			m_csCell.m_entity = this;
		}
	}

	protected override void OnClone (EM_Base org)
	{
		if(org == null)
		{
			return;
		}
		base.OnClone (org);
		if(org is EM_Monster){
			EM_Monster tmp = (EM_Monster)org;
			this.m_fReliveInv = tmp.m_fReliveInv;
			this.m_fRotation = tmp.m_fRotation;
			this.m_iUnqID = tmp.m_iUnqID;
			this.m_v3Pos = tmp.m_v3Pos;
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

	public void DoRaycast(){
		OnRaycast ();
		ToTrsfData ();
	}

	// int layerMask = LayerMask.NameToLayer("Terrain");

	void OnRaycast(){
		// 默认一个y值
		m_v3Pos.y = m_fDefaultY;
		ToTrsfData ();

		RaycastHit[] hits = Physics.RaycastAll (m_v3Pos, Vector3.down, m_fDefaultY + 10);

		RaycastHit hit;
		if (hits != null && hits.Length > 0) {
			hit = hits [0];
			// 取得碰撞点位置
			var item = hit.point;
			m_v3Pos.y = item.y + m_trsf.lossyScale.y / 2f;
		} else {
			m_v3Pos.y = 10;
		}
	}

	new public static void DoClearStatic ()
	{
		EM_Cube.DoClearStatic ();

		CORE_CURSOR = 0;
	}
}
