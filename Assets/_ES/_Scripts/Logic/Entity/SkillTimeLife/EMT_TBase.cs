using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

/// <summary>
/// 类名 : Event 事件管理 基础类 脚本
/// 作者 : Canyon
/// 日期 : 2017-01-20 14:14
/// 功能 : 管理事件
/// </summary>
public class EMT_TBases {
	
	// 总和对象
	protected List<EDT_Base> m_lEvents = new List<EDT_Base>();
	protected EDT_Base m_tmpEvent;
	protected int lens = 0;

	// 数据转换(ToJsonString)时候用
	Dictionary<float,List<EDT_Base>> dicTimeEvents = new Dictionary<float, List<EDT_Base>> ();
	List<float> lstTimeKeys = new List<float> ();

	// 特效事件
	List<EDT_Effect> m_lEffects = new List<EDT_Effect>();

	// 伤害事件
	List<EDT_Hurt> m_lHurts = new List<EDT_Hurt>();

	// 音效事件
	List<EDT_Audio> m_lAudios = new List<EDT_Audio>();

	// 震屏事件
	List<EDT_Shake> m_lShakes = new List<EDT_Shake>();

	// buff 事件
	// List<EDT_Buff> m_lBuffs = new List<EDT_Buff>();

	public T NewEvent<T>() where T : EDT_Base,new()
	{
		T ret = EDT_Base.NewEntity<T>();
		m_lEvents.Add(ret);
		return ret;
	}

	public T NewEvent<T>(float castTime,JsonData jsonData) where T : EDT_Base,new()
	{
		T ret = EDT_Base.NewEntity<T>(jsonData, castTime);
		if (ret != null) {
			m_lEvents.Add (ret);
		}
		return ret;
	}

	public void RmEvent(EDT_Base en)
	{
		if (en == null)
			return;
		en.DoClear ();

		m_lEvents.Remove (en);
	}

	protected List<T> GetList<T>() where T : EDT_Base
	{
		List<T> ret = new List<T>();
		lens = m_lEvents.Count;
		for (int i = 0; i < lens; i++)
		{
			m_tmpEvent = m_lEvents[i];
			if(m_tmpEvent is T)
			{
				ret.Add((T)m_tmpEvent);
			}
		}
		return ret;
	}

	protected void GetList<T>(ref List<T> ret) where T : EDT_Base
	{
		if (ret == null) {
			ret = new List<T> ();
		} else {
			ret.Clear ();
		}

		lens = m_lEvents.Count;
		for (int i = 0; i < lens; i++)
		{
			m_tmpEvent = m_lEvents[i];
			if(m_tmpEvent is T)
			{
				ret.Add((T)m_tmpEvent);
			}
		}
	}

	#region === 周期函数 init - start - update - end 等 === 

	public void DoReInit(JsonData jsonData,float castTime = 0){
		DoClear ();
		DoInit (jsonData,castTime);
	}

	public void DoInit (JsonData jsonData,float castTime = 0)
	{
		if (jsonData == null || !jsonData.IsArray)
			return;
		for (int j = 0; j < jsonData.Count; j++) {
			OninitOne(jsonData[j],castTime);
		}
	}

	public virtual int OninitOne(JsonData one,float castTime = 0){
		int typeInt = (int)((one) ["m_typeInt"]);
		switch (typeInt) {
		case 1:
			ToEffect(castTime, one);
			break;
		case 2:
			ToAudio(castTime, one);
			break;
		case 3:
			ToShake(castTime, one);
			break;
		case 6:
			ToHurt(castTime, one);
			break;
		}
		return typeInt;
	}

	public virtual void DoStart()
	{
		lens = m_lEvents.Count;
		for (int i = 0; i < lens; i++)
		{
			m_tmpEvent = m_lEvents[i];
			m_tmpEvent.DoStart(true);
		}
		m_tmpEvent = null;
	}

	public void DoUpdate(float deltatime)
	{
		lens = m_lEvents.Count;
		if (lens <= 0)
			return;


		for (int i = 0; i < lens; i++)
		{
			m_tmpEvent = m_lEvents[i];
			m_tmpEvent.DoUpdate(deltatime);
			if (m_tmpEvent.m_isEnd)
			{
				m_tmpEvent.DoEnd();
			}
		}
	}

	public virtual void OnSceneGUI(Transform trsfOrg)
	{
		lens = m_lEvents.Count;
		if (lens <= 0)
			return;
		
		for (int i = 0; i < lens; i++)
		{
			m_tmpEvent = m_lEvents[i];
			m_tmpEvent.DoSceneGUI(trsfOrg);
		}
	}

	public void DoEnd()
	{
		lens = m_lEvents.Count;
		for (int i = 0; i < lens; i++)
		{
			m_tmpEvent = m_lEvents[i];
			m_tmpEvent.DoEnd();
		}
		m_tmpEvent = null;
	}

