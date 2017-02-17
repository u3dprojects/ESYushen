﻿using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : 枚举多选绘制
/// 作者 : Canyon
/// 日期 : 2017-02-17 17:35
/// 功能 : 
/// </summary>
[CustomPropertyDrawer(typeof(MultiEnumFlagsAttribute))]
public class MultiEnumFlagsAttributeDrawer : PropertyDrawer {
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		/*
         * 绘制多值枚举选择框,0 全部不选, -1 全部选中, 其他是枚举之和
         * 枚举值 = 当前下标值 ^ 2
         * 默认[0^2 = 1 , 1 ^2 = 2,  4, 16 , .....]
         */
		property.intValue = EditorGUI.MaskField (position, label, property.intValue, property.enumNames);
		// base.OnGUI (position, property, label);
		Debug.Log("图层的值:" + property.intValue);
	}
}
