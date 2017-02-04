using UnityEngine;
using System.Collections;
using LitJson;

/// <summary>
/// 类名 : 地图元素编辑基础类
/// 作者 : Canyon
/// 日期 : 2017-02-03 17:06
/// 功能 : 主要是添加怪兽刷出信息等
/// </summary>
public class EM_Base : EJ_Base{

	// 对象的唯一标识 计数器
	static int CORE_CURSOR = 0;

	// 前缀
	public string m_sPrefixName = "";
	public string m_sGName;
	public Transform m_trsf;
	public GameObject m_gobj;

	int m_iCursorBase = 0;

	public EM_Base() : base(){
		m_iCursorBase = (CORE_CURSOR++);
	}

	public void DoNew(){
		OnNew ();
		OnResetGobjName ();
	}

	protected virtual void OnNew(){
		m_gobj = new GameObject ();
		m_trsf = m_gobj.transform;
	}

	protected virtual void OnResetGobjName(){
		ResetGobjName ();
	}

	protected void ResetGobjName(){
		if (m_gobj) {
			if (string.IsNullOrEmpty (m_sGName)) {
				m_sGName = m_sPrefixName + m_iCursorBase;
			}
			m_gobj.name = this.m_sGName;
		}
	}

	protected override void OnClear ()
	{
		base.OnClear ();

		if (m_gobj != null) {
			GameObject.DestroyImmediate (m_gobj);
		}
		m_trsf = null;
		m_gobj = null;
	}

	public static void DoClearStatic ()
	{
		CORE_CURSOR = 0;
	}
}
