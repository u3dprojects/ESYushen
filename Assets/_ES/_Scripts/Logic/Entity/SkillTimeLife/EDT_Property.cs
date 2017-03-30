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
		HP_Plus = 1,
		[Description("数值减 - 当前生命值")]
		HP_Minus = 2,
		[Description("数值增 - 最大生命值")]
		HP_Max_Plus = 3,
		[Description("数值减 - 最大生命值")]
		HP_Max_Minus = 4,

		[Description("数值增 - 攻击")]
		ATK_Plus = 5,
		[Description("数值减 - 攻击")]
		ATK_Minus = 6,

		[Description("数值增 - 防御")]
		DEF_Plus = 7,
		[Description("数值减 - 防御")]
		DEF_Minus = 8,

		[Description("数值增 - 穿刺")]
		IMPALE_Plus = 9,
		[Description("数值减 - 穿刺")]
		IMPALE_Minus = 10,

		[Description("数值增 - 暴击")]
		CRI_Plus = 11,
		[Description("数值减 - 暴击")]
		CRI_Minus = 12,

		[Description("数值增 - 韧性")]
		TOUGHNESS_Plus = 13,
		[Description("数值减 - 韧性")]
		TOUGHNESS_Minus = 14,

		[Description("数值增 - 暴击伤害")]
		CRIDMA_Plus = 15,
		[Description("数值减 - 暴击伤害")]
		CRIDMA_Minus = 16,

		[Description("数值增 - 暴击防御")]
		CRIDEF_Plus = 17,
		[Description("数值减 - 暴击防御")]
		CRIDEF_Minus = 18,

		[Description("数值增 - 闪避")]
		DODGE_Plus = 19,
		[Description("数值减 - 闪避")]
		DODGE_Minus = 20,

		[Description("数值增 - 命中")]
		HIT_Plus = 21,
		[Description("数值减 - 闪避")]
		HIT_Minus = 22,

		[Description("数值增 - 打击回血")]
		HIT_RECOVERY_Plus = 23,
		[Description("数值减 - 打击回血")]
		HIT_RECOVERY_Minus = 24,

		[Description("数值增 - 生命恢复")]
		HPRECOVERY_Plus = 25,
		[Description("数值减 - 生命恢复")]
		HPRECOVERY_Minus = 26,

		[Description("数值增 - 冰系之力")]
		ICE_Plus = 27,
		[Description("数值减 - 冰系之力")]
		ICE_Minus = 28,
		[Description("数值增 - 火系之力")]
		FIRE_Plus = 29,
		[Description("数值减 - 火系之力")]
		FIRE_Minus = 30,
		[Description("数值增 - 雷系之力")]
		THOUNDER_Plus = 31,
		[Description("数值减 - 雷系之力")]
		THOUNDER_Minus = 32,
		[Description("数值增 - 毒系之力")]
		POISION_Plus = 33,
		[Description("数值减 - 毒系之力")]
		POISION_Minus = 34,
		[Description("数值增 - 暗系之力")]
		DARK_Plus = 35,
		[Description("数值减 - 暗系之力")]
		DARK_Minus = 36,
		[Description("数值增 - 光系之力")]
		LIGHT_Plus = 37,
		[Description("数值减 - 光系之力")]
		LIGHT_Minus = 38,

		[Description("数值增 - 召唤能力值")]
		POWER_Plus = 39,
		[Description("数值减 - 召唤能力值")]
		POWER_Minus = 40,
		[Description("数值增 - 打击回能")]
		HIT_REPOWER_Plus = 41,
		[Description("数值减 - 打击回能")]
		HIT_REPOWER_Minus = 42,
		[Description("数值增 - 回能")]
		REPOWER_Plus = 43,
		[Description("数值减 - 回能")]
		REPOWER_Minus = 44,

		[Description("数值增 - 变身精力值")]
		ENERGY_Plus = 45,
		[Description("数值减 - 变身精力值")]
		ENERGY_Minus = 46,
		[Description("数值增 - 恢复精力")]
		REENERGY_Plus = 47,
		[Description("数值减 - 恢复精力")]
		REENERGY_Minus = 48,

		[Description("数值增 - 减免伤害")]
		REDUCE_DAM_Plus = 49,
		[Description("数值减 - 减免伤害")]
		REDUCE_DAM_Minus = 50,

		[Description("数值增 - 增加伤害")]
		ADD_DAM_Plus = 51,
		[Description("数值减 - 增加伤害")]
		ADD_DAM_Minus = 52,

		[Description("数值增 - 攻击速度")]
		ATK_SPEED_Plus = 53,
		[Description("数值减 - 攻击速度")]
		ATK_SPEED_Minus = 54,

		[Description("定身-不能移动(参数忽略)")]
		CantMove = 100,

		[Description("沉默-不能释放技能(参数忽略)")]
		CantSkill = 101,

		[Description("冰系伤害-瞬时效果")]
		icePowerDamage = 102,
		[Description("火系伤害-瞬时效果")]
		firePowerDamage = 103,
		[Description("雷系伤害-瞬时效果")]
		thunderPowerDamage = 104,
		[Description("毒系伤害-瞬时效果")]
		poisionPowerDamage = 105,
		[Description("暗系伤害-瞬时效果")]
		darkPowerDamage = 106,
		[Description("光系伤害-瞬时效果")]
		lightPowerDamage = 107,

		[Description("百分比增[以万为基数] - 增加经验")]
		Per_Exp_Plus = 108,

		// 按百分比增减
		[Description("百分比增[以万为基数] - 当前生命值")]
		Per_Hp_Plus	= 1001,
		[Description("百分比减[以万为基数] - 当前生命值")]
		Per_Hp_Minus = 1002,
		[Description("百分比增[以万为基数] - 最大生命值")]
		Per_HpMax_Plus	= 1003,
		[Description("百分比增[以万为基数] - 最大生命值")]
		Per_HpMax_Minus	= 1004,

		[Description("百分比增[以万为基数] - 防御")]
		Per_DEF_Plus = 1007,
		[Description("百分比减[以万为基数] - 防御")]
		Per_DEF_Minus = 1008,

		[Description("百分比增[以万为基数] - 穿刺")]
		Per_IMPALE_Plus = 1009,
		[Description("百分比减[以万为基数] - 穿刺")]
		Per_IMPALE_Minus = 1010,

		[Description("百分比增[以万为基数] - 暴击")]
		Per_CRI_Plus = 1011,
		[Description("百分比减[以万为基数] - 暴击")]
		Per_CRI_Minus = 1012,

		[Description("百分比增[以万为基数] - 韧性")]
		Per_TOUGHNESS_Plus = 1013,
		[Description("百分比减[以万为基数] - 韧性")]
		Per_TOUGHNESS_Minus = 1014,

		[Description("百分比增[以万为基数] - 暴击伤害")]
		Per_CRIDMA_Plus = 1015,
		[Description("百分比减[以万为基数] - 暴击伤害")]
		Per_CRIDMA_Minus = 1016,

		[Description("百分比增[以万为基数] - 暴击防御")]
		Per_CRIDEF_Plus = 1017,
		[Description("百分比减[以万为基数] - 暴击防御")]
		Per_CRIDEF_Minus = 1018,

		[Description("百分比增[以万为基数] - 闪避")]
		Per_DODGE_Plus = 1019,
		[Description("百分比减[以万为基数] - 闪避")]
		Per_DODGE_Minus = 1020,

		[Description("百分比增[以万为基数] - 命中")]
		Per_HIT_Plus = 1021,
		[Description("百分比减[以万为基数] - 闪避")]
		Per_HIT_Minus = 1022,

		[Description("百分比增[以万为基数] - 打击回血")]
		Per_HIT_RECOVERY_Plus = 1023,
		[Description("百分比减[以万为基数] - 打击回血")]
		Per_HIT_RECOVERY_Minus = 1024,

		[Description("百分比增[以万为基数] - 生命恢复")]
		Per_HPRECOVERY_Plus = 1025,
		[Description("百分比减[以万为基数] - 生命恢复")]
		Per_HPRECOVERY_Minus = 1026,

		[Description("百分比增[以万为基数] - 冰系之力")]
		Per_ICE_Plus = 1027,
		[Description("百分比减[以万为基数] - 冰系之力")]
		Per_ICE_Minus = 1028,
		[Description("百分比增[以万为基数] - 火系之力")]
		Per_FIRE_Plus = 1029,
		[Description("百分比减[以万为基数] - 火系之力")]
		Per_FIRE_Minus = 1030,
		[Description("百分比增[以万为基数] - 雷系之力")]
		Per_THOUNDER_Plus = 1031,
		[Description("百分比减[以万为基数] - 雷系之力")]
		Per_THOUNDER_Minus = 1032,
		[Description("百分比增[以万为基数] - 毒系之力")]
		Per_POISION_Plus = 1033,
		[Description("百分比减[以万为基数] - 毒系之力")]
		Per_POISION_Minus = 1034,
		[Description("百分比增[以万为基数] - 暗系之力")]
		Per_DARK_Plus = 1035,
		[Description("百分比减[以万为基数] - 暗系之力")]
		Per_DARK_Minus = 1036,
		[Description("百分比增[以万为基数] - 光系之力")]
		Per_LIGHT_Plus = 1037,
		[Description("百分比减[以万为基数] - 光系之力")]
		Per_LIGHT_Minus = 1038,

		[Description("百分比增[以万为基数] - 召唤能力值")]
		Per_POWER_Plus = 1039,
		[Description("百分比减[以万为基数] - 召唤能力值")]
		Per_POWER_Minus = 1040,
		[Description("百分比增[以万为基数] - 打击回能")]
		Per_HIT_REPOWER_Plus = 1041,
		[Description("百分比减[以万为基数] - 打击回能")]
		Per_HIT_REPOWER_Minus = 1042,
		[Description("百分比增[以万为基数] - 回能")]
		Per_REPOWER_Plus = 1043,
		[Description("百分比减[以万为基数] - 回能")]
		Per_REPOWER_Minus = 1044,

		[Description("百分比增[以万为基数] - 变身精力值")]
		Per_ENERGY_Plus = 1045,
		[Description("百分比减[以万为基数] - 变身精力值")]
		Per_ENERGY_Minus = 1046,
		[Description("百分比增[以万为基数] - 恢复精力")]
		Per_REENERGY_Plus = 1047,
		[Description("百分比减[以万为基数] - 恢复精力")]
		Per_REENERGY_Minus = 1048,

		[Description("百分比增[以万为基数] - 减免伤害")]
		Per_REDUCE_DAM_Plus = 1049,
		[Description("百分比减[以万为基数] - 减免伤害")]
		Per_REDUCE_DAM_Minus = 1050,

		[Description("百分比增[以万为基数] - 增加伤害")]
		Per_ADD_DAM_Plus = 1051,
		[Description("百分比减[以万为基数] - 增加伤害")]
		Per_ADD_DAM_Minus = 1052,

		[Description("百分比增[以万为基数] - 攻击速度")]
		Per_ATK_SPEED_Plus = 1053,
		[Description("百分比减[以万为基数] - 攻击速度")]
		Per_ATK_SPEED_Minus = 1054,
	}

	// 属性分类
	PlusType _m_iGID = PlusType.HP_Plus;
	public PlusType m_iGID{
		get{ return _m_iGID;}
		set{
			_m_iGID = value;
			if (_m_iGID == PlusType.None)
				_m_iGID = PlusType.HP_Plus;
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
