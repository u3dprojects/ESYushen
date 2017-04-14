using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// 类名 : buff 在 windows的视图
/// 作者 : Canyon
/// 日期 : 2017-01-19 16:10
/// 功能 : 
/// </summary>
public class EG_Buff {

	EN_Buff ms_entity = new EN_Buff ();

	GameObject ms_gobjEffect,ms_preGobjEffect;

	string[] JoinType = {
		"原点",
		"头部",
		"胸部",
		"腰部",
		"左手心",
		"右手心",
		"左武器攻击点",
		"右武器攻击点"
	};
	string[] TagType = {
		"增益",
		"减益"
	};
	string[] GIDType = {
		"占位"
	};
	bool ms_isRest;
	bool ms_isReplace;

	EN_OptBuff m_opt{
		get{
			return EN_OptBuff.Instance;
		}
	}

	// 事件
	EMT_Event evt_Inv = new EMT_Event();
	EMT_Event evt_One = new EMT_Event();
	EMT_Event evt_Dua = new EMT_Event();

	PS_EvtHurt m_psInv = null;
	PS_EvtHurt m_psOne = null;
	PS_EvtHurt m_psDua = null;

	List<bool> m_lFodeout = new List<bool>(){false,false,false};

	// 修改技能
	List<EDT_Property> listAttrChange = new List<EDT_Property>();

	public void DoInit(string path){
		m_opt.DoInit (path, 0);
	}

	public bool isInited{
		get{
			return m_opt.isInitSuccessed;
		}
	}

	public void DoClear(){
		m_opt.DoClear ();
		if(m_psInv != null)
			m_psInv.DoClear ();

		if(m_psOne != null)
			m_psOne.DoClear ();

		if(m_psDua != null)
			m_psDua.DoClear ();

		evt_Inv.DoClear ();
		evt_One.DoClear ();
		evt_Dua.DoClear ();

		m_lFodeout [0] = false;
		m_lFodeout [1] = false;
		m_lFodeout [2] = false;
		ms_gobjEffect = null;
		ms_preGobjEffect = null;

		listAttrChange.Clear ();
	}

