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
	
	public EDT_HitStatus():base(){
		this.m_emType = EventType.BeHitDefault;
	}

	public override void OnReInit (float castTime, LitJson.JsonData jsonData)
	{
		base.OnReInit (castTime, jsonData);

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
}
