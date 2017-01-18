using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

/// <summary>
/// 类名 : 命中事件
/// 作者 : Canyon
/// 日期 : 2017-01-18 09:20
/// 功能 : 受击者(被技能伤害的人),受到的状态(伤害，音效等)
/// </summary>
public class EDT_Hurt_BeHitter {

	// 是否可以显示
	public bool m_isCanShow = false;

	// 伤害状态
	List<EDT_Hurt_BeHitStatus> _m_lHurtStatus =  new List<EDT_Hurt_BeHitStatus>();
	List<EDT_Hurt_BeHitStatus> _m_lCurHurtStatus =  new List<EDT_Hurt_BeHitStatus>();

	// 伤害音效
	public EDT_Audio m_eAuido = new EDT_Audio ();

	protected bool m_isInitedByJson = false;

	public void DoInit(string json){
		JsonData org = JsonMapper.ToObject (json);
		DoInit (org);
	}

	public void DoInit(JsonData org){
		DoClear();
		OnInit (org);
	}

	public void OnInit(JsonData jsonData){
		if (jsonData == null || !jsonData.IsArray)
			return;
		int lens = jsonData.Count;
		JsonData tmp = null;
		int tpInt = 0;
		for (int i = 0; i < lens; i++) {
			tmp = jsonData [i];
			tpInt = (int)tmp ["m_typeInt"];
			if (tpInt == 7) {
				ToStatus (tmp);
			} else if (tpInt == 2) {
				ToAudio (tmp);
			}
		}
		this.m_isInitedByJson = true;
	}

	void ToAudio(JsonData jsonData){
		m_eAuido.DoReInit (0, jsonData);
	}

	void ToStatus(JsonData jsonData){
		EDT_Hurt_BeHitStatus status = EDT_Hurt_BeHitStatus.NewHurtStatus (jsonData);
		if (status != null) {
			_m_lHurtStatus.Add (status);
		}
	}

	public List<EDT_Hurt_BeHitStatus> GetListStatus(){
		this._m_lCurHurtStatus.Clear ();
		this._m_lCurHurtStatus.AddRange (this._m_lHurtStatus);
		return this._m_lCurHurtStatus;
	}

	public void NewStatus(){
		EDT_Hurt_BeHitStatus ret = new EDT_Hurt_BeHitStatus ();
		this._m_lHurtStatus.Add (ret);
	}

	public void RemoveStatus(EDT_Hurt_BeHitStatus rm){
		this._m_lHurtStatus.Remove (rm);
	}

	public void DoStart(){
		m_eAuido.DoStart ();
	}

	public void DoClear(){
		this.m_isInitedByJson = false;
		this.m_eAuido.DoClear ();
		this._m_lHurtStatus.Clear ();
		this._m_lCurHurtStatus.Clear ();
	}

	public JsonData ToJsonData(){
		JsonData ret = new JsonData ();
		ret.SetJsonType (JsonType.Array);

		JsonData tmp = null;

		tmp = m_eAuido.ToJsonData();
		if (tmp != null) {
			ret.Add (tmp);
		}

		int lens = this._m_lHurtStatus.Count;
		if (lens > 0) {
			for (int i = 0; i < lens; i++) {
				tmp = this._m_lHurtStatus [i].ToJsonData ();
				if (tmp != null) {
					ret.Add (tmp);
				}
			}
		}
		return ret;
	}

	static public EDT_Hurt_BeHitter NewHurtStatus(JsonData jsonData){
		EDT_Hurt_BeHitter ret = new EDT_Hurt_BeHitter ();
		ret.DoInit (jsonData);
		if (ret.m_isInitedByJson) {
			return ret;
		}else {
			ret.DoClear ();
		}
		return null;
	}
}

/// <summary>
/// 类名 : 受击者受到的状态(扣血等)
/// 作者 : Canyon
/// 日期 : 2017-01-18 09:20
/// 功能 : 
/// </summary>
public class EDT_Hurt_BeHitStatus{
	// 主分类
	public int m_fCurType = 7;

	// 分组
	public int m_iGid = 0;

	//  参数值
	public string m_sPars = "";

	protected bool m_isInitedByJson = false;

	public void DoInit(string json){
		JsonData org = JsonMapper.ToObject (json);
		DoInit (org);
	}

	public void DoInit(JsonData org){
		DoClear();
		OnInit (org);
	}

	public void OnInit(JsonData jsonData){
		if (jsonData == null)
			return;

		this.m_fCurType = (int)jsonData ["m_typeInt"];
		this.m_iGid = (int)jsonData ["m_id"];
		this.m_sPars = (string)jsonData ["m_parmStr"];
		this.m_isInitedByJson = true;
	}

	public void DoClear(){
		this.m_isInitedByJson = false;
	}

	public JsonData ToJsonData(){
		if (string.IsNullOrEmpty (this.m_sPars))
			return null;
		
		JsonData ret = new JsonData ();
		ret ["m_typeInt"] = this.m_fCurType;
		ret ["m_id"] = this.m_iGid;
		ret ["m_parmStr"] = this.m_sPars;
		return ret;
	}

	static public EDT_Hurt_BeHitStatus NewHurtStatus(JsonData jsonData){
		EDT_Hurt_BeHitStatus ret = new EDT_Hurt_BeHitStatus ();
		ret.DoInit (jsonData);
		if (ret.m_isInitedByJson) {
			return ret;
		}else {
			ret.DoClear ();
		}
		return null;
	}
}