﻿using UnityEngine;
using System.Collections;
using LitJson;

/// <summary>
/// 类名 : 创建 Bullet
/// 作者 : Canyon
/// 日期 : 2017-01-23 13:40
/// 功能 : 
/// </summary>
public class EDT_Bullet : EDT_Base {

	// id
	public int m_iID;

	// 偏移
	public Vector3 m_v3OffsetPos;

	//顺时针旋转角度偏移量,就是y轴值
	public float m_fRotation;

	public EDT_Bullet() : base(){
		this.m_emType = EventType.Bullet;
	}

	public override void OnReInit (float castTime, JsonData jsonData)
	{
		base.OnReInit (castTime, jsonData);

		this.m_iID = (int)jsonData ["m_id"];
		this.m_fRotation = float.Parse (jsonData ["m_rotDeg"].ToString ());
		this.m_v3OffsetPos = Vector3.zero;

		this.m_v3OffsetPos.x = float.Parse (jsonData ["m_posX"].ToString ());
		this.m_v3OffsetPos.y = float.Parse (jsonData ["m_posY"].ToString ());
		this.m_v3OffsetPos.z = float.Parse (jsonData ["m_posZ"].ToString ());

		this.m_isJsonDataToSelfSuccessed = true;
		this.m_isInitedFab = true;
	}

	public override JsonData ToJsonData ()
	{
		if (this.m_iID <= 0)
			return null;

		JsonData ret = new JsonData ();
		ret ["m_typeInt"] = (int)this.m_emType;
		ret ["m_id"] = this.m_iID;
		ret ["m_rotDeg"] = Round2D(this.m_fRotation,2);
		ret ["m_posX"] = Round2D(this.m_v3OffsetPos.x,2);
		ret ["m_posY"] = Round2D(this.m_v3OffsetPos.y,2);
		ret ["m_posZ"] = Round2D(this.m_v3OffsetPos.z,2);
		return ret;
	}

	public override void OnClear ()
	{
		base.OnClear ();
		this.m_emType = EventType.Bullet;
		this.m_iID = 0;
	}

	EN_OptMonster optMonster {
		get {
			return EN_OptMonster.Instance;
		}
	}

	void InitBulletFab(){
		if (!optMonster.isInitSuccessed)
			return;

		EN_Monster one = optMonster.GetEntity (m_iID);
		if (one == null)
			return;
		DoReInit (one.ModeRes, EventType.Bullet);
	}

	public override string GetPathByNameType (string objName, EventType type)
	{
		if (type != EventType.Bullet) {
			return "类型不对";
		}
		return "Assets\\PackResources\\Arts\\Effect\\Prefabs\\"+objName+".prefab";
	}

	protected override bool OnCallEvent ()
	{
		return base.OnCallEvent ();
	}

	protected override void OnCallUpdate (float upDeltaTime)
	{
		base.OnCallUpdate (upDeltaTime);
	}
}
