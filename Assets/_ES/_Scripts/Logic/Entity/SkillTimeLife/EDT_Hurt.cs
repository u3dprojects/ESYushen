﻿using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;

/// <summary>
/// 类名 : Editor 下面的时间轴上的伤害处理(打击点检测hitCheck)
/// 作者 : Canyon
/// 日期 : 2017-01-13 09:50
/// 功能 : 
/// </summary>
public class EDT_Hurt : EDT_Base {

	// 优先目标
	public int m_iTargetFilter;

	// 攻击数量
	public int m_iTargetCount;

	// 伤害区域列表(m_zones)
	List<EDT_Hurt_Area> m_lHurtAreas = new List<EDT_Hurt_Area>();
	List<EDT_Hurt_Area> m_lCurHurtAreas = new List<EDT_Hurt_Area>();

	// 受击方收到的伤害状态(伤害值，特效等)
	EDT_Hurt_BeHitter m_eBeHitter = new EDT_Hurt_BeHitter();

	public EDT_Hurt():base(){
		this.m_iCurType = 6;
	}

	public override void OnReInit (float castTime, JsonData jsonData)
	{
		base.OnReInit (castTime, jsonData);

		this.m_iTargetFilter = (int)jsonData ["m_targetFilter"];
		this.m_iTargetCount = (int)jsonData ["m_targetCount"];

		IDictionary dicJsonData = (IDictionary)jsonData;

		JsonData tmp = null;
		JsonData tmp2 = null;
		IDictionary dicJsonData2 = null;
		if (dicJsonData.Contains ("m_zoneHelper")) {
			tmp = jsonData ["m_zoneHelper"];
			dicJsonData2 = (IDictionary)tmp;
			if (dicJsonData2.Contains ("m_zones")) {
				tmp2 = tmp ["m_zones"];
				if (tmp2.IsArray) {
					EDT_Hurt_Area hurtArea = null;
					for (int i = 0; i < tmp2.Count; i++) {
						hurtArea = EDT_Hurt_Area.NewHurtArea (tmp2 [i]);
						if (hurtArea != null) {
							m_lHurtAreas.Add (hurtArea);
						}
					}
				}
			}
		}

		if (dicJsonData.Contains ("m_shotEvents")) {
			tmp = jsonData ["m_shotEvents"];
			m_eBeHitter.DoInit (tmp);
		}

		this.m_isJsonDataToSelfSuccessed = true;
		this.m_isInitedFab = true;
	}

	public override JsonData ToJsonData ()
	{
		if (!this.m_isInitedFab || this.m_lHurtAreas.Count <= 0)
			return null;

		JsonData ret = new JsonData ();
		ret["m_typeInt"] = this.m_iCurType;
		ret["m_targetFilter"] = this.m_iTargetFilter;
		ret["m_targetCount"] = this.m_iTargetCount;

		JsonData tmp = new JsonData ();
		JsonData tmp2 = new JsonData ();
		JsonData tmp3;

		foreach (var item in this.m_lHurtAreas) {
			tmp3 = item.ToJsonData ();
			if (tmp3 == null) {
				continue;
			}
			tmp2.Add (tmp3);
		}
		tmp2.SetJsonType (JsonType.Array);
		tmp ["m_zones"] = tmp2;
		ret["m_zoneHelper"] = tmp;

		tmp2 = this.m_eBeHitter.ToJsonData ();
		ret["m_shotEvents"] = tmp2;
		return ret;
	}

	protected override bool OnCallEvent ()
	{
		if (m_eBeHitter.m_isCanShow) {
			m_eBeHitter.DoStart ();
		}

		Debug.Log ("=hurt=");
		return true;
	}

	protected override void OnCallUpdate (float upDeltaTime)
	{
		base.OnCallUpdate (upDeltaTime);
	}

	public override void OnClear ()
	{
		base.OnClear ();

		this.m_iCurType = 6;

		m_lHurtAreas.Clear ();
		m_lCurHurtAreas.Clear ();

		m_eBeHitter.DoClear ();
	}

	public void NewHurtArea(){
		if (m_lHurtAreas == null) {
			return;
		}
		m_lHurtAreas.Add (new EDT_Hurt_Area());
	}

	public void RemoveHurtArea(EDT_Hurt_Area hurtArea){
		m_lHurtAreas.Remove (hurtArea);
	}

	public List<EDT_Hurt_Area> GetAreaList(){
		m_lCurHurtAreas.Clear ();
		if (m_lHurtAreas != null && m_lHurtAreas.Count > 0) {
			m_lCurHurtAreas.AddRange (m_lHurtAreas);
		}
		return m_lCurHurtAreas;
	}

	public override void OnSceneGUI ()
	{
		base.OnSceneGUI ();
		DrawAreaInSceneView ();
	}

	void DrawAreaInSceneView(){
		List<EDT_Hurt_Area> list = GetAreaList ();
		int lens = list.Count;
		EDT_Hurt_Area tmp = null;
		for (int i = 0; i < lens; i++) {
			tmp = list [i];
			tmp.DrawAreaInSceneView (m_trsfOwner);
		}
	}

	#region === 受击者相关信息绘制 ===

	public bool m_isShowBeHitterWhenPlay{
		get{ return m_eBeHitter.m_isCanShow; }
		set{
			m_eBeHitter.m_isCanShow = value;
		}
	}

	public void NewBeHitStatus(){
		m_eBeHitter.NewStatus ();
	}

	public List<EDT_Property> GetHitStatusList(){
		return m_eBeHitter.GetListStatus ();
	}

	public void RemoveBeHitStatus(EDT_Property rm){
		m_eBeHitter.RemoveStatus(rm);
	}

	public EDT_Audio GetBeHitAudio(){
		return m_eBeHitter.m_eAuido;
	}

	#endregion
}
