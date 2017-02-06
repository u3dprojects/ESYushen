using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 类名 : 绘制 地图元素-父类
/// 作者 : Canyon
/// 日期 : 2017-02-03 17:50
/// 功能 : 
/// </summary>
public class PSM_Base<T>  where T : EM_Base{
	System.Action m_callNew;
	System.Action<T> m_callRemove;
	List<T> m_list;
	string m_title;
	List<bool> m_lFodeout = new List<bool>();

	public PSM_Base(string title,System.Action callNew,System.Action<T> callRemove){
		this.m_title = title;
		this.m_callNew = callNew;
		this.m_callRemove = callRemove;
	}

	public void DoDraw(List<T> list){
		this.m_list = list;
		_DrawUnitCells ();
	}

	void _DrawUnitCells(){
		EG_GUIHelper.FG_BeginVAsArea();
		{
			{
				// 上
				EG_GUIHelper.FEG_Head(m_title,true,this.m_callNew);
			}

			{
				// 中
				int lens = m_list.Count;
				if (lens > 0) {
					T temp = null;
					for (int i = 0; i < lens; i++) {
						temp = m_list [i];
						if (m_lFodeout.Count <= i) {
							m_lFodeout.Add (temp.m_isOpenFoldout);
						}
						_DrawOneCell (i, temp);
					}
				} else {
					m_lFodeout.Clear();
				}
			}
		}
		EG_GUIHelper.FG_EndV();
	}

	void _DrawOneCell(int index, T one)
	{
		
		EG_GUIHelper.FEG_BeginV();
		{
			EG_GUIHelper.FEG_BeginH();
			{
				m_lFodeout[index] = EditorGUILayout.Foldout(m_lFodeout[index], "元素 - " + one.m_sGName);

				one.m_isOpenFoldout = m_lFodeout [index];

				GUI.color = Color.red;
				if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(50)))
				{
					if (this.m_callRemove != null) {
						this.m_callRemove (one);
					}
					m_lFodeout.RemoveAt(index);
				}
				GUI.color = Color.white;
			}
			EG_GUIHelper.FEG_EndH();

			EG_GUIHelper.FG_Space(5);

			if (m_lFodeout.Count > index && m_lFodeout[index])
			{
				_DrawOneView(one);
			}
		}
		EG_GUIHelper.FEG_EndV();
	}

	void _DrawOneView(T one){
		EG_GUIHelper.FEG_BeginVArea ();
		_DrawOneAttrs (one);
		EG_GUIHelper.FEG_EndV ();
	}

	protected virtual void _DrawOneAttrs(T one)
	{
		
	}
}
