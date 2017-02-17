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

	// 技能效果类型  SkillEffectType
	public enum PlusType
	{
		// 按数值增减
		[Description("无")]
		None = 0,
	
		[Description("数值增 - 当前生命值")]
		HPPlus = 1,

		[Description("数值减 - 当前生命值")]
		HPMinus = 2,

		[Description("数值增 - 最大生命值")]
		HpMaxPlus = 3,

		[Description("数值减 - 最大生命值")]
		HpMaxMinus = 4,

		// 按百分比增减
		[Description("百分比增[以万为基数] - 当前生命值")]
		Hp_Per_Plus	= 1001,

		[Description("百分比减[以万为基数] - 当前生命值")]
		Hp_Per_Minus = 1002,

		[Description("百分比增[以万为基数] - 最大生命值")]
		HpMax_Per_Plus	= 1003,

		[Description("百分比增[以万为基数] - 最大生命值")]
		HpMax_Per_Minus	= 1004,
	}

	// 属性分类
	PlusType _m_iGID = PlusType.HPPlus;
	public PlusType m_iGID{
		get{ return _m_iGID;}
		set{
			_m_iGID = value;
			if (_m_iGID == PlusType.None)
				_m_iGID = PlusType.HPPlus;
		}
	}

	public string m_sPars = "";

	public EDT_Property():base(){
		m_emTag = PropretyTag.Forever;
	}

	public override void OnReInit (float castTime, JsonData jsonData)
	{
		base.OnReInit (castTime, jsonData);

		this.m_iGID = (PlusType)(int)jsonData ["m_id"];
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
		ret ["m_id"] = (int)this.m_iGID;
		ret ["m_parmStr"] = this.m_sPars;
		return ret;
	}

	public override void OnClear ()
	{
		base.OnClear ();

		m_emTag = PropretyTag.Forever;
	}
}
