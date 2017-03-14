using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

/// <summary>
/// 类名 : 绘制 地图元素 - 怪物聚集中心点
/// 作者 : Canyon
/// 日期 : 2017-03-14 11:10
/// 功能 : 用于UI缩略图展示上面用
/// </summary>
public class PSM_MonsterCenter : PSM_Base<EM_MonsterCenter> {
	
	public PSM_MonsterCenter(string title,System.Action callNew,System.Action<EM_MonsterCenter> callRemove) : base(title,callNew,callRemove){
	}

	protected override void _DrawOneAttrs (EM_MonsterCenter one)
	{
		base._DrawOneAttrs (one);

		one.OnResetColor ();

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("怪物ID:", GUILayout.Width(80));
			one.m_iUnqID = EditorGUILayout.IntField (one.m_iUnqID);
			one.CheckUnqIDChange ();
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		one.m_iLevel = EditorGUILayout.IntField ("怪物等级:", one.m_iLevel);
		EG_GUIHelper.FG_Space(5);

		one.m_v3Pos = EditorGUILayout.Vector3Field ("聚集中心点:", one.m_v3Pos);
		EG_GUIHelper.FG_Space(5);


		EG_GUIHelper.FG_BeginH ();
		{
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
}
