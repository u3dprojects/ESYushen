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
		Skill
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

	// delegate 更新
	System.Action call4OnUpdate;
	System.Action<SceneView> call4OnSceneViewGUI;

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
	}

	void OnDisable()
	{
		EditorApplication.update -= OnUpdate;
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
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
					+ "描述 : 目前可以操作Buff表。\n";
				GUILayout.Label(txtDecs, style);
				style.alignment = TextAnchor.MiddleLeft;
			}
			EG_GUIHelper.FEG_EndH ();

			EG_GUIHelper.FEG_BeginH ();
			m_emType = (EmExcelTable)EditorGUILayout.EnumPopup ("选择表类型:",(System.Enum)m_emType);

			if (m_emType != m_emPreType) {
				DoClear ();
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

	void OnDestroy()
	{
		OnClearSWindow();
		EditorApplication.update -= OnUpdate;
		SceneView.onSceneGUIDelegate -= OnSceneGUI;

		DoClear();
	}

	#endregion

	#region  == Self Func ===

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
		m_egBuff.DoClear ();
		m_egSkill.DoClear ();
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
