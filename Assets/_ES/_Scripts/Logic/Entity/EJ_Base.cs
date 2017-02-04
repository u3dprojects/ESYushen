using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;

/// <summary>
/// 类名 : Json Data 的基础类
/// 作者 : Canyon
/// 日期 : 2017-02-04 09:36
/// 功能 : 主要是处理Json <---> string 之间的转换
/// </summary>
public class EJ_Base {

	public EJ_Base(){}

	protected JsonData m_jdOrg;

	protected bool m_isSuccessJD2Self = false;

	public void DoReInit(string json){
		JsonData jsonData = JsonMapper.ToObject (json);
		DoReInit (jsonData);
	}

	public void DoReInit(JsonData jsonData){
		DoClear ();
		DoInit (jsonData);
	}

	public void DoInit(JsonData jsonData){
		m_jdOrg = jsonData;
		m_isSuccessJD2Self = OnInit ();
	}

	protected virtual bool OnInit(){
		return true;
	}

	public virtual JsonData ToJsonData(){
		return m_jdOrg;
	}

	public virtual string ToJsonString(){
		return "";
	}

	public void DoClear(){
		m_jdOrg = null;
		m_isSuccessJD2Self = false;

		OnClear ();
	}

	protected virtual void OnClear(){
	}

	#region == 静态方法 ==

	static public T NewEntity<T>() where T : EJ_Base,new(){
		return new T();
	}

	static public T NewEntity<T>(JsonData jsonData) where T : EJ_Base,new()
	{
		T one = new T();
		one.DoReInit (jsonData);
		if (one.m_isSuccessJD2Self) {
			return one;
		} else {
			one.DoClear ();
		}
		return null;
	}

	static public List<T> GetList<T>(List<T> listOrg)
	{
		List<T> ret = new List<T>();
		if (listOrg == null)
			return ret;
		
		int lens = listOrg.Count;
		if(lens <= 0)
			return ret;

		object tmp;
		for (int i = 0; i < lens; i++)
		{
			tmp = listOrg[i];
			if(tmp is T)
			{
				ret.Add((T)tmp);
			}
		}
		return ret;
	}

	static public List<T> GetList<T>(IList listOrg)
	{
		List<T> ret = new List<T>();
		if (listOrg == null)
			return ret;

		int lens = listOrg.Count;
		if(lens <= 0)
			return ret;

		object tmp;
		for (int i = 0; i < lens; i++)
		{
			tmp = listOrg[i];
			if(tmp is T)
			{
				ret.Add((T)tmp);
			}
		}
		return ret;
	}

	static public void GetList<T>(List<T> listOrg,ref List<T> ret)
	{
		if (ret == null) {
			ret = new List<T> ();
		} else {
			ret.Clear ();
		}

		if (listOrg == null)
			return;

		int lens = listOrg.Count;
		if(lens <= 0)
			return;

		object tmp;
		for (int i = 0; i < lens; i++)
		{
			tmp = listOrg[i];
			if(tmp is T)
			{
				ret.Add((T)tmp);
			}
		}
	}

	static public void GetList<T>(IList listOrg,ref List<T> ret)
	{
		if (ret == null) {
			ret = new List<T> ();
		} else {
			ret.Clear ();
		}

		if (listOrg == null)
			return;

		int lens = listOrg.Count;
		if(lens <= 0)
			return;

		object tmp;
		for (int i = 0; i < lens; i++)
		{
			tmp = listOrg[i];
			if(tmp is T)
			{
				ret.Add((T)tmp);
			}
		}
	}

	// 保留小数
	static public double Round2D(float org, int acc)
	{
		double pow = Mathf.Pow(10, acc);
		double temp = org * pow;
		return Mathf.RoundToInt((float)temp) / pow;
	}

	#endregion
}
