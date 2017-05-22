using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : Map 在 windows的视图
/// 作者 : Canyon
/// 日期 : 2017-05-22 17:10
/// 功能 : 扩展专门针对游戏特殊扩展
/// </summary>
public partial class EG_Map {
	/// <summary>
	/// 视图变化扩展类
	/// </summary>
	/// <param name="instanceID">Instance I.</param>
	/// <param name="types">Types.</param>
	void OnChangeGobjInHierarchyGUI (int instanceID, int types){
		switch (types) {
		case 1:
			// 新增函数
			break;
		case 2:
			// 移除函数
			break;
		}
	}
}
