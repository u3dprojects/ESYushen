using UnityEngine;
using System.Collections;
using LitJson;

/// <summary>
/// 类名 : Editor 下面的时间轴上的受击者的表现形式(无，退，飞，倒，晕)
/// 作者 : Canyon
/// 日期 : 2017-01-24 10:35
/// 功能 : 
/// </summary>
public class EDT_HitStatus : EDT_Base {
	// (0无，1退，2飞，3倒，4晕)
	public int m_hitStatus = 0;

	public EDT_HitStatus():base(){
		this.m_emType = EventType.BeHitDefault;
		m_hitStatus = 0;
	}

	public override void OnReInit (float castTime, LitJson.JsonData jsonData)
	{
		base.OnReInit (castTime, jsonData);

		InitHitStatus ();
		this.m_isJsonDataToSelfSuccessed = true;
		this.m_isInitedFab = true;
	}

	public override JsonData ToJsonData ()
	{
		JsonData ret = new JsonData ();
		ret["m_typeInt"] = (int)this.m_emType;
		return ret;
	}

	public override void OnClear ()
	{
		base.OnClear ();
		this.m_emType = EventType.BeHitDefault;
	}

	public void Status2Enum(){
		switch (m_hitStatus) {
		case 1:
			this.m_emType = EDT_Base.EventType.BeHitBack;
			break;
		case 2:
			this.m_emType = EDT_Base.EventType.BeHitFly;
			break;
		default:
			this.m_emType = EDT_Base.EventType.BeHitDefault;
			break;
		}
	}

	public void InitHitStatus(){
		switch (m_emType) {
		case EventType.BeHitBack:
			m_hitStatus = 1;
			break;
		case EventType.BeHitFly:
			m_hitStatus = 2;
			break;
		default:
			m_hitStatus = 0;
			break;
		}
	}
}
