using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

/// <summary>
/// 类名 : 地图元素 - 触发刷怪区域
/// 作者 : Canyon
/// 日期 : 2017-04-26 16:10
/// 功能 : 用于服务器刷怪
/// </summary>
public class PSM_AreaBornMonster : PSM_Base<EM_AreasBornMonster> {
	
	public PSM_AreaBornMonster(string title,System.Action callNew,System.Action<EM_AreasBornMonster> callRemove) : base(title,callNew,callRemove){
	}

	protected override void _DrawOneAttrs (EM_AreasBornMonster one)
	{
		base._DrawOneAttrs (one);

		one.OnResetColor ();

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("分组ID:", GUILayout.Width(80));
			one.m_iUnqID = EditorGUILayout.IntField (one.m_iUnqID);
			one.CheckUnqIDChange ();
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		one.m_v3Pos = EditorGUILayout.Vector3Field ("中心点:", one.m_v3Pos);
		EG_GUIHelper.FG_Space(5);


		one.m_fRadius = EditorGUILayout.FloatField("半径:", one.m_fRadius);
		EG_GUIHelper.FG_Space(5);

		one.m_isRound = EditorGUILayout.Toggle("是否循环:", one.m_isRound);
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
