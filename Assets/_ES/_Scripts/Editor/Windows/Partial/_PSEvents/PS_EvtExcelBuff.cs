using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : 绘制 buff表的事件
/// 作者 : Canyon
/// 日期 : 2017-01-21 15:10
/// 功能 : 
/// </summary>
public class PS_EvtExcelBuff {
	
	// 事件
	EMT_HitEvent m_evtHit = new EMT_HitEvent();

	string m_sJson = "";
	string m_sJsonTemp = "";

	public void DoReInit(string json){
		m_evtHit.DoReInit (json,0);
		m_sJson = json;
	}

	public string ToJsonString(){
		m_sJsonTemp = m_evtHit.ToJsonString ();
		if (string.IsNullOrEmpty (m_sJsonTemp)) {
			m_sJson = "null";
		} else {
			m_sJson = m_sJsonTemp;
		}
		return m_sJson;
	}

	public void DoClear(){
		m_evtHit.DoClear ();
		m_sJson = "";
		m_sJsonTemp = "";
	}

	public void DoDraw(){
		EG_GUIHelper.FEG_BeginVArea();
		{
			EditorGUILayout.LabelField(ToJsonString(), EditorStyles.textArea);
			EG_GUIHelper.FG_Space(5);

			_DrawAttrs ();
			EG_GUIHelper.FG_Space(5);

			_DrawBuffs ();
			// EG_GUIHelper.FG_Space(5);
		}
		EG_GUIHelper.FEG_EndV();
		EG_GUIHelper.FG_Space(5);


	}

	// 绘制修改属性
	PS_EvtAttrs psEvtAttrs;
	void _DrawAttrs(){
		if (psEvtAttrs == null) {
			psEvtAttrs = new PS_EvtAttrs ("属性修改列表:", false, delegate {
				m_evtHit.NewEvent<EDT_Property> ();
			}, delegate (EDT_Property one) {
				m_evtHit.RmEvent (one);
			}, false);
		}

		psEvtAttrs.DoDraw (0, m_evtHit.GetLAttrs());
	}

	// 绘制 buff
	PS_EvtBuff psEvtBuff;
	void _DrawBuffs(){
		if (psEvtBuff == null) {
			psEvtBuff = new PS_EvtBuff ("Buff列表:", false, delegate {
				m_evtHit.NewEvent<EDT_Buff> ();
			}, delegate (EDT_Buff one) {
				m_evtHit.RmEvent (one);
			}, false);
		}

		psEvtBuff.DoDraw (0, m_evtHit.GetLBuffs());
	}
}
