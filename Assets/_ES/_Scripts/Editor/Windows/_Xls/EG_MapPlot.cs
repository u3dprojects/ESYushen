using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using LitJson;

/// <summary>
/// 类名 : Map 中 剧情触发方式
/// 作者 : Canyon
/// 日期 : 2017-05-22 17:43
/// 功能 : 
/// </summary>
public partial class EG_MapPlot {

	EN_Map ms_entity = new EN_Map ();

	EN_OptMap m_opt{
		get{
			return EN_OptMap.Instance;
		}
	}

	bool m_isInView = false;

	public EG_MapPlot(){
		EH_Listen.call4GUI += OnChangeGobj;
	}

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
		m_isInView = false;
	}

	public void DrawShow()
	{
		m_isInView = true;

		EG_GUIHelper.FEG_HeadTitMid ("MapList Excel 表 - 编写 剧情模块",Color.yellow);

		EG_GUIHelper.FEG_BeginH();
		ms_entity.ID = EditorGUILayout.IntField("ID:",ms_entity.ID);
		if (GUILayout.Button("查询"))
		{
			if (m_opt.isInitSuccessed)
			{
				EN_Map ms_curEnity = m_opt.GetEntity(ms_entity.ID);
				OnInitEntity2Attrs(ms_curEnity);
			}
			else{
				EditorUtility.DisplayDialog("Tips","没有选则技能表，不能进行查询搜索!","Okey");
			}
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginH ();
		{
			ms_entity.SceneResId = EditorGUILayout.IntField ("地图场景资源ID:", ms_entity.SceneResId);
			ms_entity.SceneName = "Map_" + ms_entity.SceneResId;
			EditorGUILayout.LabelField ("地图场景:", ms_entity.SceneName);
			if (GUILayout.Button ("加载场景")) {
				OpenScene (ms_entity.SceneName);
			}
		}
		EG_GUIHelper.FEG_EndH ();
		EG_GUIHelper.FG_Space (5);

	}

	void OpenScene(string sceneName){
		string path = "";
		string path2 = "";
		string path3 = "";
		if (!string.IsNullOrEmpty (sceneName)) {
			// 在Assets下面没有任何文件夹包含的时候可以调用,(添加的到BuildSetting里面的时候，调用无效，正确的调用方式不明？？)
			Scene scene = SceneManager.GetSceneByName (sceneName);
			path = scene.path;
			if (!scene.IsValid()) {
				Debug.LogWarning ("GetSceneByName,目前测试只能加载直接放到Assets下面的场景,即为，Assets/xxx.unity！！！");

				path2 = "Assets\\PackResources\\Arts\\Test\\Map\\"+sceneName + ".unity";
				bool isExists = File.Exists(path2);
				if (isExists) {
					path = path2;
				} else {
					Debug.LogWarning ("场景路径path = [" + path2 + "],不存在！！！");
					path3 = "Assets\\PackResources\\Arts\\Map\\"+sceneName + ".unity";
					isExists = File.Exists(path3);
					if (isExists) {
						path = path3;	
					} else {
						Debug.LogWarning ("场景路径path = [" + path3 + "],不存在！！即为场景名字 = [" + sceneName + "]的场景不存在！！！");
						return;
					}
				}
			}
			UnityEditor.SceneManagement.EditorSceneManager.OpenScene (path);

			EU_ScheduleTask.m_instance.DoTask (1.2f, ReRaycast);
		}
	}

	void OnInitEntity2Attrs(EN_Map entity)
	{
		
		if(entity != null)
		{
			
			ms_entity.DoClone (entity);

			EU_ScheduleTask.m_instance.DoClear ();

			OpenScene (ms_entity.SceneName);
		}
		m_lPlot.Clear ();

		// ToList<EM_Monster>(ms_entity.strMonsters);
	}

	void OnInitAttrs2Entity()
	{
		EN_Map entity = m_opt.GetOrNew(ms_entity.ID);
		ms_entity.rowIndex = entity.rowIndex;
		ms_entity.sheet = entity.sheet;
		entity.DoClone (ms_entity);
	}

	public void SaveExcel(string savePath){
		OnInitAttrs2Entity ();
		m_opt.Save (savePath);
	}

	/// <summary>
	/// 添加scene视图绘制
	/// </summary>
	public void OnSceneGUI(SceneView sceneView){
		
	}

	#region === 刷怪点 ===

	List<UnityEngine.MonoBehaviour> m_lPlot = new List<UnityEngine.MonoBehaviour> ();

	void ReRaycast(){
		
	}

	void ReRaycast<T>(List<T> list) where T : UnityEngine.MonoBehaviour{
		int lens = list.Count;
		if (lens <= 0) {
			return;
		}

		for (int i = 0; i < lens; i++) {
			
		}
	}

	void RmMapCell(UnityEngine.MonoBehaviour cell){
		if (cell == null)
			return;
		m_lPlot.Remove (cell);
	}

	void OnChangeGobj (int instanceID, int types)
	{
		if (!m_isInView)
			return;
		
		switch (types) {
		case 1:
			Object obj = EditorUtility.InstanceIDToObject (instanceID);
			if (obj != null) {
				
			}
			break;
		case 2:
			break;
		}
	}
	#endregion

}
