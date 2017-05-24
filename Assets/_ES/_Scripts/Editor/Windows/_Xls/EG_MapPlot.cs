/// <summary>
/// 是否是美术工程
/// </summary>
// #define YuShenGameArtProject

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

	/// <summary>
	/// 剧情json对象
	/// </summary>
	JsonData m_jsonData = new JsonData ();

	/// <summary>
	/// 延迟更新数据
	/// </summary>
	float m_reDelay = 0.1f;

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
		OnClearDelegate();
	}

	public void DrawShow()
	{

		OnInitDelegate ();

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

		try {
			CheckPlotJson ();
			#if YuShenGameArtProject
			m_reDelay -= Time.fixedDeltaTime;
			if (m_reDelay < 0) {
				m_reDelay = 0.1f;
				RefreshAllJsonData ();
			}
			#endif
			ms_entity.strStory = m_jsonData.Count > 0 ? m_jsonData.ToJson () : "null";
			EditorGUILayout.LabelField("场景触发区域:", ms_entity.strStory, EditorStyles.textArea);
		} catch (System.Exception ex) {
			#if YuShenGameArtProject
			m_dicPlot.Clear ();
			#endif
		}
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
		}
	}

	void OnInitEntity2Attrs(EN_Map entity)
	{
		
		if(entity != null)
		{
			#if YuShenGameArtProject
			m_dicPlot.Clear ();
			#endif

			ms_entity.DoClone (entity);


			OpenScene (ms_entity.SceneName);
		}

//		if (!string.IsNullOrEmpty (ms_entity.strStory) && !"null".Equals(ms_entity.strStory.ToLower().Trim())) {
//			JsonData data = JsonMapper.ToObject (ms_entity.strStory);
//			if (data.IsArray) {
//				int lens = data.Count;
//				JsonData mapData = null;
//				for (int i = 0; i < lens; i++) {
//					mapData = data [i];
//					int unique = (int)mapData ["unique"];
//					InitPlotCell (unique);
//				}
//			}
//		}
	}

	void CheckPlotJson(){
		if (m_jsonData == null) {
			m_jsonData = new JsonData ();
		}


		if (!m_jsonData.IsArray) {
			m_jsonData.Clear ();
			m_jsonData.SetJsonType (JsonType.Array);
		}
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

		SaveScene ();
	}

	/// <summary>
	/// 添加scene视图绘制
	/// </summary>
	public void OnSceneGUI(SceneView sceneView){
		
	}

	void OnInitDelegate(){
		TransformEditor.AddEvent(OnChangeTransform);
	}

	void OnClearDelegate(){
		TransformEditor.RemoveEvent(OnChangeTransform);
	}

	void OnChangeTransform(Transform trsf){
		#if YuShenGameArtProject
		RefreshAllJsonData ();
		#endif
	}


	void SaveScene(){

		#if YuShenGameArtProject
		m_jsonData.Clear ();
		int lens = 0;
		bool isValid = false;
		MapTrigger tmp = null;

		m_lPlot.Clear ();
		m_lPlot.AddRange (m_dicPlot.Values);
		lens = m_lPlot.Count;
		for (int i = 0; i < lens; i++) {
			tmp = m_lPlot [i];
			isValid = IsValid (tmp);
			if (isValid) {
				if(tmp.cameraObject != null && tmp.cameraGameObject == null){
					GameObject gobj = GameObject.Instantiate(tmp.cameraObject) as GameObject;
					gobj.name = "PlotModel";
					gobj.transform.SetParent(tmp.transform,false);

					tmp.cameraGameObject = gobj;
					tmp.cameraTransform = gobj.transform;
					tmp.cameraAnimator = gobj.GetComponent<Animator>();
					gobj.SetActive(false);
				}
			}else{
				RmPlotCell(tmp);
				GameObject.DestroyImmediate (tmp.gameObject);
			}
		}
		#endif

		UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
		UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
	}

	void OnChangeGobj (int instanceID, int types)
	{
		if (!m_isInView)
			return;

		// Debug.Log (string.Format("id = {0},types = {1}",instanceID,types));

		#if YuShenGameArtProject
		switch (types) {
		case 1:
			InitPlotCell (instanceID);
			break;
		case 2:
			RmPlotCell (instanceID);
			break;
		}
		#endif
	}

	#region === 刷怪点 ===

	#if YuShenGameArtProject

	Dictionary<int,MapTrigger> m_dicPlot = new Dictionary<int, MapTrigger>();
	List<MapTrigger> m_lPlot = new List<MapTrigger> ();

	bool IsValid(MapTrigger one){
		if (one.triggerID <= 0)
			return false;
		
		if (one.totalTime <= 0)
			return false;

		if (one.destPos == Vector3.zero) {
			return false;
		}

		if (one.masterAniID <= 0) {
			return false;
		}

		Collider col = one.GetComponent<Collider> ();
		if (col == null) {
			return false;
		}


		return true;
	}

	JsonData ToJsonData(MapTrigger one){
		if (!IsValid(one)) {
			return null;
		}

		Collider col = one.GetComponent<Collider> ();

		JsonData data = new JsonData ();
		data.SetJsonType (JsonType.Object);
		// data ["unique"] = one.GetInstanceID ();
		data ["storyID"] = one.triggerID;
		data ["timeLength"] = EJ_Base.Round2D(one.totalTime,2);

		JsonData tmp = new JsonData ();
		tmp["x"] = EJ_Base.Round2D(one.destPos.x,2);
		tmp["y"] = EJ_Base.Round2D(one.destPos.y,2);
		tmp["z"] = EJ_Base.Round2D(one.destPos.z,2);
		data ["destPos"] = tmp;

		int area_type = 1;
		if (col is BoxCollider) {
			area_type = 2;
		}

		Transform trsf = one.transform;
		data ["area_type"] = area_type;
		data ["area_centerX"] = EJ_Base.Round2D(trsf.position.x,2);
		data ["area_centerZ"] = EJ_Base.Round2D(trsf.position.z,2);
		if (area_type == 1) {
			CapsuleCollider capCol = col as CapsuleCollider;
			data ["area_range"] = EJ_Base.Round2D(capCol.radius,2);
			data ["area_width"] = 0;
			data ["area_rot"] = 0;
		} else {
			data ["area_range"] = EJ_Base.Round2D(col.bounds.size.x,2);	
			data ["area_width"] = EJ_Base.Round2D(col.bounds.size.z,2);
			data ["area_rot"] = EJ_Base.Round2D(trsf.eulerAngles.y,2);
		}
		return data;
	}

	void RmPlotCell(int instanceID){
		m_dicPlot.Remove (instanceID);
	}

	void RmPlotCell(UnityEngine.MonoBehaviour cell){
		if (cell == null)
			return;
		RmPlotCell(cell.GetInstanceID ());
	}

	void InitPlotCell(int instanceID){
		Object obj = EditorUtility.InstanceIDToObject (instanceID);
		if (obj != null) {
			GameObject gobj = obj as GameObject;
			MapTrigger[] arrs = gobj.GetComponentsInChildren<MapTrigger> (true);
			if (arrs == null || arrs.Length <= 0) {
				return;
			}

			MapTrigger tmpCell;
			for (int i = 0; i < arrs.Length; i++) {
				tmpCell = arrs [i];
				if (tmpCell != null && !m_dicPlot.ContainsKey (tmpCell.GetInstanceID ())) {
					PrefabType ptype = PrefabUtility.GetPrefabType (tmpCell.gameObject);
					if (ptype == PrefabType.PrefabInstance) {
						PrefabUtility.DisconnectPrefabInstance (tmpCell.gameObject);
					}
					m_dicPlot.Add (tmpCell.GetInstanceID (), tmpCell);
					RefreshAllJsonData ();
				}
			}

		} else {
			RmPlotCell (instanceID);	
			RefreshAllJsonData ();
		}
	}

	void RefreshAllJsonData(){
		m_jsonData.Clear ();
		m_lPlot.Clear ();
		m_lPlot.AddRange (m_dicPlot.Values);
		int lens = m_lPlot.Count;
		JsonData tmp = null;
		for (int i = 0; i < lens; i++) {
			tmp = ToJsonData(m_lPlot[i]);
			if (tmp == null)
				continue;
			m_jsonData.Add (tmp);
		}
	}

	#endif

	#endregion

}
