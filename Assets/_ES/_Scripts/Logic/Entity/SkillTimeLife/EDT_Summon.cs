using UnityEngine;
using System.Collections;
using LitJson;

/// <summary>
/// 类名 : Editor 下面的时间轴上的召唤物
/// 作者 : Canyon
/// 日期 : 2017-02-28 11:50
/// 功能 : 
/// </summary>
public class EDT_Summon : EDT_Base {

	// 持续时间
	public float m_fDuration;

	// 召唤物ID
	public int m_iSummonId;

	public EDT_Summon():base(){
		this.m_emType = EventType.Summon;
	}

	public override void OnReInit (float castTime, LitJson.JsonData jsonData)
	{
		base.OnReInit (castTime, jsonData);

		this.m_fDuration = float.Parse (jsonData ["m_duration"].ToString ());
		this.m_iSummonId = (int)jsonData ["m_summonId"];

		this.m_isJsonDataToSelfSuccessed = true;
		this.m_isInitedFab = true;
	}

	public override JsonData ToJsonData ()
	{
		if (this.m_fDuration <= 0 || this.m_iSummonId <= 0)
			return null;
		
		JsonData ret = new JsonData ();
		ret["m_typeInt"] = (int)this.m_emType;
		ret["m_duration"] = Round2D(this.m_fDuration,2);
		ret["m_summonId"] = this.m_iSummonId;
		return ret;
	}

	public override void OnClear ()
	{
		base.OnClear ();
		this.m_emType = EventType.Summon;
	}
}
