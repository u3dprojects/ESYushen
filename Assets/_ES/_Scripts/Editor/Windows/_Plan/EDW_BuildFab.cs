using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

using UnityEngine.UI;

/// <summary>
/// 类名 : 编辑器导出文件unity的资源如图片等为Fab
/// 作者 : Canyon
/// 日期 : 2017-02-10 14:50
/// 功能 : 
/// </summary>
public class EDW_BuildFab : EditorWindow {
	
	static bool isOpenWindowView = false;

	static protected EDW_BuildFab vwWindow = null;

	// 窗体宽高
	static public float width = 600;
	static public float height = 295;

	[MenuItem("Tools/Windows/BuildAsset2Fab")]
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
			vwWindow = GetWindow<EDW_BuildFab>("BuildFabs");

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

	#region  == Member Attribute ===

	// 文件夹
	UnityEngine.Object m_objFolderOrg,m_objFolderTo;

	// UGUI的图片设置宽高
	bool isSetUpWH = false;
	float m_fWidth,m_fHight;

	/// <summary>
	/// prefab的asset资源name
	/// </summary>
	string abName = "";

	/// <summary>
	/// prefab的asset资源后缀名
	/// </summary>
	string abSuffix = "";
	#endregion

	#region  == EditorWindow Func ===

	void Awake()
	{
		DoInit();
	}

	void OnEnable()
	{
	}

	void OnDisable()
	{
	}

