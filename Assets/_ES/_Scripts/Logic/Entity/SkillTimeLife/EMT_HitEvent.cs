using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

/// <summary>
/// 类名 : 命中事件
/// 作者 : Canyon
/// 日期 : 2017-01-18 09:20
/// 功能 : 受击者(被技能伤害的人),受到的状态(伤害，音效等事件)
/// </summary>
public class EMT_HitEvent : EMT_TBases {

	// 是否可以显示
	public bool m_isCanShow = false;

	// 修改属性事件
	List<EDT_Property> m_lAttrs = new List<EDT_Property>();

	// buff 事件
	List<EDT_Buff> m_lBuffs = new List<EDT_Buff>();

	public override void DoClear ()
	{
		base.DoClear ();
		m_lAttrs.Clear ();
		m_lBuffs.Clear ();
	}

	public JsonData ToJsonData(){
		return ToArrayJsonData(m_lEvents);
	}

	public override int OninitOne (LitJson.JsonData one, float castTime = 0)
	{
		int typeInt = base.OninitOne (one, castTime);
		switch (typeInt) {
		case 7:
		case 8:
			ToChangeAttrs (one,castTime,typeInt);
			break;
		case 9:
			ToBuff (one,castTime);
			break;
		}
		return typeInt;
	}

	public override string ToJsonString ()
	{
		return ToArrayJsonString (m_lEvents);
	}

	void ToChangeAttrs(JsonData jsonData,float castTime,int tpInt){
		EDT_Property status = NewEvent<EDT_Property>(castTime,jsonData);
		if (status != null) {
			status.m_emTag = (EDT_Property.PropretyTag)(tpInt - 7);
			m_lEvents.Add (status);
		}
	}

	void ToBuff(JsonData jsonData,float castTime){
		NewEvent<EDT_Buff>(castTime,jsonData);
	}

	public List<EDT_Property> GetLAttrs(){
		GetList<EDT_Property> (ref m_lAttrs);
		return m_lAttrs;
	}

	public List<EDT_Buff> GetLBuffs(){
		GetList<EDT_Buff> (ref m_lBuffs);
		return m_lBuffs;
	}
}
