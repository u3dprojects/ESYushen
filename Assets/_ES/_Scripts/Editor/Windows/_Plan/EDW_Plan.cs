using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

/// <summary>
/// 类名 : excel 数据编辑器窗口Class
/// 作者 : Canyon
/// 日期 : 2017-01-19 14:30
/// 功能 : 
/// </summary>
public class EDW_Plan : EditorWindow {
	
	static bool isOpenWindowView = false;

	static protected EDW_Plan vwWindow = null;

	// 窗体宽高
	static public float width = 600;
	static public float height = 400;

	[MenuItem("Tools/Windows/ExcelReadWrite")]
	static void AddWindow()
	{
		if (isOpenWindowView || vwWindow != null)
			return;

		try
		{
			isOpenWindowView = true;
			float x = 460;
			float y = 200;
			Rect rect = new Rect(x, y, width, height);

			// 大小不能拉伸
			// vwWindow = GetWindowWithRect<EDW_Skill>(rect, true, "SkillEditor");

			// 窗口，只能单独当成一个窗口,大小可以拉伸
			//vwWindow = GetWindow<EDW_Skill>(true,"SkillEditor");

			// 这个合并到其他窗口中去,大小可以拉伸
			vwWindow = GetWindow<EDW_Plan>("ExcelReadWrite");

			vwWindow.position = rect;
		}
		catch (System.Exception)
		{
			OnClearSWindow();
			throw;
		}
	}

	static void OnClearSWindow()
	{
		isOpenWindowView = false;
		vwWindow = null;
	}

	public enum EmExcelTable{
		Buffer,
		Bullet,
		Skill,
		Map
	}
	#region  == Member Attribute ===

	Vector2 scrollPos;
	float topDescH = 0;
	float botBtnH = 20;
	float minScrollH = 260;
	float curScrollH = 260;
	float minWidth = 440;
	float curWidth = 0;

	EmExcelTable m_emType = EmExcelTable.Buffer;
	EmExcelTable m_emPreType = EmExcelTable.Buffer;

	string pathOpenExcel;

	EG_Buff m_egBuff = new EG_Buff();
	EG_Skill m_egSkill = new EG_Skill();
	EG_Bullet m_egBullet = new EG_Bullet();
	EG_Map m_egMap = new EG_Map();

	// delegate 更新
	System.Action call4OnUpdate;
	System.Action<SceneView> call4OnSceneViewGUI;

	// Hierarchy监听
	// EH_Listen m_hListen = new EH_Listen();

	#endregion

	#region  == EditorWindow Func ===

	void Awake()
	{
		DoInit();
	}

	void OnEnable()
	{
		EditorApplication.update += OnUpdate;
		SceneView.onSceneGUIDelegate += OnSceneGUI;
		EH_Listen.DoInit ();

		call4OnSceneViewGUI = m_egMap.OnSceneGUI;
	}

	void OnDisable()
	{
		DoClear();
	}

	void OnGUI()
	{
		EG_GUIHelper.FEG_BeginV ();
		{
			EG_GUIHelper.FEG_BeginH ();
			{
				GUIStyle style = EditorStyles.label;
				style.alignment = TextAnchor.MiddleCenter;
				string txtDecs = "类名 : excel数据表编辑器窗口\n"
					+ "作者 : Canyon\n"
					+ "日期 : 2017 - 01 - 19 14:50\n"
					+ "描述 : 目前可以操作Buff表,Skill表,Bullet表,MapList表。\n";
				GUILayout.Label(txtDecs, style);
				style.alignment = TextAnchor.MiddleLeft;
			}
			EG_GUIHelper.FEG_EndH ();

			EG_GUIHelper.FEG_BeginH ();
			m_emType = (EmExcelTable)EditorGUILayout.EnumPopup ("选择表类型:",(System.Enum)m_emType);

			if (m_emType != m_emPreType) {
				OnClearData ();
				m_emPreType = m_emType;
			}
			EG_GUIHelper.FEG_EndH ();

			RecokenWH ();

			EG_GUIHelper.FG_Space (5);

			EG_GUIHelper.FEG_BeginScroll(ref scrollPos,0,curWidth,curScrollH);
			_DrawSearchExcel();

			ShowExcel ();

			EG_GUIHelper.FEG_EndScroll();

			_DrawOptExcelBtns();
		}
		EG_GUIHelper.FEG_EndV ();
	}

	// 在给定检视面板每秒10帧更新
	void OnInspectorUpdate()
	{
		Repaint();
	}

//	void OnHierarchyChange()
//	{
//		Debug.Log("当Hierarchy视图中的任何对象发生改变时调用一次");
//	}
//
//	void OnProjectChange()
//	{
//		Debug.Log("当Project视图中的资源发生改变时调用一次");
//	}
//
//	void OnSelectionChange()
//	{
//		//当窗口出去开启状态，并且在Hierarchy视图中选择某游戏对象时调用
//		foreach(Transform t in Selection.transforms)
//		{
//			//有可能是多选，这里开启一个循环打印选中游戏对象的名称
//			Debug.Log("OnSelectionChange" + t.name);
//		}
//	}

	void OnDestroy()
	{
		DoClear ();
		OnClearSWindow();
		EU_ScheduleTask.m_instance.DoDestroy();
	}

	#endregion

	#region  == Self Func ===

	void OnClearEditorDelegate(){
		EditorApplication.update -= OnUpdate;
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
	}

	void DoInit()
	{
		
	}

	void OnUpdate()
	{
		if (this.call4OnUpdate != null)
		{
			this.call4OnUpdate();
		}
	}

	void DoClear()
	{
		EH_Listen.DoClear ();

		OnClearEditorDelegate ();

		call4OnUpdate = null;
		call4OnSceneViewGUI = null;

		OnClearData ();
	}

