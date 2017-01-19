using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : buff 在 windows的视图
/// 作者 : Canyon
/// 日期 : 2017-01-19 17:10
/// 功能 : 
/// </summary>
public class EG_Buff {

	EN_Buff ms_curEnity;

	int ms_ID;
	string ms_name;
	string ms_desc;
	int ms_nameId;
	int ms_descId;
	string ms_icon;
	GameObject ms_gobjEffect;
	string ms_res = "";
	int ms_join;
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
	string ms_mate;
	int ms_tag;
	string[] TagType = {
		"增益",
		"减益"
	};
	int ms_gid;
	string[] GIDType = {
		"占位"
	};
	bool ms_isRest;
	string ms_sInv;
	string ms_sOne;
	float ms_fInv;
	string ms_sDur;

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

	public void DrawExcel()
	{
		EG_GUIHelper.FEG_BeginH();
		ms_ID = EditorGUILayout.IntField("ID:",ms_ID);
		if (GUILayout.Button("查询"))
		{
			if (optBuff.isInitSuccessed)
			{
				ms_curEnity = optBuff.GetEntity(ms_ID);
				OnInitEntity2Attrs();
			}
			else{
				EditorUtility.DisplayDialog("Tips","没有选则技能表，不能进行查询搜索!","Okey");
			}
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		ms_name = EditorGUILayout.TextField("名称:", ms_name);
		EG_GUIHelper.FG_Space(5);

		ms_nameId = EditorGUILayout.IntField("名称ID:", ms_nameId);
		EG_GUIHelper.FG_Space(5);

		ms_desc = EditorGUILayout.TextField("描述:", ms_desc);
		EG_GUIHelper.FG_Space(5);

		ms_descId = EditorGUILayout.IntField("描述ID:", ms_descId);
		EG_GUIHelper.FG_Space(5);

		ms_icon = EditorGUILayout.TextField("图标Icon:", ms_icon);
		EG_GUIHelper.FG_Space(5);

		ms_gobjEffect = EditorGUILayout.ObjectField ("资源:", ms_gobjEffect, typeof(GameObject), false) as GameObject;
		EG_GUIHelper.FG_Space(5);

		EditorGUILayout.LabelField ("名称:" + ms_res);
		EG_GUIHelper.FG_Space(5);

		ms_join = EditorGUILayout.Popup("挂节点:", ms_join, JoinType);
		EG_GUIHelper.FG_Space(5);

		ms_mate = EditorGUILayout.TextField("材质变化:", ms_mate);
		EG_GUIHelper.FG_Space(5);

		ms_tag = EditorGUILayout.Popup("主类型:", ms_tag, TagType);
		EG_GUIHelper.FG_Space(5);

		ms_gid = EditorGUILayout.Popup("子类型:", ms_gid, GIDType);
		EG_GUIHelper.FG_Space(5);

		ms_isRest = EditorGUILayout.Toggle("替代是否重置:", ms_isRest);
		EG_GUIHelper.FG_Space(5);

		EditorGUILayout.LabelField ("持续效果:", ms_sInv);
		EG_GUIHelper.FG_Space(5);

		EditorGUILayout.LabelField ("瞬时效果:", ms_sInv);
		EG_GUIHelper.FG_Space(5);

		ms_fInv = EditorGUILayout.FloatField ("持续效果的间隔时间(单位秒):", ms_fInv);
		EG_GUIHelper.FG_Space(5);

		EditorGUILayout.LabelField ("特殊效果:", ms_sDur);
		EG_GUIHelper.FG_Space(5);
	}

	void OnInitEntity2Attrs()
	{
		if(ms_curEnity != null)
		{
			this.ms_ID = this.ms_curEnity.ID;
			this.ms_name = this.ms_curEnity.Name;
			this.ms_nameId = this.ms_curEnity.NameID;
			this.ms_desc = this.ms_curEnity.Desc;
			this.ms_descId = this.ms_curEnity.DescID;

			this.ms_icon = this.ms_curEnity.Icon;

			this.ms_res = this.ms_curEnity.EffectResName;
			this.ms_join = this.ms_curEnity.JoinId;
			this.ms_mate = this.ms_curEnity.MateChange;
			this.ms_tag = this.ms_curEnity.Tag;
			this.ms_gid = this.ms_curEnity.GID;
			this.ms_isRest = this.ms_curEnity.IsResetWhenGet == 1 ? true : false;
			this.ms_sInv = this.ms_curEnity.strEvtInterval;
			this.ms_sOne = this.ms_curEnity.strEvtOnce;
			this.ms_fInv = this.ms_curEnity.Interval;
			this.ms_sDur = this.ms_curEnity.strEvtDuration;
		}
	}

	void OnInitAttrs2Entity()
	{
		ms_curEnity = optBuff.GetOrNew(ms_ID);
		this.ms_curEnity.ID = this.ms_ID;
		this.ms_curEnity.Name = this.ms_name;
		this.ms_curEnity.NameID = this.ms_nameId;
		this.ms_curEnity.Desc = this.ms_desc;
		this.ms_curEnity.DescID = this.ms_descId;

		this.ms_curEnity.Icon = this.ms_icon;
		this.ms_curEnity.EffectResName = this.ms_res;
		this.ms_curEnity.JoinId = this.ms_join;
		this.ms_curEnity.MateChange = this.ms_mate;
		this.ms_curEnity.Tag = this.ms_tag;
		this.ms_curEnity.GID = this.ms_gid;
		this.ms_curEnity.IsResetWhenGet = this.ms_isRest ? 1 : 0;
		this.ms_curEnity.strEvtInterval = this.ms_sInv;
		this.ms_curEnity.strEvtOnce = this.ms_sOne;
		this.ms_curEnity.Interval = this.ms_fInv;
		this.ms_curEnity.strEvtDuration = this.ms_sDur;
	}

	public void SaveExcel(string savePath){
		OnInitAttrs2Entity ();
		optBuff.Save (savePath);
	}
}
