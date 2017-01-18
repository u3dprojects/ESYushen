using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

/// <summary>
/// 类名 : Editor 的时间轴上的所有Event对象管理脚本
/// 作者 : Canyon
/// 日期 : 2017-01-12 09:36
/// 功能 : 该对象是个集合
/// </summary>
public class EDT_TEvents  {

	// 属于谁(谁放的，谁造成的)
	public Transform m_trsfOwner;

	public string m_sJson = "";
	JsonData m_jsonData;

	// 总和对象
	List<EDT_Base> m_lEvents = new List<EDT_Base>();
	EDT_Base m_tmpEvent;
	int lens = 0;

	// 特效事件
	List<EDT_Effect> m_lEffects = new List<EDT_Effect>();

	// 伤害事件
	List<EDT_Hurt> m_lHurts = new List<EDT_Hurt>();

	// 技能主动音效
	List<EDT_Audio> m_lAudios = new List<EDT_Audio>();

	public T NewEvent<T>() where T : EDT_Base,new()
	{
		T ret = new T();
		m_lEvents.Add(ret);
		return ret;
	}

	public T NewEvent<T>(float castTime,JsonData jsonData) where T : EDT_Base,new()
	{
		T ret = new T();
		ret.DoReInit (castTime, jsonData);
		if (ret.m_isJsonDataToSelfSuccessed) {
			m_lEvents.Add (ret);
			return ret;
		} else {
			ret.DoClear ();
		}
		return null;
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

	public void GetList<T>(ref List<T> ret) where T : EDT_Base
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

	public void DoStart()
	{
		lens = m_lEvents.Count;
		for (int i = 0; i < lens; i++)
		{
			m_tmpEvent = m_lEvents[i];
			m_tmpEvent.DoStart(true);
		}
		m_tmpEvent = null;
	}

	public void OnUpdate(float deltatime)
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

	public void OnSceneGUI()
	{
		lens = m_lEvents.Count;
		if (lens <= 0)
			return;


		for (int i = 0; i < lens; i++)
		{
			m_tmpEvent = m_lEvents[i];
			m_tmpEvent.m_trsfOwner = this.m_trsfOwner;
			m_tmpEvent.DoSceneGUI();
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

	public void DoClear()
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

		m_sJson = "";
		m_jsonData = null;
	}

	public void DoReInit(string json){
		DoClear();
		DoInit (json);
	}

	// 数据解析
	public void DoInit(string json){
		this.m_sJson = json;
		m_jsonData = JsonMapper.ToObject (json);
		if(!m_jsonData.IsArray){
			return;
		}

		int lens = m_jsonData.Count;
		JsonData tmpJsonData = null;
		float casttime = 0.0f;
		int typeInt = -1;

		for (int i = 0; i < lens; i++) {
			tmpJsonData = m_jsonData [i];

			if (!tmpJsonData.IsObject)
				continue;

			casttime = float.Parse(tmpJsonData ["m_timing"].ToString());
			tmpJsonData = tmpJsonData ["m_castEvts"];
			if (!tmpJsonData.IsArray) 
				continue;

			for (int j = 0; j < tmpJsonData.Count; j++) {
				typeInt = (int)((tmpJsonData [j]) ["m_typeInt"]);
				switch (typeInt) {
				case 1:
					ToEffect(casttime, tmpJsonData [j]);
					break;
				case 2:
					ToAudio(casttime, tmpJsonData [j]);
					break;
				case 6:
					ToHurt(casttime, tmpJsonData [j]);
					break;
				}
			}
		}
	}

	// 转为特效事件
	void ToEffect(float time,JsonData data){
		NewEvent<EDT_Effect> (time,data);
	}

	// 转为打击点事件
	void ToHurt(float time,JsonData data){
		NewEvent<EDT_Hurt> (time,data);
	}

	// 转为技能声音
	void ToAudio(float time,JsonData data){
		NewEvent<EDT_Audio> (time,data);
	}

	public string ToStrJsonData(){
		lens = m_lEvents.Count;
		if (lens <= 0) {
			return "";
		}

		Dictionary<float,List<EDT_Base>> tmpDic = new Dictionary<float, List<EDT_Base>> ();
		List<EDT_Base> tmpList = null;
		for (int i = 0; i < lens; i++)
		{
			m_tmpEvent = m_lEvents[i];
			if (!m_tmpEvent.m_isInitedFab)
				continue;
			
			if (tmpDic.ContainsKey (m_tmpEvent.m_fCastTime)) {
				tmpList = tmpDic [m_tmpEvent.m_fCastTime];
			} else {
				tmpList = new List<EDT_Base> ();
				tmpDic.Add (m_tmpEvent.m_fCastTime, tmpList);
			}

			tmpList.Add (m_tmpEvent);
		}

		if (tmpDic.Count <= 0) {
			return "";
		}

		JsonData tmpData = new JsonData ();
		tmpData.SetJsonType (JsonType.Array);

		JsonData tmpData2, tmpData3,tmpData4;

		foreach (KeyValuePair<float,List<EDT_Base>> item in tmpDic) {
			tmpData2 = new JsonData ();
			tmpData2 ["m_timing"] = EDT_Base.Round2D(item.Key,2);

			tmpData3 = new JsonData ();
			tmpData3.SetJsonType (JsonType.Array);
			foreach (EDT_Base one in item.Value) {
				tmpData4 = one.ToJsonData ();
				if (tmpData4 == null)
					continue;
				tmpData3.Add (tmpData4);
			}
			tmpData2 ["m_castEvts"] = tmpData3;

			tmpData.Add (tmpData2);
		}

		return JsonMapper.ToJson (tmpData);
	}
}
