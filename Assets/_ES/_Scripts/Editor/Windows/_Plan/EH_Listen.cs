using UnityEngine;
using System.Collections;
using System.ComponentModel;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 类名 : windows 编辑器中 监听 Hierarchy 对象的改变
/// 作者 : Canyon
/// 日期 : 2017-02-06 13:36
/// 功能 : 
/// </summary>
public static class EH_Listen {

	enum ListenType{
		[Description("无操作")]
		Default = 0,

		[Description("新增")]
		Add = 1,

		[Description("删除")]
		Reduce = 2
	}

	static public System.Action call4Changed;

	static public System.Action<int,int> call4GUI;

	static public System.Action<Transform[]> call4SelectionChange;

	static bool isChanged  = false;
	static Hashtable map = new Hashtable();

	static public void DoInit(){
		OnInitEditorDelegate ();
	}

	static public void DoClear(){
		OnClearEditorDelegate ();

		map.Clear ();

		call4Changed = null;
		call4GUI = null;
		call4SelectionChange = null;
	}

	static void OnInitEditorDelegate(){
		// EditorApplication.searchChanged += OnSearchChanged;

		EditorApplication.hierarchyWindowChanged += OnHierarchyWindowChanged;
		EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;

		Selection.selectionChanged += OnSelectionChange;
	}

	static void OnClearEditorDelegate(){
		// EditorApplication.searchChanged -= OnSearchChanged;

		EditorApplication.hierarchyWindowChanged -= OnHierarchyWindowChanged;
		EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyWindowItemOnGUI;

		Selection.selectionChanged -= OnSelectionChange;
	}

	//当窗口出去开启状态，并且在Hierarchy视图中选择某游戏对象时调用
	static void OnSelectionChange()
	{
		//有可能是多选，这里开启一个循环打印选中游戏对象的名称
		Transform[] trsfArrs = Selection.transforms;
		Transform trsf = null;
		if (trsfArrs != null && trsfArrs.Length > 0) {
			for (int i = 0; i < trsfArrs.Length; i++) {
				trsf = trsfArrs [i];
				// Debug.Log (" EH_Listen OnSelectionChange" + trsf.name);
			}
			ExcuteCallSelectionChange (trsfArrs);
		} else {
			// Debug.Log (" EH_Listen OnSelectionChange No Choose In Hierarchy");
			ExcuteCallSelectionChange (trsf);
		}
	}

	// 先回调Change
	static void OnHierarchyWindowChanged(){
		isChanged = true;
		// Debug.Log ("OnHierarchyWindowChanged ,time =  " + Time.realtimeSinceStartup);
	}

	// 后通知GUI
	static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect){
		Object obj = EditorUtility.InstanceIDToObject (instanceID);
		if (obj != null) {
			if (!map.ContainsKey (instanceID)) {
				map [instanceID] = ListenType.Add;
				isChanged = true;
			}
		}

		_HandleListenMap ();

		// _DebugLogGUI (instanceID, obj);
	}

	static void _DebugLogGUI(int instanceID,Object obj,bool isTime = false){
		string info = "Hierarchy, instanceID = " + instanceID;
		if (obj != null) {
			info += ",name = " + obj.name;
		}

		if (isTime) {
			info += ", time = " + Time.realtimeSinceStartup;
		}

		Debug.Log (info);
	}

	static void _HandleListenMap(){
		if (!isChanged) {
			return;
		}
		isChanged = false;

		ArrayList keys = new ArrayList(map.Keys);
		int lens = keys.Count;
		Object obj = null;
		int instanceID = 0;
		ListenType types = ListenType.Default;
		for (int i = 0; i < lens; i++) {
			instanceID = (int)keys [i];
			obj = EditorUtility.InstanceIDToObject (instanceID);
			if (obj == null) {
				ExcuteCallGUI (instanceID, 2);
				map.Remove (instanceID);
			} else {
				types = (ListenType)map [instanceID];
				if (types == ListenType.Add) {
					ExcuteCallGUI (instanceID, 1);
					map [instanceID] = ListenType.Default;
				}
			}
		}

	}

	static void ExcuteCallGUI(int instanceID,int types){
		if (call4GUI != null) {
			call4GUI (instanceID, types);
		}
	}

	static void ExcuteCallSelectionChange(params Transform[] selectTrsf){
		if (call4SelectionChange != null) {
			call4SelectionChange (selectTrsf);
		}
	}

	static public void AddCall4Change(System.Action callFunc)
	{
		if(call4Changed == null)
		{
			call4Changed = callFunc;
		}else
		{
			call4Changed += callFunc;
		}
	}

	static public void RemoveCall4Change(System.Action callFunc)
	{
		if (call4Changed != null)
		{
			call4Changed -= callFunc;
		}
	}

	static public void AddCall4GUI(System.Action<int,int> callFunc)
	{
		if(call4GUI == null)
		{
			call4GUI = callFunc;
		}else
		{
			call4GUI += callFunc;
		}
	}

	static public void RemoveCall4GUI(System.Action<int,int> callFunc)
	{
		if (call4GUI != null)
		{
			call4GUI -= callFunc;
		}
	}

	static public void AddCall4SelectChange(System.Action<Transform[]> callFunc)
	{
		if(call4SelectionChange == null)
		{
			call4SelectionChange = callFunc;
		}else
		{
			call4SelectionChange += callFunc;
		}
	}

	static public void RemoveCall4SelectChange(System.Action<Transform[]> callFunc)
	{
		if (call4SelectionChange != null)
		{
			call4SelectionChange -= callFunc;
		}
	}
}
