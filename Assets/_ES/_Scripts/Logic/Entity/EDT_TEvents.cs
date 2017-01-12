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

	public string m_sJson = "";
	JsonData m_jsonData;

	// 总和对象
	List<EDT_Base> m_lEvents = new List<EDT_Base>();
	EDT_Base m_tmpEvent;
	int lens = 0;

	// 特效事件
	List<EDT_Effect> m_lEffects = new List<EDT_Effect>();

	public T NewEvent<T>() where T : EDT_Base,new()
	{
		T ret = new T();
		m_lEvents.Add(ret);
		return ret;
	}

	public void RmEvent(EDT_Base en)
	{
		if (en == null)
			return;
		en.DoRemove();
	}

	public List<T> GetList<T>() where T : EDT_Base
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

	public List<EDT_Effect> GetListEffects(){
		m_lEffects.Clear ();
		lens = m_lEvents.Count;
		for (int i = 0; i < lens; i++)
		{
			m_tmpEvent = m_lEvents[i];
			if(m_tmpEvent is EDT_Effect)
			{
				m_lEffects.Add((EDT_Effect)m_tmpEvent);
			}
		}
		return m_lEffects;
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

		m_sJson = "";
		m_jsonData = null;
	}

	public void DoReInit(string json){
		DoClear();
		DoInit (json);
	}

	public void DoInit(string json){
		this.m_sJson = json;
		m_jsonData = JsonMapper.ToObject (json);
		Debug.Log (m_jsonData [0]);
	}
}
