using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using LitJson;

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

	// 雷达的偏移量
	Vector3 m_v3OffsetRadar = Vector3.zero;

	// 音效
	UnityEngine.Object m_objAudio,m_objPreAudio;

	// 场景
	// UnityEngine.Object m_objScene,m_objPreScene;

	bool m_isInView = false;

	public EG_Map(){
		EH_Listen.call4GUI += OnChangeGobj;
		EH_Listen.call4SelectionChange += OnSelectionChange;
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

		OnClearBornMonster ();

		EM_Monster.DoClearStatic ();

		OnClearDelegate ();

		m_isInView = false;
	}

	public void DrawShow()
	{
		m_isInView = true;

		_DrawChooseMonster ();

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
			ms_entity.SceneResId = EditorGUILayout.IntField ("地图场景资源ID:", ms_entity.SceneResId);
			ms_entity.SceneName = "Map_" + ms_entity.SceneResId;
			EditorGUILayout.LabelField ("地图场景:", ms_entity.SceneName);
			if (GUILayout.Button ("加载场景")) {
				OpenScene (ms_entity.SceneName);
			}
		}
		EG_GUIHelper.FEG_EndH ();
		EG_GUIHelper.FG_Space (5);

		ms_entity.UIResName = EditorGUILayout.TextField("雷达UI资源名:", ms_entity.UIResName);
		EG_GUIHelper.FG_Space(5);

		m_v3OffsetRadar = EditorGUILayout.Vector3Field ("雷达视图偏移量:", m_v3OffsetRadar);
		EG_GUIHelper.FG_Space(5);
		ms_entity.radarOffsetX = m_v3OffsetRadar.x;
		ms_entity.radarOffsetZ = m_v3OffsetRadar.z;

		ms_entity.radarLength = EditorGUILayout.FloatField ("雷达缩略图长:", ms_entity.radarLength);
		EG_GUIHelper.FG_Space(5);

		ms_entity.radarWidth = EditorGUILayout.FloatField ("雷达缩略图宽:", ms_entity.radarWidth);
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

		EG_GUIHelper.FEG_BeginH ();
		EditorGUILayout.LabelField ("[0绝对和平地图,1和平地图,2战斗地图]:");
		ms_entity.AreaStatus = EditorGUILayout.IntField(ms_entity.AreaStatus);
		EG_GUIHelper.FEG_EndH ();
		EG_GUIHelper.FG_Space(5);

		// 刷怪点
		_DrawBornMonster();

		// 怪物聚集中心点
		_DrawMCenter();

		// Npc
		_DrawBornNpc();
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
		m_v3PosBorn = Vector3.zero;
		m_v3OffsetRadar = Vector3.zero;

		if(entity != null)
		{
			EM_Monster.DoClearStatic ();

			ms_entity.DoClone (entity);

			m_v3PosBorn.x = ms_entity.PosX;
			m_v3PosBorn.z = ms_entity.PosZ;

			m_v3OffsetRadar.x = ms_entity.radarOffsetX;
			m_v3OffsetRadar.z = ms_entity.radarOffsetZ;

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

			EU_ScheduleTask.m_instance.DoClear ();

			OpenScene (ms_entity.SceneName);
		}
		m_lMapCells.Clear ();

		ToList<EM_Monster>(ms_entity.strMonsters);
		ToList<EM_NPC>(ms_entity.strNpcs);
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

	#region === 刷怪点 ===

	List<EM_Base> m_lMapCells = new List<EM_Base> ();
	JsonData jsonData = new JsonData ();
	JsonData tmpJD;
	EM_Base tmpCell;

	void ToList<T>(string json) where T : EM_Base,new()
	{
		if (string.IsNullOrEmpty (json) || "null".Equals (json,System.StringComparison.OrdinalIgnoreCase)) {
			return;
		}

		JsonData jsonData = JsonMapper.ToObject (json);
		if(!jsonData.IsArray){
			return;
		}

		int lens = jsonData.Count;
		T temp = null;
		for (int i = 0; i < lens; i++) {
			tmpJD = jsonData [i];
			temp = EM_Base.NewEntity<T> (tmpJD);
			if (temp == null) {
				continue;
			}

			m_lMapCells.Add (temp);
		}
	}

	string ToJsonString<T>(List<T> list) where T : EM_Base{
		int lens = list.Count;
		if (lens <= 0) {
			return "null";
		}
		jsonData.Clear ();
		jsonData.SetJsonType (JsonType.Array);
		for (int i = 0; i < lens; i++) {
			tmpCell = list [i];
			tmpJD = tmpCell.ToJsonData ();
			if (tmpJD == null)
				continue;
			jsonData.Add(tmpJD);
		}

		return JsonMapper.ToJson (jsonData);
	}

	EM_Base GetEntity(int instanceID){
		int lens = m_lMapCells.Count;
		if (lens <= 0) {
			return null;
		}

		for (int i = 0; i < lens; i++) {
			tmpCell = m_lMapCells [i];
			if (tmpCell.m_iGobjInstanceID == instanceID)
				return tmpCell;
		}
		return null;
	}

	void RefreshFoldOut(params Transform[] trsfs){
		int lens = m_lMapCells.Count;
		if (lens <= 0) {
			return;
		}

		for (int i = 0; i < lens; i++) {
			tmpCell = m_lMapCells [i];
			tmpCell.m_isOpenFoldout = _IsHas(tmpCell.m_trsf,trsfs);
		}
	}


	void ReRaycast(){
		ReRaycast<EM_Monster> (GetLMonsters ());
		ReRaycast<EM_NPC> (GetLNpcs ());
	}

	void ReRaycast<T>(List<T> list) where T : EM_UnitCell{
		int lens = list.Count;
		if (lens <= 0) {
			return;
		}

		for (int i = 0; i < lens; i++) {
			(list [i]).DoRaycast();
		}
	}

	bool _IsHas (Transform trsfCur, params Transform[] org){
		if (org == null || org.Length <= 0)
			return false;
		int lens = org.Length;
		Transform trsfTemp = null;
		bool isHas = false;
		for (int i = 0; i < lens; i++) {
			trsfTemp = org [i];
			isHas = (trsfTemp == trsfCur);
			if (isHas)
				break;
		}
		return isHas;
	}

	void RmMapCell(EM_Base cell){
		if (cell == null)
			return;
		cell.DoClear ();
		m_lMapCells.Remove (cell);
	}

	void OnClearBornMonster(){
		jsonData.Clear ();
		OnClearList();
	}

	void OnClearList(){
		int lens = m_lMapCells.Count;
		if (lens <= 0) {
			return;
		}
		for (int i = 0; i < lens; i++) {
			tmpCell = m_lMapCells [i];
			tmpCell.DoClear ();
		}
		m_lMapCells.Clear();
	}

	void OnChangeTransform(Transform trsf){
		int lens = m_lMapCells.Count;
		if (lens <= 0) {
			return;
		}

		for (int i = 0; i < lens; i++) {
			tmpCell = m_lMapCells [i];
			tmpCell.OnChangeTransform (trsf);
		}
	}

	void _NewMapCell(int instanceID, GameObject gobj){
		if (gobj == null)
			return;

		EUM_Cell enCell = gobj.GetComponent<EUM_Cell> ();
		if (enCell == null) {
			return;
		}

		EM_Base one = GetEntity (instanceID);
		if (one != null && gobj == one.m_gobj) {
			return;
		}

		if (one == null) {
			// one = EM_Base.NewEntity<EM_Monster> ();
			if (enCell is EUM_Monster) {
				one = ((EUM_Monster)enCell).m_entity;
			} else if (enCell is EUM_Npc) {
				one = ((EUM_Npc)enCell).m_entity;
			} else if (enCell is EUM_MonsterCenter) {
				one = ((EUM_MonsterCenter)enCell).m_entity;
			}
			m_lMapCells.Add (one);
		}

		one.Reset (gobj);
	}

	void OnReInitDelegate(){
		OnClearDelegate ();
		OnInitDelegate ();
	}

	void OnInitDelegate(){
		TransformEditor.onChangeTransform += OnChangeTransform;
	}

	void OnClearDelegate(){
		TransformEditor.onChangeTransform -= OnChangeTransform;
	}

	void OnChangeGobj (int instanceID, int types)
	{
		if (!m_isInView)
			return;
		
		switch (types) {
		case 1:
			Object obj = EditorUtility.InstanceIDToObject (instanceID);
			if (obj != null) {
				_NewMapCell (instanceID,obj as GameObject);
			}
			break;
		case 2:
			tmpCell = GetEntity (instanceID);
			if (tmpCell != null) {
				tmpCell.m_gobj = null;
				RmMapCell (tmpCell);
			}
			break;
		}
	}

	void OnSelectionChange(params Transform[] trsf){
		if (!m_isInView)
			return;
		
		RefreshFoldOut (trsf);
	}

	// 刷怪点
	List<EM_Monster> m_lMapMonsters = new List<EM_Monster> ();
	public List<EM_Monster> GetLMonsters(){
		EM_Base.GetList<EM_Monster> (m_lMapCells, ref m_lMapMonsters);
		return m_lMapMonsters;
	}

	PSM_Monster m_psMonster;
	void _DrawBornMonster(){
		if (m_psMonster == null) {
			m_psMonster = new PSM_Monster ("刷怪点", _NewMonster, RmMapCell);

			OnReInitDelegate ();
		}

		ms_entity.strMonsters = ToJsonString<EM_Monster> (GetLMonsters());
		EditorGUILayout.LabelField("刷怪点",ms_entity.strMonsters, EditorStyles.textArea);
		EG_GUIHelper.FG_Space(5);

		m_psMonster.DoDraw (GetLMonsters ());

		EG_GUIHelper.FG_Space(10);
	}

	void _NewMonster(){
		EM_Monster one = EM_Monster.NewEntity<EM_Monster> ();
		one.DoMakeNew ();
		m_lMapCells.Add (one);
	}

	// Npc点
	List<EM_NPC> m_lNpcs = new List<EM_NPC> ();
	public List<EM_NPC> GetLNpcs(){
		EM_Base.GetList<EM_NPC> (m_lMapCells, ref m_lNpcs);
		return m_lNpcs;
	}

	PSM_Npc m_psNpc;
	void _DrawBornNpc(){
		if (m_psNpc == null) {
			m_psNpc = new PSM_Npc ("刷NPC点", _NewNpc, RmMapCell);

			OnReInitDelegate ();
		}

		ms_entity.strNpcs = ToJsonString<EM_NPC> (GetLNpcs());
		EditorGUILayout.LabelField("刷NPC点",ms_entity.strNpcs, EditorStyles.textArea);
		EG_GUIHelper.FG_Space(5);

		m_psNpc.DoDraw (GetLNpcs ());

		EG_GUIHelper.FG_Space(10);
	}

	void _NewNpc(){
		EM_NPC one = EM_NPC.NewEntity<EM_NPC> ();
		one.DoMakeNew ();
		m_lMapCells.Add (one);
	}

	// 怪物聚集中心点
	List<EM_MonsterCenter> m_lMCenter = new List<EM_MonsterCenter> ();
	public List<EM_MonsterCenter> GetLMCenters(){
		EM_Base.GetList<EM_MonsterCenter> (m_lMapCells, ref m_lMCenter);
		return m_lMCenter;
	}

	PSM_MonsterCenter m_psMCenter;
	void _DrawMCenter(){
		if (m_psMCenter == null) {
			m_psMCenter = new PSM_MonsterCenter ("怪物聚集中心点", _NewMCenter, RmMapCell);

			OnReInitDelegate ();
		}

		ms_entity.strMonsterCenters = ToJsonString<EM_MonsterCenter> (GetLMCenters());
		EditorGUILayout.LabelField("怪物聚集中心点",ms_entity.strMonsterCenters, EditorStyles.textArea);
		EG_GUIHelper.FG_Space(5);

		m_psMCenter.DoDraw (GetLMCenters ());

		EG_GUIHelper.FG_Space(10);
	}

	void _NewMCenter(){
		EM_MonsterCenter one = EM_MonsterCenter.NewEntity<EM_MonsterCenter> ();
		one.DoMakeNew ();
		m_lMapCells.Add (one);
	}

	#endregion

	EN_OptMonster optMonster{
		get{
			return EN_OptMonster.Instance;
		}
	}

	void _DrawChooseMonster(){
		EG_GUIHelper.FEG_BeginH();

		Color def = GUI.color;
		if (optMonster.isInitSuccessed) {
			GUI.color = Color.green;
		} else {
			GUI.color = Color.red;
		}

		EditorGUILayout.LabelField ("初始化了怪物数据", "状态："+(optMonster.isInitSuccessed ? "Success - " + optMonster.m_eOptXls.fileName : "未选择怪物Excel"));

		if (GUILayout.Button("选取Monster Excel表"))
		{
			string path = UnityEditor.EditorUtility.OpenFilePanel("选取excel文件", "", "xls");
			optMonster.DoInit (path, 0);
		}
		EG_GUIHelper.FEG_EndH();

		GUI.color = def;
		EG_GUIHelper.FG_Space(10);
	}
}
