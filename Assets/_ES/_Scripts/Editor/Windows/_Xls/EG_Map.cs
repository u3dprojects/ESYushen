using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

/// <summary>
/// 类名 : Map 在 windows的视图
/// 作者 : Canyon
/// 日期 : 2017-02-03 11:10
/// 功能 : 
/// </summary>
public class EG_Map {

	EN_Map ms_entity = new EN_Map ();

	EN_OptMap m_opt{
		get{
			return EN_OptMap.Instance;
		}
	}

	Vector3 m_v3PosBorn = Vector3.zero;

	// 音效
	UnityEngine.Object m_objAudio,m_objPreAudio;

	// 场景
	// UnityEngine.Object m_objScene,m_objPreScene;

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
	}

	public void DrawShow()
	{
		EG_GUIHelper.FEG_HeadTitMid ("MapList Excel 表",Color.cyan);

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

		ms_entity.NameID = EditorGUILayout.IntField("名称ID:", ms_entity.NameID);
		EG_GUIHelper.FG_Space(5);

		ms_entity.DescID = EditorGUILayout.IntField("描述ID:", ms_entity.DescID);
		EG_GUIHelper.FG_Space(5);

		// 场景
//		m_objScene = EditorGUILayout.ObjectField ("地图场景:", m_objScene, typeof(UnityEngine.Object), false);
//		EG_GUIHelper.FG_Space(5);
//		EG_GUIHelper.FG_Space(5);
//		if (m_objScene != null) {
//			if (m_objPreScene != m_objScene) {
//				m_objPreScene = m_objScene;
//				string path = AssetDatabase.GetAssetPath (m_objScene);
//				ms_entity.SceneName = Path.GetFileNameWithoutExtension (path);
//			}
//		} else {
//			ms_entity.SceneName = "";
//		}

		EG_GUIHelper.FEG_BeginH ();
		{
			ms_entity.SceneName = EditorGUILayout.TextField ("地图场景:", ms_entity.SceneName);
			if (GUILayout.Button ("加载场景")) {
				OpenScene (ms_entity.SceneName);
			}
		}
		EG_GUIHelper.FEG_EndH ();
		EG_GUIHelper.FG_Space (5);

		ms_entity.UIResName = EditorGUILayout.TextField("UI资源名:", ms_entity.UIResName);
		EG_GUIHelper.FG_Space(5);

		m_v3PosBorn = EditorGUILayout.Vector3Field ("出生点坐标:", m_v3PosBorn);
		EG_GUIHelper.FG_Space(5);
		ms_entity.PosX = m_v3PosBorn.x;
		ms_entity.PosZ = m_v3PosBorn.z;

		ms_entity.Rotation = EditorGUILayout.FloatField ("出生朝向:", ms_entity.Rotation);
		EG_GUIHelper.FG_Space(5);

		ms_entity.Width = EditorGUILayout.FloatField ("地图宽:", ms_entity.Width);
		EG_GUIHelper.FG_Space(5);

		ms_entity.Length = EditorGUILayout.FloatField ("地图长:", ms_entity.Length);
		EG_GUIHelper.FG_Space(5);

		// 背景音乐
		m_objAudio = EditorGUILayout.ObjectField ("背景音乐:", m_objAudio, typeof(AudioClip), false) as AudioClip;
		EG_GUIHelper.FG_Space(5);
		if (m_objAudio != null) {
			if (m_objPreAudio != m_objAudio) {
				m_objPreAudio = m_objAudio;
				string path = AssetDatabase.GetAssetPath (m_objAudio);
				ms_entity.BgMusic = Path.GetFileNameWithoutExtension (path);
			}
		} else {
			ms_entity.BgMusic = "";
		}

		ms_entity.NodeRow = EditorGUILayout.IntField("分块行数:", ms_entity.NodeRow);
		EG_GUIHelper.FG_Space(5);

		ms_entity.NodeColumn = EditorGUILayout.IntField("分块列数:", ms_entity.NodeColumn);
		EG_GUIHelper.FG_Space(5);

		// 刷怪点
		// 触发器点
	}

	void OpenScene(string sceneName){
		string path = "";
		string path2 = "";
		if (!string.IsNullOrEmpty (sceneName)) {
			// 在Assets下面没有任何文件夹包含的时候可以调用,(添加的到BuildSetting里面的时候，调用无效，正确的调用方式不明？？)
			Scene scene = SceneManager.GetSceneByName (sceneName);
			path = scene.path;
			if (!scene.IsValid()) {
				Debug.LogWarning ("GetSceneByName,目前测试只能加载直接放到Assets下面的场景,即为，Assets/xxx.unity！！！");

				path2 = "Assets\\PackResources\\Arts\\Test\\Map\\"+sceneName + ".unity";
				bool isExists = File.Exists(path2);
				if (!isExists) {
					Debug.LogWarning ("场景路径path = ["+path2+"],即为场景名字 = [" + sceneName + "]的场景不存在！！！");
					return;
				}

				path = path2;
			}
			UnityEditor.SceneManagement.EditorSceneManager.OpenScene (path);
		}
	}

	void OnInitEntity2Attrs(EN_Map entity)
	{
		if(entity != null)
		{
			ms_entity.DoClone (entity);

			if(!string.IsNullOrEmpty(ms_entity.BgMusic)){
				string path = "Assets\\PackResources\\Arts\\Sound\\"+ms_entity.BgMusic+".mp3";
				bool isExists = File.Exists(path);
				if (isExists) {
					this.m_objAudio = AssetDatabase.LoadAssetAtPath (path, typeof(UnityEngine.AudioClip)) as AudioClip;
					this.m_objPreAudio = this.m_objAudio;
				} else {
					Debug.LogWarning ("资源路径path = [" + path + "],不存在！！！");
				}
			}

			OpenScene (ms_entity.SceneName);
		}
	}

	void OnInitAttrs2Entity()
	{
		EN_Map entity = m_opt.GetOrNew(ms_entity.ID);
		ms_entity.rowIndex = entity.rowIndex;
		entity.DoClone (ms_entity);
	}

	public void SaveExcel(string savePath){
		OnInitAttrs2Entity ();
		m_opt.Save (savePath);
	}
}
