using UnityEngine;
using System.Collections;
using LitJson;

/// <summary>
/// 类名 : Editor 下面的时间轴上的震屏
/// 作者 : Canyon
/// 日期 : 2017-01-18 17:35
/// 功能 : 
/// </summary>
public class EDT_Shake : EDT_Base {

	// 持续时间
	public float m_fDuration;

	// 震屏强度[0-1]
	public float m_fStrength;

	public EDT_Shake():base(){
	}

	public override void OnReInit (float castTime, LitJson.JsonData jsonData)
	{
		base.OnReInit (castTime, jsonData);

		this.m_fDuration = float.Parse (jsonData ["m_duration"].ToString ());
		this.m_fStrength = float.Parse (jsonData ["m_radio"].ToString ());

		this.m_isJsonDataToSelfSuccessed = true;
		this.m_isInitedFab = true;
	}

	public override JsonData ToJsonData ()
	{
		if (this.m_fDuration <= 0 || this.m_fStrength <= 0)
			return null;
		
		JsonData ret = new JsonData ();
		ret["m_typeInt"] = this.m_iCurType;
		ret["m_duration"] = Round2D(this.m_fDuration,2);
		ret["m_radio"] = Round2D(this.m_fStrength,2);
		return ret;
	}

	public override void OnClear ()
	{
		base.OnClear ();
		this.m_iCurType = 3;
	}
}