	public void DrawShow()
	{
		EG_GUIHelper.FEG_HeadTitMid ("Buff Excel 表",Color.cyan);

		EG_GUIHelper.FEG_BeginH();
		ms_entity.ID = EditorGUILayout.IntField("ID:",ms_entity.ID);
		if (GUILayout.Button("查询"))
		{
			if (m_opt.isInitSuccessed)
			{
				EN_Buff ms_curEnity = m_opt.GetEntity(ms_entity.ID);
				OnInitEntity2Attrs(ms_curEnity);
			}
			else{
				EditorUtility.DisplayDialog("Tips","没有选则技能表，不能进行查询搜索!","Okey");
			}
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		ms_entity.Name = EditorGUILayout.TextField("名称:", ms_entity.Name);
		EG_GUIHelper.FG_Space(5);

		ms_entity.NameID = EditorGUILayout.IntField("名称ID:", ms_entity.NameID);
		EG_GUIHelper.FG_Space(5);

		ms_entity.Desc = EditorGUILayout.TextField("描述:", ms_entity.Desc);
		EG_GUIHelper.FG_Space(5);

		ms_entity.DescID = EditorGUILayout.IntField("描述ID:", ms_entity.DescID);
		EG_GUIHelper.FG_Space(5);

		ms_entity.Icon = EditorGUILayout.TextField("图标Icon:", ms_entity.Icon);
		EG_GUIHelper.FG_Space(5);

		ms_gobjEffect = EditorGUILayout.ObjectField ("资源:", ms_gobjEffect, typeof(GameObject), false) as GameObject;
		EG_GUIHelper.FG_Space(5);

		if (ms_gobjEffect != null) {
			if (ms_preGobjEffect != ms_gobjEffect) {
				ms_preGobjEffect = ms_gobjEffect;
				string path = AssetDatabase.GetAssetPath (ms_gobjEffect);
				ms_entity.EffectResName = Path.GetFileNameWithoutExtension (path);
			}
		} else {
			ms_entity.EffectResName = "";
		}

		EditorGUILayout.LabelField ("资源名称:" + ms_entity.EffectResName);
		EG_GUIHelper.FG_Space(5);

		ms_entity.JoinId = EditorGUILayout.Popup("挂节点:", ms_entity.JoinId, JoinType);
		EG_GUIHelper.FG_Space(5);

		ms_entity.MateChange = EditorGUILayout.TextField("材质变化:", ms_entity.MateChange);
		EG_GUIHelper.FG_Space(5);

		ms_entity.Tag = EditorGUILayout.Popup("主类型:", ms_entity.Tag, TagType);
		EG_GUIHelper.FG_Space(5);

		ms_entity.GID = EditorGUILayout.Popup("子类型:", ms_entity.GID, GIDType);
		EG_GUIHelper.FG_Space(5);

		ms_entity.Group = EditorGUILayout.IntField("分组:", ms_entity.Group);
		EG_GUIHelper.FG_Space(5);

		ms_isReplace = EditorGUILayout.Toggle("同分组的是否替换:", ms_isReplace);
		ms_entity.IsReplace = ms_isReplace ? 1 : 0;
		EG_GUIHelper.FG_Space(5);

		ms_isRest = EditorGUILayout.Toggle("替代是否重置:", ms_isRest);
		ms_entity.IsResetWhenGet = ms_isRest ? 1 : 0;
		EG_GUIHelper.FG_Space(5);

		ms_entity.Interval = EditorGUILayout.FloatField ("[持续效果]间隔时间(单位秒):", ms_entity.Interval);
		EG_GUIHelper.FG_Space(5);

		m_lFodeout[0] = EditorGUILayout.Foldout(m_lFodeout[0], "持续效果:");
		EG_GUIHelper.FG_Space(5);
		if (m_lFodeout [0]) {
			_DrawInv();
		}
		ms_entity.strEvtInterval = evt_Inv.ToJsonString ();

		m_lFodeout[1] = EditorGUILayout.Foldout(m_lFodeout[1], "瞬时效果:");
		EG_GUIHelper.FG_Space(5);
		if (m_lFodeout [1]) {
			_DrawOne ();
		}
		ms_entity.strEvtOnce = evt_One.ToJsonString ();

		m_lFodeout[2] = EditorGUILayout.Foldout(m_lFodeout[2], "每帧效果:");
		EG_GUIHelper.FG_Space(5);
		if (m_lFodeout [2]) {			
			_DrawDur();
		}
		ms_entity.strEvtDuration = evt_Dua.ToJsonString ();

		_DrawAttrChanges ();
	}

	void OnInitEntity2Attrs(EN_Buff entity)
	{
		if(entity != null)
		{
			ms_entity.DoClone (entity);

			evt_Inv.DoReInit (ms_entity.strEvtInterval);
			evt_One.DoReInit (ms_entity.strEvtOnce);
			evt_Dua.DoReInit (ms_entity.strEvtDuration);

			this.ms_isRest = ms_entity.IsResetWhenGet == 1 ? true : false;
			if(!string.IsNullOrEmpty(ms_entity.EffectResName)){
				string path = "Assets\\PackResources\\Arts\\Effect\\Prefabs\\"+ms_entity.EffectResName+".prefab";
				bool isExists = File.Exists(path);
				if (isExists) {
					this.ms_gobjEffect = AssetDatabase.LoadAssetAtPath (path, typeof(UnityEngine.GameObject)) as GameObject;
					this.ms_preGobjEffect = this.ms_gobjEffect;
				} else {
					Debug.LogWarning("资源路径path = ["+path + "],不存在！！！");
				}
			}

			listAttrChange.Clear ();

			List<EDT_Property> list = EDT_Property.ParseList (ms_entity.strAttrChange);
			if (list != null) {
				listAttrChange.AddRange (list);
			}
		}
	}

	void OnInitAttrs2Entity()
	{
		EN_Buff entity = m_opt.GetOrNew(ms_entity.ID);
		ms_entity.rowIndex = entity.rowIndex;
		entity.DoClone (ms_entity);
	}

	public void SaveExcel(string savePath){
		OnInitAttrs2Entity ();
		m_opt.Save (savePath);
	}

	void _DrawInv(){
		if (m_psInv == null) {
			m_psInv = new PS_EvtHurt("目标事件列表",true,_NewEvtInv,RmEventInv,false);
		}
		m_psInv.DoDraw (0,0, evt_Inv.GetLHurts ());
	}

	void _NewEvtInv(){
		evt_Inv.NewEvent<EDT_Hurt> ();
	}

	void RmEventInv(EDT_Base one){
		evt_Inv.RmEvent (one);
	}

	void _DrawOne(){
		if (m_psOne == null) {
			m_psOne = new PS_EvtHurt("目标事件列表",true,_NewEvtOne,RmEventOne,false);
		}
		m_psOne.DoDraw (0,0, evt_One.GetLHurts ());
	}

	void _NewEvtOne(){
		evt_One.NewEvent<EDT_Hurt> ();
	}

	void RmEventOne(EDT_Base one){
		evt_One.RmEvent (one);
	}

	void _DrawDur(){
		if (m_psDua == null) {
			m_psDua = new PS_EvtHurt("目标事件列表",true,_NewEvtDur,RmEventDur,false);
		}
		m_psDua.DoDraw (0,0, evt_Dua.GetLHurts ());
	}

	void _NewEvtDur(){
		evt_Dua.NewEvent<EDT_Hurt> ();
	}

	void RmEventDur(EDT_Base one){
		evt_Dua.RmEvent (one);
	}


	// 绘制修改属性
	PS_EvtAttrs psEvtAttrs;
	void _DrawAttrChanges(){
		if (psEvtAttrs == null) {
			psEvtAttrs = new PS_EvtAttrs ("属性修改列表:", false,_NewAttrs,_RmAttr, false);
		}

		psEvtAttrs.DoDraw (0, listAttrChange);
		ms_entity.strAttrChange = EDT_Property.ToStrFmt (listAttrChange);
	}

	void _NewAttrs(){
		EDT_Property newVal = EDT_Property.NewEntity<EDT_Property> ();
		listAttrChange.Add (newVal);
	}

	void _RmAttr(EDT_Property one){
	}
}