	void OnClearData()
	{
		m_egBuff.DoClear ();
		m_egSkill.DoClear ();
		m_egBullet.DoClear ();
		m_egMap.DoClear ();

		EU_ScheduleTask.m_instance.DoClear ();
	}

	void OnSceneGUI(SceneView sceneView) {
		// Do your drawing here using Handles.
		Handles.BeginGUI();
		// Do your drawing here using GUI.
		Handles.EndGUI();    

		if (this.call4OnSceneViewGUI != null) {
			this.call4OnSceneViewGUI (sceneView);
		}
	}

	public void AddCall4Update(System.Action callFunc)
	{
		if(this.call4OnUpdate == null)
		{
			this.call4OnUpdate = callFunc;
		}else
		{
			this.call4OnUpdate += callFunc;
		}
	}

	public void RemoveCall4Update(System.Action callFunc)
	{
		if (this.call4OnUpdate != null)
		{
			this.call4OnUpdate -= callFunc;
		}
	}

	public void AddCall4SceneGUI(System.Action<SceneView> callFunc)
	{
		if(this.call4OnSceneViewGUI == null)
		{
			this.call4OnSceneViewGUI = callFunc;
		}else
		{
			this.call4OnSceneViewGUI += callFunc;
		}
	}

	public void RemoveCall4Update(System.Action<SceneView> callFunc)
	{
		if (this.call4OnSceneViewGUI != null)
		{
			this.call4OnSceneViewGUI -= callFunc;
		}
	}

	void RecokenWH()
	{
		// 100 - 是主界面顶部高度 20 - 是误差偏移
		curScrollH = (this.position.height - 100) - topDescH - botBtnH;
		curScrollH = Mathf.Max(curScrollH, minScrollH);

		curWidth = (this.position.width);
		curWidth = Mathf.Max(curWidth, minWidth);
	}

	bool isReady(){
		switch (m_emType) {
		case EmExcelTable.Buffer:
			return m_egBuff.isInited;
		case EmExcelTable.Skill:
			return m_egSkill.isInited;
		case EmExcelTable.Bullet:
			return m_egBullet.isInited;
		case EmExcelTable.Map:
			return m_egMap.isInited;
		}
		return false;
	}

	void InitExcel(string pathOpen){
		switch (m_emType) {
		case EmExcelTable.Buffer:
			m_egBuff.DoInit(pathOpen);
			break;
		case EmExcelTable.Skill:
			m_egSkill.DoInit (pathOpen);
			break;
		case EmExcelTable.Bullet:
			m_egBullet.DoInit (pathOpen);
			break;
		case EmExcelTable.Map:
			m_egMap.DoInit (pathOpen);
			break;
		}
	}

	void SaveExcel(string savePath){
		switch (m_emType) {
		case EmExcelTable.Buffer:
			m_egBuff.SaveExcel (savePath);
			break;
		case EmExcelTable.Skill:
			m_egSkill.SaveExcel (savePath);
			break;
		case EmExcelTable.Bullet:
			m_egBullet.SaveExcel (savePath);
			break;
		case EmExcelTable.Map:
			m_egMap.SaveExcel (savePath);
			break;
		}
	}

	void ShowExcel(){
		switch (m_emType) {
		case EmExcelTable.Buffer:
			m_egBuff.DrawShow ();
			break;
		case EmExcelTable.Skill:
			m_egSkill.DrawShow ();
			break;
		case EmExcelTable.Bullet:
			m_egBullet.DrawShow ();
			break;
		case EmExcelTable.Map:
			m_egMap.DrawShow ();
			break;
		}
	}

	#endregion

	#region == Draw Func ==

	void _DrawSearchExcel()
	{
		EG_GUIHelper.FEG_BeginH();
		if (GUILayout.Button("选取Excel表"))
		{
			this.pathOpenExcel = UnityEditor.EditorUtility.OpenFilePanel("选取excel文件", "", "xls");
			InitExcel (this.pathOpenExcel);
		}
		EG_GUIHelper.FEG_EndH();

		EG_GUIHelper.FG_Space(5);

		if (string.IsNullOrEmpty(this.pathOpenExcel))
		{
			EG_GUIHelper.FEG_BeginH();
			EditorGUILayout.HelpBox("请单击选取Excel,目前不能根据ID进行查询?", MessageType.Error);
			EG_GUIHelper.FEG_EndH();
		}else
		{
			EG_GUIHelper.FEG_BeginH();
			EG_GUIHelper.FG_Label("Excel表路径:" + this.pathOpenExcel);
			EG_GUIHelper.FEG_EndH();
		}

		EG_GUIHelper.FG_Space(5);
	}

	void _DrawOptExcelBtns()
	{
		EG_GUIHelper.FEG_BeginH();
		{
			if (GUILayout.Button("Save Excel"))
			{
				if (!isReady())
				{
					EditorUtility.DisplayDialog("Tips", "没有选则表，不能进行保存!", "Okey");
					return;
				}

				string folder = Path.GetDirectoryName(this.pathOpenExcel);
				string fileName = Path.GetFileNameWithoutExtension(this.pathOpenExcel);
				string suffix = Path.GetExtension(this.pathOpenExcel);
				suffix = suffix.Replace(".", "");
				string savePath = UnityEditor.EditorUtility.SaveFilePanel("保存Excel文件", folder, fileName, suffix);

				if (string.IsNullOrEmpty(savePath))
				{
					UnityEditor.EditorUtility.DisplayDialog("Tips", "The save path is Empty !!!", "Okey");
					return;
				}

				SaveExcel (savePath);
			}
		}
		EG_GUIHelper.FEG_EndH();
	}
	#endregion
}
