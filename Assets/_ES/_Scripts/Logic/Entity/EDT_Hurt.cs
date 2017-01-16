using UnityEngine;
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

	// 是否绘制伤害区域
	public bool m_isShowArea;

	// 伤害区域显示超时时间
	public float m_fTimeOutShowArea;

	// 伤害区域列表(m_zones)
	List<EDT_Hurt_Area> m_lHurtAreas;
	List<EDT_Hurt_Area> m_lAreas = new List<EDT_Hurt_Area>();

	public EDT_Hurt():base(){
		this.m_iCurType = 6;
	}

	public override void OnReInit (float castTime, JsonData jsonData)
	{
		base.OnReInit (castTime, jsonData);

		this.m_iTargetFilter = (int)jsonData ["m_targetFilter"];
		this.m_iTargetCount = (int)jsonData ["m_targetCount"];

		m_lHurtAreas = new List<EDT_Hurt_Area> ();

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

		this.m_isJsonDataToSelfSuccessed = true;
		this.m_isInitedFab = true;
	}

	public override JsonData ToJsonData ()
	{
		if (!this.m_isInitedFab || this.m_lHurtAreas == null || this.m_lHurtAreas.Count <= 0)
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

		return ret;
	}

	protected override bool OnCallEvent ()
	{
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
		if (m_lHurtAreas != null) {
			m_lHurtAreas.Clear ();
			m_lHurtAreas = null;
		}
		m_lAreas.Clear ();
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
		m_lAreas.Clear ();
		if (m_lHurtAreas != null && m_lHurtAreas.Count > 0) {
			m_lAreas.AddRange (m_lHurtAreas);
		}
		return m_lAreas;
	}

	public void DrawAreaInSceneView(){
		if (this.m_isShowArea) {
			List<EDT_Hurt_Area> list = GetAreaList ();
			int lens = list.Count;
			EDT_Hurt_Area tmp = null;
			for (int i = 0; i < lens; i++) {
				tmp = list [i];
				tmp.DrawAreaInSceneView (m_trsfOwner);
			}
		}
	}
}
