using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.ComponentModel;

/// <summary>
/// 类名 : Editor 下面的时间轴上的伤害处理(打击点检测hitCheck)
/// 作者 : Canyon
/// 日期 : 2017-01-13 09:50
/// 功能 : 
/// </summary>
public class EDT_Hurt : EDT_Base {

	public enum HurtType
	{
		[Description("攻击锁定目标")]
		OneTarget = 0,

		[Description("区域内攻击目标")]
		MoveTarget = 1
	}

	HurtType _m_emType = HurtType.MoveTarget;

	public HurtType  m_emType{
		get{ return _m_emType; }
		set{
			_m_emType = value;
			this.m_iCurType = _m_emType == HurtType.OneTarget ? 5 : 6;
		}
	}

	// 优先目标
	public int m_iTargetFilter;

	// 攻击数量
	public int m_iTargetCount;

	// 伤害区域列表(m_zones)
	EMT_HitArea m_eHitArea = new EMT_HitArea ();

	// 受击方收到的伤害状态(伤害值，特效等)
	EMT_HitEvent m_eHitEvent = new EMT_HitEvent();

	public EDT_Hurt():base(){
		m_emType = HurtType.MoveTarget;
	}

	public override void OnReInit (float castTime, JsonData jsonData)
	{
		base.OnReInit (castTime, jsonData);

		IDictionary dicJsonData = (IDictionary)jsonData;

		if (dicJsonData.Contains ("m_targetFilter")) {
			this.m_iTargetFilter = (int)jsonData ["m_targetFilter"];
		}

		if (dicJsonData.Contains ("m_targetCount")) {
			this.m_iTargetCount = (int)jsonData ["m_targetCount"];
		}


		JsonData tmp = null;
		IDictionary dicJsonData2 = null;
		if (dicJsonData.Contains ("m_zoneHelper")) {
			tmp = jsonData ["m_zoneHelper"];
			dicJsonData2 = (IDictionary)tmp;
			if (dicJsonData2.Contains ("m_zones")) {
				m_eHitArea.DoReInit (tmp ["m_zones"], castTime);
			}
		}

		if (dicJsonData.Contains ("m_shotEvents")) {
			tmp = jsonData ["m_shotEvents"];
			m_eHitEvent.DoReInit(tmp,castTime);
		}

		this.m_isJsonDataToSelfSuccessed = true;
		this.m_isInitedFab = true;
	}

	public override JsonData ToJsonData ()
	{
		if (!this.m_isInitedFab)
			return null;

		JsonData ret = new JsonData ();
		ret["m_typeInt"] = this.m_iCurType;
		JsonData tmp;

		if (m_emType == HurtType.MoveTarget) {
			ret ["m_targetFilter"] = this.m_iTargetFilter;
			ret ["m_targetCount"] = this.m_iTargetCount;

			JsonData tmpArea = m_eHitArea.ToJsonData();
			if (tmpArea != null) {
				tmp = new JsonData ();
				tmp ["m_zones"] = tmpArea;
				ret ["m_zoneHelper"] = tmp;
			}
		}

		tmp = this.m_eHitEvent.ToJsonData ();
		ret["m_shotEvents"] = tmp;
		return ret;
	}

	protected override bool OnCallEvent ()
	{
		if (m_eHitEvent.m_isCanShow) {
			m_eHitEvent.DoStart ();
		}

		m_eHitArea.DoStart ();

		Debug.Log ("=hurt=");
		return true;
	}

	protected override void OnCallUpdate (float upDeltaTime)
	{
		base.OnCallUpdate (upDeltaTime);
		if (m_eHitEvent.m_isCanShow) {
			m_eHitEvent.DoUpdate (upDeltaTime);
		}
		m_eHitArea.DoUpdate (upDeltaTime);
	}

	public override void OnClear ()
	{
		base.OnClear ();

		m_emType = HurtType.MoveTarget;

		m_eHitArea.DoClear ();
		m_eHitEvent.DoClear ();
	}

	public override void OnSceneGUI (Transform trsfOrg)
	{
		base.OnSceneGUI (trsfOrg);
		m_eHitArea.OnSceneGUI(trsfOrg);
	}

	public override void DoEnd ()
	{
		base.DoEnd ();
		Debug.Log ("Hurt Do End");
	}

	#region ===  伤害区域 ===

	public void NewHitArea(){
		m_eHitArea.NewArea();
	}

	public void RemoveHitArea(EDT_Hurt_Area hurtArea){
		m_eHitArea.RmArea (hurtArea);
	}

	public List<EDT_Hurt_Area> GetAreaList(){
		return m_eHitArea.GetLAreas ();
	}

	#endregion

	#region === 受击者相关信息绘制 ===

	public bool m_isShowBeHitterWhenPlay{
		get{ return m_eHitEvent.m_isCanShow; }
		set{
			m_eHitEvent.m_isCanShow = value;
		}
	}

	public void RemoveEvent(EDT_Base evt){
		m_eHitEvent.RmEvent (evt);
	}

	public void NewBeHitStatus(){
		m_eHitEvent.NewEvent<EDT_Property>();
	}

	public List<EDT_Property> GetHitStatusList(){
		return m_eHitEvent.GetLAttrs ();
	}

	public void NewBeHitAudio(){
		m_eHitEvent.NewEvent<EDT_Audio>();
	}

	public List<EDT_Audio> GetHitAudioList(){
		return m_eHitEvent.GetLAudios ();
	}

	public void NewHitBuff(){
		m_eHitEvent.NewEvent<EDT_Buff>();
	}

	public List<EDT_Buff> GetHitBuffList(){
		return m_eHitEvent.GetLBuffs ();
	}

	#endregion
}
