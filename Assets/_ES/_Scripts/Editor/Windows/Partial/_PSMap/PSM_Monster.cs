using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

/// <summary>
/// 类名 : 绘制 地图元素 - 刷怪点 
/// 作者 : Canyon
/// 日期 : 2017-02-03 18:10
/// 功能 : 
/// </summary>
public class PSM_Monster : PSM_Base<EM_Monster> {

	int m_iMID = 0;

	GameObject m_gobjModel;

	public PSM_Monster(string title,System.Action callNew,System.Action<EM_Monster> callRemove) : base(title,callNew,callRemove){
	}

	protected override void _DrawOneAttrs (EM_Monster one)
	{
		base._DrawOneAttrs (one);

		one.OnResetColor ();

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("怪物ID:", GUILayout.Width(80));
			one.m_iUnqID = EditorGUILayout.IntField (one.m_iUnqID);
			if (m_iMID != one.m_iUnqID) {
				one.m_isShowModel = false;
				m_iMID = one.m_iUnqID;

				one.DoDestroyChild ();
				m_gobjModel = null;
			}
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		one.m_v3Pos = EditorGUILayout.Vector3Field ("产生点:", one.m_v3Pos);
		EG_GUIHelper.FG_Space(5);

		one.m_fRotation = EditorGUILayout.FloatField ("初始旋转角度:", one.m_fRotation);
		EG_GUIHelper.FG_Space(5);

		one.m_fReliveInv = EditorGUILayout.FloatField ("刷新时间间隔:", one.m_fReliveInv);
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FEG_BeginToggleGroup ("是否显示怪物模型", ref one.m_isShowModel);
		{
			if (!EN_OptMonster.Instance.isInitSuccessed) {
				EditorGUILayout.LabelField ("未选择怪物Excel,不能显示模型");
			}
		}
		EG_GUIHelper.FEG_EndToggleGroup ();
		_ShowMonster (one);

		EG_GUIHelper.FG_BeginH ();
		{
//			if (GUILayout.Button ("SyncToData")) {
//				one.ToData ();
//			}
			if (GUILayout.Button ("SyncToInspector")) {
				one.ToTrsfData ();
			}
		}
		EG_GUIHelper.FG_EndH ();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FG_BeginH ();
		{
			if (GUILayout.Button ("Choose Cell(选中物体)")) {
				one.DoActiveInHierarchy();
			}
		}
		EG_GUIHelper.FG_EndH ();
		EG_GUIHelper.FG_Space(5);
	}

	void _ShowMonster(EM_Monster one){
		if (!EN_OptMonster.Instance.isInitSuccessed) {
			return;
		}
		Debug.Log ("===== show monster ====");
		if (m_gobjModel != null) {
			m_gobjModel.SetActive (one.m_isShowModel);
			return;
		}

		if (!one.m_isShowModel)
			return;
		
		EN_Monster exlMonster = EN_OptMonster.Instance.GetEntity (one.m_iUnqID);
		if (exlMonster == null) {
			Debug.LogWarning ("怪物Excel表中ID= [" + one.m_iUnqID + "],不存在！！");
			return;
		}

		string path = "";
		path = "Assets\\PackResources\\Arts\\Test\\Prefabs\\Monster\\"+ exlMonster.ModeRes + ".prefab";
		bool isExists = File.Exists(path);

		if (!isExists) {
			Debug.LogWarning ("怪物路径path = [" + path + "],不存在！！");

			path = "Assets\\PackResources\\Arts\\Prefabs\\Monster\\"+ exlMonster.ModeRes + ".prefab";

			isExists = File.Exists(path);
			if (!isExists) {
				Debug.LogWarning ("怪物路径path = [" + path + "],不存在！！");
				return;
			}
		}

		GameObject gobj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.GameObject)) as GameObject;
		m_gobjModel = GameObject.Instantiate(gobj, Vector3.zero, Quaternion.identity) as GameObject;
		one.AddChild (m_gobjModel);
		m_gobjModel.SetActive(true);
	}
}
