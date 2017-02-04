using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : 绘制 地图元素 - 刷怪点 
/// 作者 : Canyon
/// 日期 : 2017-02-03 18:10
/// 功能 : 
/// </summary>
public class PSM_Monster : PSM_Base<EM_Monster> {

	public PSM_Monster(string title,System.Action callNew,System.Action<EM_Monster> callRemove) : base(title,callNew,callRemove){
	}

	protected override void _DrawOneAttrs (EM_Monster one)
	{
		base._DrawOneAttrs (one);

		EG_GUIHelper.FEG_BeginH();
		{
			GUILayout.Label("怪物ID:", GUILayout.Width(80));
			one.m_iID = EditorGUILayout.IntField (one.m_iID);
		}
		EG_GUIHelper.FEG_EndH();
		EG_GUIHelper.FG_Space(5);

		one.m_v3Pos = EditorGUILayout.Vector3Field ("产生点:", one.m_v3Pos);
		EG_GUIHelper.FG_Space(5);

		one.m_fRotation = EditorGUILayout.FloatField ("初始旋转角度:", one.m_fRotation);
		EG_GUIHelper.FG_Space(5);

		one.m_fReliveInv = EditorGUILayout.FloatField ("刷新时间间隔:", one.m_fReliveInv);
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FG_BeginH ();
		{
			if (GUILayout.Button ("Choose Cell(选中物体)")) {
				Selection.activeTransform = one.m_trsf;
			}
		}
		EG_GUIHelper.FG_EndH ();
		EG_GUIHelper.FG_Space(5);

		EG_GUIHelper.FG_BeginH ();
		{
			if (GUILayout.Button ("SyncToData")) {
				one.ToData ();
			}

			if (GUILayout.Button ("SyncToInspector")) {
				one.ToTrsfData ();
			}
		}
		EG_GUIHelper.FG_EndH ();
		EG_GUIHelper.FG_Space(5);
	}
}
