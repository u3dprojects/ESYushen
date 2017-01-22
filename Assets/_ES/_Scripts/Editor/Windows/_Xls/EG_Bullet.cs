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
public class EG_Bullet {

	EN_Bullet ms_entity = new EN_Bullet ();

	GameObject ms_gobjEffect,ms_preGobjEffect;

	GameObject ms_gobjBlowup,ms_preGobjBlowup;

	bool ms_isNearGround,ms_isFollower;

	EN_OptBullet m_opt{
		get{
			return EN_OptBullet.Instance;
		}
	}

	// 事件
	EMT_HitArea m_evtAreaFly = new EMT_HitArea();
	EMT_HitEvent m_evtFly = new EMT_HitEvent();

	EMT_HitArea m_evtAreaBlowUp = new EMT_HitArea();
	EMT_HitEvent m_evtBlowUp = new EMT_HitEvent();

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
		ms_gobjEffect = null;
		ms_preGobjEffect = null;
		ms_gobjBlowup = null;
		ms_preGobjBlowup = null;
	}

	public void DrawShow()
	{
		EG_GUIHelper.FEG_BeginH();
		ms_entity.ID = EditorGUILayout.IntField("ID:",ms_entity.ID);
		if (GUILayout.Button("查询"))
		{
			if (m_opt.isInitSuccessed)
			{
				EN_Bullet ms_curEnity = m_opt.GetEntity(ms_entity.ID);
				OnInitEntity2Attrs(ms_curEnity);
			}
			else{
				EditorUtility.DisplayDialog("Tips","没有选则技能表，不能进行查询搜索!","Okey");
			}
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		ms_gobjEffect = EditorGUILayout.ObjectField ("资源:", ms_gobjEffect, typeof(GameObject), false) as GameObject;
		EG_GUIHelper.FG_Space(5);

		if (ms_gobjEffect != null) {
			if (ms_preGobjEffect != ms_gobjEffect) {
				ms_preGobjEffect = ms_gobjEffect;
				string path = AssetDatabase.GetAssetPath (ms_gobjEffect);
				ms_entity.ResName = Path.GetFileNameWithoutExtension (path);
			}
		} else {
			ms_entity.ResName = "";
		}

		ms_entity.Speed =  EditorGUILayout.FloatField ("飞行速度:",ms_entity.Speed);
		EG_GUIHelper.FG_Space(5);

		ms_isNearGround = EditorGUILayout.Toggle("是否贴地:",ms_isNearGround);
		ms_entity.IsGround = ms_isNearGround ? 1 : 0;
		EG_GUIHelper.FG_Space(5);


		ms_entity.LifeTime = EditorGUILayout.FloatField("最大生命周期(秒):", ms_entity.LifeTime);
		EG_GUIHelper.FG_Space(5);

		ms_isFollower = EditorGUILayout.Toggle("是否跟踪目标？:", ms_isFollower);
		ms_entity.IsFollowTarget = ms_isFollower ? 1 : 0;
		EG_GUIHelper.FG_Space(5);

		ms_entity.BlowUpTime = EditorGUILayout.FloatField("爆炸时间点(秒,0表示不爆炸):", ms_entity.BlowUpTime);
		EG_GUIHelper.FG_Space(5);

		ms_gobjBlowup = EditorGUILayout.ObjectField ("爆炸特效:", ms_gobjBlowup, typeof(GameObject), false) as GameObject;
		EG_GUIHelper.FG_Space(5);

		if (ms_gobjBlowup != null) {
			if (ms_preGobjBlowup != ms_gobjBlowup) {
				ms_preGobjBlowup = ms_gobjBlowup;
				string path = AssetDatabase.GetAssetPath (ms_gobjBlowup);
				ms_entity.BlowUpEffectName = Path.GetFileNameWithoutExtension (path);
			}
		} else {
			ms_entity.BlowUpEffectName = "";
		}

		ms_entity.ThroughNum = EditorGUILayout.IntField ("穿透个数:", ms_entity.ThroughNum);
		EG_GUIHelper.FG_Space(5);

		ms_entity.BlowUpHitNum = EditorGUILayout.IntField ("爆炸可伤害个数:", ms_entity.BlowUpHitNum);
		EG_GUIHelper.FG_Space(5);

		// 绘制飞行，和爆炸的相关事件(区域检查，命中处理)
		_DrawAreaFly();
		EG_GUIHelper.FG_Space(8);

		_DrawAreaBlowUp ();

	}

	GameObject GetFabEffect(string efcName){
		string path = "Assets\\PackResources\\Arts\\Effect\\Prefabs\\"+ms_entity.ResName+".prefab";
		bool isExists = File.Exists(path);
		if (!isExists)
		{
			Debug.LogWarning("资源路径path = ["+path + "],不存在！！！");
			return null;
		}
		return AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.GameObject)) as GameObject;
	}

	void OnInitEntity2Attrs(EN_Bullet entity)
	{
		if(entity != null)
		{
			ms_entity.DoClone (entity);

			ms_isNearGround = ms_entity.IsGround == 1 ? true : false;
			ms_isFollower = ms_entity.IsFollowTarget == 1 ? true : false;

			if(!string.IsNullOrEmpty(ms_entity.ResName)){
				this.ms_gobjEffect = GetFabEffect (ms_entity.ResName);
				this.ms_preGobjEffect = this.ms_gobjEffect;
			}

			if(!string.IsNullOrEmpty(ms_entity.BlowUpEffectName)){
				this.ms_gobjBlowup = GetFabEffect (ms_entity.BlowUpEffectName);
				this.ms_preGobjBlowup = this.ms_gobjBlowup;
			}

			m_evtAreaBlowUp.DoReInit (ms_entity.AreaBlowUp);
			m_evtAreaFly.DoReInit (ms_entity.AreaFlying);

			m_evtBlowUp.DoReInit (ms_entity.EvtBlowUp, 0);
			m_evtFly.DoReInit (ms_entity.EvtFlying, 0);
		}
	}

	void OnInitAttrs2Entity()
	{
		EN_Bullet entity = m_opt.GetOrNew(ms_entity.ID);
		ms_entity.rowIndex = entity.rowIndex;
		entity.DoClone (ms_entity);
	}

	public void SaveExcel(string savePath){
		OnInitAttrs2Entity ();
		m_opt.Save (savePath);
	}

	// 绘制
	PS_EvtHurtArea m_psAreaFly;
	void _DrawAreaFly(){
		if (m_psAreaFly == null) {
			m_psAreaFly = new PS_EvtHurtArea ("飞行碰撞区域:", false, _NewAreaFly, _RmAreaFly, false);
		}
		ms_entity.AreaFlying = m_evtAreaFly.ToJsonString ();
		EG_GUIHelper.FEG_BeginVArea ();
		{
			EditorGUILayout.LabelField ("字符串值:" + ms_entity.AreaFlying, EditorStyles.textArea);
			EG_GUIHelper.FG_Space (8);

			m_psAreaFly.DoDraw (0, m_evtAreaFly.GetLAreas ());
		}
		EG_GUIHelper.FEG_EndV ();

	}

	void _NewAreaFly(){
		m_evtAreaFly.NewArea ();
	}

	void _RmAreaFly(EDT_Hurt_Area one){
		m_evtAreaFly.RmArea (one);
	}

	PS_EvtHurtArea m_psAreaBlowUp;
	void _DrawAreaBlowUp(){
		if (m_psAreaBlowUp == null) {
			m_psAreaBlowUp = new PS_EvtHurtArea ("爆炸碰撞区域:", false, _NewAreaBlowUp, _RmAreaBlowUp, false);
		}

		ms_entity.AreaBlowUp = m_evtAreaBlowUp.ToJsonString ();
		EG_GUIHelper.FEG_BeginVArea ();
		{
			EditorGUILayout.LabelField ("字符串值:" + ms_entity.AreaBlowUp, EditorStyles.textArea);
			EG_GUIHelper.FG_Space (8);

			m_psAreaBlowUp.DoDraw(0,m_evtAreaBlowUp.GetLAreas());
		}
		EG_GUIHelper.FEG_EndV ();
	}

	void _NewAreaBlowUp(){
		m_evtAreaBlowUp.NewArea ();
	}
		
	void _RmAreaBlowUp(EDT_Hurt_Area one){
		m_evtAreaBlowUp.RmArea (one);
	}

	void _DrawEventFly(){
		
	}
}
