using UnityEngine;
using System.Collections;
using LitJson;

/// <summary>
/// 类名 : Editor 下面的时间轴上的停顿帧
/// 作者 : Canyon
/// 日期 : 2017-01-23 15:35
/// 功能 : 
/// </summary>
public class EDT_Stay : EDT_Base {

	// 持续时间
	public float m_fDuration;

	public EDT_Stay():base(){
		this.m_emType = EventType.Stay;
	}

	public override void OnReInit (float castTime, LitJson.JsonData jsonData)
	{
		base.OnReInit (castTime, jsonData);

		this.m_fDuration = float.Parse (jsonData ["m_pauseSeconds"].ToString ());

		this.m_isJsonDataToSelfSuccessed = true;
		this.m_isInitedFab = true;
	}

	public override JsonData ToJsonData ()
	{
		if (this.m_fDuration <= 0 )
			return null;
		
		JsonData ret = new JsonData ();
		ret["m_typeInt"] = (int)this.m_emType;
		ret["m_pauseSeconds"] = Round2D(this.m_fDuration,2);
		return ret;
	}

	public override void OnClear ()
	{
		base.OnClear ();
		this.m_emType = EventType.Stay;
	}
}
