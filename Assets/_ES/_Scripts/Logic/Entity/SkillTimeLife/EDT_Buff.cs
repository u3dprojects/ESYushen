using UnityEngine;
using System.Collections;
using LitJson;

/// <summary>
/// 类名 : 创建buffer
/// 作者 : Canyon
/// 日期 : 2017-01-20 09:55
/// 功能 : 
/// </summary>
public class EDT_Buff : EDT_Base {

	// buff id
	public int m_iBuffId;

	// buff 有效时间
	public float m_fDuration;

	// buff 等级
	public int m_iLevel;

	// buff 参数概率【0-10000]
	public int m_iRate;

	public EDT_Buff() : base(){
		this.m_iCurType = 9;
	}

	public override void OnReInit (float castTime, JsonData jsonData)
	{
		base.OnReInit (castTime, jsonData);

		this.m_iBuffId = (int)jsonData ["m_id"];
		this.m_fDuration = float.Parse (jsonData ["m_duration"].ToString ());
		this.m_iLevel = (int)jsonData["m_level"];
		this.m_iRate = (int)jsonData ["m_probability"];

		this.m_isJsonDataToSelfSuccessed = true;
		this.m_isInitedFab = true;
	}

	public override JsonData ToJsonData ()
	{
		if (this.m_iBuffId <= 0)
			return null;

		JsonData ret = new JsonData ();
		ret ["m_typeInt"] = this.m_iCurType;
		ret ["m_id"] = this.m_iBuffId;
		ret ["m_duration"] = Round2D(this.m_fDuration,2);
		ret ["m_level"] = this.m_iLevel;
		ret ["m_probability"] = this.m_iRate;
		return ret;
	}

	public override void OnClear ()
	{
		base.OnClear ();
		this.m_iCurType = 9;
		this.m_iBuffId = 0;
	}
}
