using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : buff 在 windows的视图
/// 作者 : Canyon
/// 日期 : 2017-01-19 16:10
/// 功能 : 
/// </summary>
public class EG_Buff {

	EN_Buff ms_entity = new EN_Buff ();

	GameObject ms_gobjEffect;
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

	EN_BuffOpt optBuff{
		get{
			return EN_BuffOpt.Instance;
		}
	}

	public void DoInit(string path){
		optBuff.DoInit (path, 0);
	}

	public bool isInited{
		get{
			return optBuff.isInitSuccessed;
		}
	}

	public void DoClear(){
		optBuff.DoClear ();
	}

	public void DrawShow()
	{
		EG_GUIHelper.FEG_BeginH();
		ms_entity.ID = EditorGUILayout.IntField("ID:",ms_entity.ID);
		if (GUILayout.Button("查询"))
		{
			if (optBuff.isInitSuccessed)
			{
				EN_Buff ms_curEnity = optBuff.GetEntity(ms_entity.ID);
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

		EditorGUILayout.LabelField ("名称:" + ms_entity.EffectResName);
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

		EditorGUILayout.LabelField ("持续效果:", ms_entity.strEvtInterval);
		EG_GUIHelper.FG_Space(5);

		EditorGUILayout.LabelField ("瞬时效果:", ms_entity.strEvtOnce);
		EG_GUIHelper.FG_Space(5);

		ms_entity.Interval = EditorGUILayout.FloatField ("持续效果的间隔时间(单位秒):", ms_entity.Interval);
		EG_GUIHelper.FG_Space(5);

		EditorGUILayout.LabelField ("特殊效果:", ms_entity.strEvtDuration);
		EG_GUIHelper.FG_Space(5);
	}

	void OnInitEntity2Attrs(EN_Buff entity)
	{
		if(entity != null)
		{
			ms_entity.DoClone (entity);
			this.ms_isRest = ms_entity.IsResetWhenGet == 1 ? true : false;
		}
	}

	void OnInitAttrs2Entity()
	{
		EN_Buff entity = optBuff.GetOrNew(ms_entity.ID);
		ms_entity.rowIndex = entity.rowIndex;
		entity.DoClone (ms_entity);
	}

	public void SaveExcel(string savePath){
		OnInitAttrs2Entity ();
		optBuff.Save (savePath);
	}
}