	void OnGUI()
	{
		EG_GUIHelper.FEG_BeginV ();
		{
			EG_GUIHelper.FEG_BeginH ();
			{
				GUIStyle style = EditorStyles.label;
				style.alignment = TextAnchor.MiddleCenter;
				string txtDecs = "类名 : 生成Prefab\n"
					+ "作者 : Canyon\n"
					+ "日期 : 2017 - 02 - 10 14:50\n"
					+ "描述 : 目前只能生成unity3d 自带的UI.Image\n";
				GUILayout.Label(txtDecs, style);
				style.alignment = TextAnchor.MiddleLeft;
			}
			EG_GUIHelper.FEG_EndH ();
			EG_GUIHelper.FG_Space(10);

			EG_GUIHelper.FEG_BeginH();
			m_objFolderOrg = EditorGUILayout.ObjectField ("来源文件夹", m_objFolderOrg, typeof(UnityEngine.Object), false);
			EG_GUIHelper.FEG_EndH();
			EG_GUIHelper.FG_Space(10);

			EG_GUIHelper.FEG_BeginH();
			m_objFolderTo = EditorGUILayout.ObjectField ("Fab目标文件夹", m_objFolderTo, typeof(UnityEngine.Object), false);
			EG_GUIHelper.FEG_EndH();
			EG_GUIHelper.FG_Space(10);

			EG_GUIHelper.FEG_BeginToggleGroup ("是否指定宽(Width),高(Height)", ref isSetUpWH);
			{
				m_fWidth = EditorGUILayout.FloatField ("Width:", m_fWidth);	
				EG_GUIHelper.FG_Space(5);
				m_fHight = EditorGUILayout.FloatField ("Height:", m_fHight);
			}
			EG_GUIHelper.FEG_EndToggleGroup ();
			EG_GUIHelper.FG_Space(10);

			abName = EditorGUILayout.TextField ("Fab的Asset资源名", abName);
			EG_GUIHelper.FG_Space(10);

			abSuffix = EditorGUILayout.TextField ("Fab的Asset资源后缀", abSuffix);
			EG_GUIHelper.FG_Space(10);

			_DrawOptBtns ();
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
		OnClearSWindow();
		DoClear();
	}

	#endregion

	#region  == Self Func ===

	void DoInit()
	{
		
	}

	void DoClear()
	{
	}

	void DoMake(){
		if (m_objFolderOrg == null) {
			EditorUtility.DisplayDialog ("Tips", "请选择来源文件夹!!!", "Okey");
			return;
		}

		if (m_objFolderTo == null) {
			EditorUtility.DisplayDialog ("Tips", "请选择目标文件夹!!!", "Okey");
			return;
		}

		System.Type typeFolder = typeof(UnityEditor.DefaultAsset);
		System.Type typeOrg = m_objFolderOrg.GetType ();
		System.Type typeTo = m_objFolderTo.GetType ();

		// Debug.Log (typeOrg);

		if (typeOrg != typeFolder) {
			EditorUtility.DisplayDialog ("Tips", "来源文件不是文件夹,请选择来源文件夹!!!", "Okey");
			return;
		}

		if (typeTo != typeFolder) {
			EditorUtility.DisplayDialog ("Tips", "目标文件不是文件夹,请选择目标文件夹!!!", "Okey");
			return;
		}

		string pathOrg = AssetDatabase.GetAssetPath (m_objFolderOrg);
		string[] arrPaths = Directory.GetFiles (pathOrg);
		int lens = arrPaths.Length;

		if (lens <= 0) {
			EditorUtility.DisplayDialog ("Tips", "来源文件夹中无任何资源!!!", "Okey");
			return;
		}

		List<GameObject> list = new List<GameObject> ();
		string tmpPath = "";
		string suffix = "";
		string name = "";

		Sprite spriteTemp;
		Image imgUI;
		RectTransform rectTrsf;
		GameObject gobj;

		bool isSetSelfWH = isSetUpWH && m_fWidth > 0 && m_fHight > 0;

		for (int i = 0; i < lens; i++) {
			tmpPath = arrPaths [i];
			suffix = Path.GetExtension (tmpPath);
			name = Path.GetFileNameWithoutExtension (tmpPath);

			if (".meta".Equals (suffix, System.StringComparison.OrdinalIgnoreCase))
				continue;

			if(".png".Equals (suffix, System.StringComparison.OrdinalIgnoreCase)){
				gobj = new GameObject(name);
				imgUI = gobj.AddComponent<Image>();
				spriteTemp = AssetDatabase.LoadAssetAtPath<Sprite>(tmpPath);
				imgUI.sprite = spriteTemp;//Sprite.Create (t2dTemp);
				imgUI.raycastTarget = false;
				if (isSetSelfWH) {
					rectTrsf = gobj.GetComponent<RectTransform> ();
					rectTrsf.sizeDelta = new Vector2 (m_fWidth, m_fHight);
				} else {
					imgUI.SetNativeSize ();
				}

				list.Add(gobj);
			}
		}

		pathOrg = AssetDatabase.GetAssetPath (m_objFolderTo);
		lens = list.Count;
		for (int i = 0; i < lens; i++) {
			gobj = list [i];
			tmpPath = pathOrg + "/" + gobj.name + ".prefab";
			gobj = PrefabUtility.CreatePrefab (tmpPath, gobj,ReplacePrefabOptions.ConnectToPrefab);
			EditorUtility.SetDirty (gobj);

			SetAssetBundleInfo (tmpPath);
		}

		for (int i = 0; i < lens; i++) {
			gobj = list [i];
			GameObject.DestroyImmediate (gobj);
		}

		list.Clear ();

		EditorUtility.DisplayDialog ("Congratulations", "Congratulations Build Prefab Successed!!!", "Okey");
	}

	void SetAssetBundleInfo(string tmpPath){
		abName = abName.Trim ();
		abSuffix = abSuffix.Trim ();

		bool isABName = !string.IsNullOrEmpty (abName);
		bool isABSuffix = !string.IsNullOrEmpty (abSuffix);

		if (!isABName)
			return;
		
		AssetImporter assett = AssetImporter.GetAtPath (tmpPath);

		if (isABName) {
			// 资源名
			assett.assetBundleName = abName;
		}

		if (isABSuffix) {
			// 资源后缀名
			assett.assetBundleVariant = abSuffix;
		}
	}
		
	#endregion

	#region == Draw Func ==
	void _DrawOptBtns()
	{
		EG_GUIHelper.FEG_BeginH();
		{
			if (GUILayout.Button("DoMakeFab"))
			{
				DoMake();
			}
		}
		EG_GUIHelper.FEG_EndH();
	}
	#endregion
}
