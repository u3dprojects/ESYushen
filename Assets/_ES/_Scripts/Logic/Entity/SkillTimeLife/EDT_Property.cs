using UnityEngine;
using System.Collections;
using LitJson;
using System.ComponentModel;

/// <summary>
/// 类名 : 修改属性
/// 作者 : Canyon
/// 日期 : 2017-01-20 09:55
/// 功能 : 
/// </summary>
public class EDT_Property : EDT_Base {

	public enum PropretyTag{
		[Description("永久修改")]
		Forever,

		[Description("短暂修改")]
		ShortTime
	}

	PropretyTag _m_emTag = PropretyTag.Forever;
	public PropretyTag m_emTag{
		get{
			return _m_emTag;
		}
		set{
			_m_emTag = value;
			switch (_m_emTag) {
			case PropretyTag.ShortTime:
				this.m_emType = EventType.Attribute;
				break;
			default:
				this.m_emType = EventType.Property;
				break;
			}
		}
	}

	// 属性分类
	public int m_iGID = 0;

	public string m_sPars = "";

	public EDT_Property():base(){
		m_emTag = PropretyTag.Forever;
	}

	public override void OnReInit (float castTime, JsonData jsonData)
	{
		base.OnReInit (castTime, jsonData);

		this.m_iGID = (int)jsonData ["m_id"];
		this.m_sPars = (string)jsonData ["m_parmStr"];

		this.m_isJsonDataToSelfSuccessed = true;
		this.m_isInitedFab = true;
	}

	public override JsonData ToJsonData ()
	{
		if (string.IsNullOrEmpty (this.m_sPars))
			return null;

		JsonData ret = new JsonData ();
		ret ["m_typeInt"] = (int)this.m_emType;
		ret ["m_id"] = this.m_iGID;
		ret ["m_parmStr"] = this.m_sPars;
		return ret;
	}

	public override void OnClear ()
	{
		base.OnClear ();

		m_emTag = PropretyTag.Forever;
	}
}
