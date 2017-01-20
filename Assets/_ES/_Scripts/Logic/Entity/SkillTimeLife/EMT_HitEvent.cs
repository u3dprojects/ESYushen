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

	public override int OninitOne (LitJson.JsonData one, float castTime = 0)
	{
		int typeInt = base.OninitOne (one, castTime);
		switch (typeInt) {
		case 7:
		case 8:
			ToChangeAttrs (one,typeInt);
			break;
		}
		return typeInt;
	}

	void ToChangeAttrs(JsonData jsonData,int tpInt){
		EDT_Property status = EDT_Property.NewEntity<EDT_Property>(jsonData);
		if (status != null) {
			status.m_emTag = (EDT_Property.PropretyTag)(tpInt - 7);
			m_lEvents.Add (status);
		}
	}

	public List<EDT_Property> GetLAttrs(){
		GetList<EDT_Property> (ref m_lAttrs);
		return m_lAttrs;
	}

	public override void DoClear ()
	{
		base.DoClear ();
		m_lAttrs.Clear ();
	}

	public JsonData ToJsonData(){
		return ToArrayJsonData(m_lEvents);
	}
}