	public virtual void DoClear()
	{
		lens = m_lEvents.Count;
		for (int i = 0; i < lens; i++)
		{
			m_tmpEvent = m_lEvents[i];
			m_tmpEvent.DoClear ();
		}

		m_lEvents.Clear();

		m_tmpEvent = null;

		m_lEffects.Clear ();
		m_lHurts.Clear ();
		m_lAudios.Clear ();
		m_lShakes.Clear ();
	}

	#endregion

	#region === 转换 Func === 

	/// <summary>
	/// 默认转为主动触发事件，有时间轴
	/// </summary>
	/// <returns>The string json data.</returns>
	public virtual string ToJsonString(){
		lens = m_lEvents.Count;
		if (lens <= 0) {
			return "";
		}

		dicTimeEvents.Clear ();
		lstTimeKeys.Clear ();

		List<EDT_Base> tmpList = null;
		for (int i = 0; i < lens; i++)
		{
			m_tmpEvent = m_lEvents[i];
			if (!m_tmpEvent.m_isInitedFab)
				continue;

			if (dicTimeEvents.ContainsKey (m_tmpEvent.m_fCastTime)) {
				tmpList = dicTimeEvents [m_tmpEvent.m_fCastTime];
			} else {
				tmpList = new List<EDT_Base> ();
				dicTimeEvents.Add (m_tmpEvent.m_fCastTime, tmpList);
				lstTimeKeys.Add (m_tmpEvent.m_fCastTime);
			}

			tmpList.Add (m_tmpEvent);
		}

		if (dicTimeEvents.Count <= 0) {
			return "";
		}

		JsonData tmpData = new JsonData ();
		tmpData.SetJsonType (JsonType.Array);
		JsonData tmpData2;

		lstTimeKeys.Sort ((x,y) => {
			return x < y ? -1 : 1;
		});

		foreach (var key in lstTimeKeys) {
			tmpList = dicTimeEvents[key];
			tmpData2 = new JsonData ();
			tmpData2 ["m_timing"] = EDT_Base.Round2D(key,2);
			tmpData2 ["m_castEvts"] = ToArrayJsonData (tmpList);
			tmpData.Add (tmpData2);
		}

//		foreach (KeyValuePair<float,List<EDT_Base>> item in tmpDic) {
//			tmpData2 = new JsonData ();
//			tmpData2 ["m_timing"] = EDT_Base.Round2D(item.Key,2);
//			tmpData2 ["m_castEvts"] = ToArrayJsonData (item.Value);
//			tmpData.Add (tmpData2);
//		}

		return JsonMapper.ToJson (tmpData);
	}

	public JsonData ToArrayJsonData(List<EDT_Base> listOrg){
		if (listOrg == null || listOrg.Count <= 0) {
			return null;
		}

		JsonData ret = new JsonData ();
		ret.SetJsonType (JsonType.Array);

		lens = listOrg.Count;
		JsonData tmp = null;
		for (int i = 0; i < lens; i++)
		{
			m_tmpEvent = listOrg[i];
			tmp = m_tmpEvent.ToJsonData();
			if (tmp != null) {
				ret.Add (tmp);
			}
		}
		m_tmpEvent = null;
		return ret;
	}

	// 转为特效事件
	protected void ToEffect(float time,JsonData data){
		NewEvent<EDT_Effect> (time,data);
	}

	// 转为打击点事件
	protected void ToHurt(float time,JsonData data){
		NewEvent<EDT_Hurt> (time,data);
	}

	// 转为声音事件
	protected void ToAudio(float time,JsonData data){
		NewEvent<EDT_Audio> (time,data);
	}

	// 转为震屏事件
	protected void ToShake(float time,JsonData data){
		NewEvent<EDT_Shake> (time,data);
	}

	// 转为 Buff事件
	protected void ToBuff(float time,JsonData data){
		NewEvent<EDT_Buff>(time,data);
	}

	public List<EDT_Effect> GetLEffects(){
		GetList<EDT_Effect> (ref m_lEffects);
		return m_lEffects;
	}

	public List<EDT_Hurt> GetLHurts(){
		GetList<EDT_Hurt> (ref m_lHurts);
		return m_lHurts;
	}

	public List<EDT_Audio> GetLAudios(){
		GetList<EDT_Audio> (ref m_lAudios);
		return m_lAudios;
	}

	public List<EDT_Shake> GetLShakes(){
		GetList<EDT_Shake> (ref m_lShakes);
		return m_lShakes;
	}

//	public List<EDT_Buff> GetLBuffs(){
//		GetList<EDT_Buff> (ref m_lBuffs);
//		return m_lBuffs;
//	}

	#endregion
}
