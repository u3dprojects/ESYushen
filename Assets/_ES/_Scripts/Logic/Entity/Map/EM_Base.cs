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

	public int m_iGobjInstanceID;
	// 绘制的时候是否打开了视图Foldout
	public bool m_isOpenFoldout;

	int m_iCursorBase = 0;

	public EM_Base() : base(){
		m_iCursorBase = (CORE_CURSOR++);
	}

	public void NewOrReset(GameObject org = null){
		if (org == null) {
			DoNew ();
		} else {
			Reset (org);
		}
	}

	public void Reset(GameObject org){
		if (org == null)
			return;
		
		DoClear ();

		m_gobj = org;
		m_trsf = m_gobj.transform;

		OnResetGobjInfo ();

		m_isOpenFoldout = true;
		DoActiveInHierarchy ();
	}

	public void DoNew(){
		OnNew ();

		OnResetGobjInfo ();

		m_isOpenFoldout = true;
		DoActiveInHierarchy ();
	}

	protected virtual void OnNew(){
		m_gobj = new GameObject ();
		m_trsf = m_gobj.transform;
	}

	void OnResetGobjInfo(){
		OnResetGobjName ();
		ResetGobjInstanceID ();

		if (m_gobj) {
			// 添加一个脚本作为类型判断
			EM_Cell m_csMapCell = m_gobj.GetComponent<EM_Cell> ();
			if (m_csMapCell == null) {
				m_csMapCell = m_gobj.AddComponent<EM_Cell> ();
			}
		}
	}

	void ResetGobjInstanceID(){
		if (this.m_gobj) {
			this.m_iGobjInstanceID = this.m_gobj.GetInstanceID ();
		}
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

	public virtual void OnChangePosition(Transform trsf){
	}

	public virtual void OnChangeRotation(Transform trsf){
	}

	public void DoActiveInHierarchy(){
		if (m_gobj) {
			#if UNITY_EDITOR
			UnityEditor.Selection.activeGameObject = m_gobj;
			// UnityEditor.Selection.activeTransform = m_trsf;
			UnityEditor.SceneView.FrameLastActiveSceneView();
			#endif
		}
	}

	public static void DoClearStatic ()
	{
		CORE_CURSOR = 0;
	}
}
