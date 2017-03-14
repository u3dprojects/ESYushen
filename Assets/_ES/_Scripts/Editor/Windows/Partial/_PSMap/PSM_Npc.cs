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
public class PSM_Npc : PSM_Base<EM_NPC> {
	
	public PSM_Npc(string title,System.Action callNew,System.Action<EM_NPC> callRemove) : base(title,callNew,callRemove){
	}

	protected override void _DrawOneAttrs (EM_NPC one)
	{
		base._DrawOneAttrs (one);

		one.OnResetColor ();

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("NpcID:", GUILayout.Width(80));
			one.m_iUnqID = EditorGUILayout.IntField (one.m_iUnqID);
			one.CheckUnqIDChange ();
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		one.m_v3Pos = EditorGUILayout.Vector3Field ("产生点:", one.m_v3Pos);
		EG_GUIHelper.FG_Space(5);

		one.m_fRotation = EditorGUILayout.FloatField ("初始旋转角度:", one.m_fRotation);
		EG_GUIHelper.FG_Space(5);

		one.m_iTalkId = EditorGUILayout.IntField ("对话ID <--> 语言表中ID",one.m_iTalkId);
		EG_GUIHelper.FG_Space(5);

//		one.m_fReliveInv = EditorGUILayout.FloatField ("刷新时间间隔:", one.m_fReliveInv);
//		EG_GUIHelper.FG_Space(5);

//		EG_GUIHelper.FEG_BeginToggleGroup ("是否显示怪物模型", ref one.m_isShowModel);
//		{
//			if (!EN_OptMonster.Instance.isInitSuccessed) {
//				EditorGUILayout.LabelField ("未选择怪物Excel,不能显示模型");
//			}
//		}
//		EG_GUIHelper.FEG_EndToggleGroup ();
//		_ShowMonster (one);

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

	void _ShowMonster(EM_NPC one){
		if (!EN_OptMonster.Instance.isInitSuccessed) {
			return;
		}

		if (one.m_gobjModel != null) {
			one.ModelActiveStatus ();
			return;
		}

		if (!one.m_isShowModel) {
			return;
		}

		EN_Monster exlMonster = EN_OptMonster.Instance.GetEntity (one.m_iUnqID);
		if (exlMonster == null) {
			Debug.LogWarning ("怪物Excel表中ID= [" + one.m_iUnqID + "],不存在！！");
			return;
		}

		string path = "";
		path = "Assets\\PackResources\\Arts\\Test\\Prefabs\\Monster\\"+ exlMonster.ModeRes + ".prefab";
		bool isExists = File.Exists(path);

		if (!isExists) {
			// Debug.LogWarning ("怪物路径path = [" + path + "],不存在！！");

			path = "Assets\\PackResources\\Arts\\Prefabs\\Monster\\"+ exlMonster.ModeRes + ".prefab";

			isExists = File.Exists(path);

			if (!isExists) {
				path = "Assets\\PackResources\\Arts\\Sprites\\Prefabs\\"+ exlMonster.ModeRes + ".prefab";

				isExists = File.Exists(path);

				if (!isExists) {
					Debug.LogWarning ("怪物路径path = [" + path + "],不存在！！");
					return;
				}
			}
		}

		GameObject gobj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.GameObject)) as GameObject;
		gobj = GameObject.Instantiate(gobj, Vector3.zero, Quaternion.identity) as GameObject;
		EUM_Npc em = gobj.GetComponent<EUM_Npc> ();
		if (em != null) {
			GameObject.DestroyImmediate (em);
		}

		one.AddModel (gobj);
	}
}
