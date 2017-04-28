using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

/// <summary>
/// 类名 : 伤害区域检查事件
/// 作者 : Canyon
/// 日期 : 2017-01-22 14:33
/// 功能 : 
/// </summary>
public class EMT_HitArea {
	
	// 伤害区域列表(m_zones)
	List<EDT_Hurt_Area> m_lHitAreas = new List<EDT_Hurt_Area>();
	List<EDT_Hurt_Area> m_lCurHitAreas = new List<EDT_Hurt_Area>();
	EDT_Hurt_Area m_eAreaTemp = null;
	int lens = 0;

	public bool IsHasArea{
		get{ 
			return this.m_lHitAreas.Count > 0;
		}
	}

	public void DoReInit(string json){
		DoClear();
		if (string.IsNullOrEmpty (json)) {
			return;
		}

		JsonData jsonData = JsonMapper.ToObject (json);
		DoReInit (jsonData, 0);
	}

	public void DoReInit(JsonData jsonData,float castTime){
		DoClear();
		if (!jsonData.IsArray) {
			return;
		}
		lens = jsonData.Count;
		for (int i = 0; i < lens; i++) {
			m_eAreaTemp = EDT_Hurt_Area.NewEntity<EDT_Hurt_Area> (jsonData [i], castTime);
			if (m_eAreaTemp == null)
				continue;
			m_lHitAreas.Add (m_eAreaTemp);
		}
		m_eAreaTemp = null;
	}

	public JsonData ToJsonData(){
		return EMT_TBases.ToArrayJsonData(m_lHitAreas);
	}

	public string ToJsonString(){
		return EMT_TBases.ToArrayJsonString(m_lHitAreas);
	}

	public void DoClear(){
		OnClearArea ();
		m_lHitAreas.Clear ();
		m_lCurHitAreas.Clear ();
	}

	public void NewArea(){
		m_lHitAreas.Add (new EDT_Hurt_Area ());
	}

	public void RmArea(EDT_Hurt_Area one){
		m_lHitAreas.Remove (one);
	}

	void OnClearArea(){
		List<EDT_Hurt_Area> list = GetLAreas ();
		lens = list.Count;
		for (int i = 0; i < lens; i++) {
			m_eAreaTemp = list [i];
			m_eAreaTemp.DoClear ();
		}
		m_eAreaTemp = null;
	}


	public void OnSceneGUI(Transform trsfOrg){
		List<EDT_Hurt_Area> list = GetLAreas ();
		lens = list.Count;
		for (int i = 0; i < lens; i++) {
			m_eAreaTemp = list [i];
			m_eAreaTemp.DoSceneGUI(trsfOrg);
		}
		m_eAreaTemp = null;
	}

	public void DoStart(){
		List<EDT_Hurt_Area> list = GetLAreas ();
		lens = list.Count;
		for (int i = 0; i < lens; i++) {
			m_eAreaTemp = list [i];
			m_eAreaTemp.DoStart (true);
		}
		m_eAreaTemp = null;
	}

	public void DoUpdate(float deltatime){
		List<EDT_Hurt_Area> list = GetLAreas ();
		lens = list.Count;
		for (int i = 0; i < lens; i++) {
			m_eAreaTemp = list [i];
			m_eAreaTemp.DoUpdate (deltatime);
		}
		m_eAreaTemp = null;
	}

	public List<EDT_Hurt_Area> GetLAreas(){
		m_lCurHitAreas.Clear ();
		if (m_lHitAreas != null && m_lHitAreas.Count > 0) {
			m_lCurHitAreas.AddRange (m_lHitAreas);
		}
		return m_lCurHitAreas;
	}
}
