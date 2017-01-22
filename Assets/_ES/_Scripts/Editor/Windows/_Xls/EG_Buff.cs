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

	EN_OptBuff m_opt{
		get{
			return EN_OptBuff.Instance;
		}
	}

	// 事件
	PS_EvtExcelBuff m_psInv = new PS_EvtExcelBuff();
	PS_EvtExcelBuff m_psOne = new PS_EvtExcelBuff();
	PS_EvtExcelBuff m_psDua = new PS_EvtExcelBuff();

	List<bool> m_lFodeout = new List<bool>(){false,false,false};

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
		m_psInv.DoClear ();
		m_psOne.DoClear ();
		m_psDua.DoClear ();

		m_lFodeout [0] = false;
		m_lFodeout [1] = false;
		m_lFodeout [2] = false;
		ms_gobjEffect = null;
		ms_preGobjEffect = null;
	}

	public void DrawShow()
	{
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

		ms_isRest = EditorGUILayout.Toggle("替代是否重置:", ms_isRest);
		ms_entity.IsResetWhenGet = ms_isRest ? 1 : 0;
		EG_GUIHelper.FG_Space(5);

		ms_entity.Interval = EditorGUILayout.FloatField ("[持续效果]间隔时间(单位秒):", ms_entity.Interval);
		EG_GUIHelper.FG_Space(5);

		m_lFodeout[0] = EditorGUILayout.Foldout(m_lFodeout[0], "持续效果:");
		EG_GUIHelper.FG_Space(5);
		if (m_lFodeout [0]) {
			m_psInv.DoDraw ();
		}
		ms_entity.strEvtInterval = m_psInv.ToJsonString ();

		m_lFodeout[1] = EditorGUILayout.Foldout(m_lFodeout[1], "瞬时效果:");
		EG_GUIHelper.FG_Space(5);
		if (m_lFodeout [1]) {
			m_psOne.DoDraw ();
		}
		ms_entity.strEvtOnce = m_psOne.ToJsonString ();

		m_lFodeout[2] = EditorGUILayout.Foldout(m_lFodeout[2], "每帧效果:");
		EG_GUIHelper.FG_Space(5);
		if (m_lFodeout [2]) {
			m_psDua.DoDraw ();
		}
		ms_entity.strEvtDuration = m_psDua.ToJsonString ();
	}

	void OnInitEntity2Attrs(EN_Buff entity)
	{
		if(entity != null)
		{
			ms_entity.DoClone (entity);

			m_psInv.DoReInit (ms_entity.strEvtDuration);
			m_psOne.DoReInit (ms_entity.strEvtInterval);
			m_psDua.DoReInit (ms_entity.strEvtOnce);

			this.ms_isRest = ms_entity.IsResetWhenGet == 1 ? true : false;
			if(!string.IsNullOrEmpty(ms_entity.EffectResName)){
				string path = "Assets\\PackResources\\Arts\\Effect\\Prefabs\\"+ms_entity.EffectResName+".prefab";
				bool isExists = File.Exists(path);
				if (!isExists)
				{
					Debug.LogWarning("资源路径path = ["+path + "],不存在！！！");
					return;
				}
				this.ms_gobjEffect = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.GameObject)) as GameObject;
				this.ms_preGobjEffect = this.ms_gobjEffect;
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
}
