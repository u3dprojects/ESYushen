using UnityEngine;
using System.Collections;
using LitJson;

/// <summary>
/// 类名 : 地图元素编辑基础类
/// 作者 : Canyon
/// 日期 : 2017-02-03 17:06
/// 功能 : 主要是添加怪兽刷出信息等
/// </summary>
[System.Serializable]
public class EM_Base : EJ_Base{

	// 对象的唯一标识 计数器
	static int CORE_CURSOR = 0;

	// 前缀
	[System.NonSerialized]
	public string m_sPrefixName = "";

	[System.NonSerialized]
	public string m_sGName;

	[System.NonSerialized]
	public Transform m_trsf;

	[System.NonSerialized]
	public GameObject m_gobj;

	[System.NonSerialized]
	public int m_iGobjInstanceID;

	// 绘制的时候是否打开了视图Foldout
	public bool m_isOpenFoldout;

	int m_iCursorBase = 0;

	// 类型脚本
	[System.NonSerialized]
	public EM_Cell m_csCell;

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

		if (m_gobj != null && org != m_gobj) {
			DoClear ();
		}

		m_gobj = org;
		m_trsf = org.transform;
		EM_Cell csCell = org.GetComponent<EM_Cell> ();
		OnClone (csCell.m_entity);

		OnResetGobjInfo ();

		m_isOpenFoldout = true;
		DoActiveInHierarchy ();
	}

	protected virtual void OnClone(EM_Base org){
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
		ResetCShape ();
	}

	protected virtual void ResetCShape(){
		if (m_gobj != null && m_csCell == null) {
			// 添加一个脚本作为类型判断
			m_csCell = m_gobj.GetComponent<EM_Cell> ();
			if (m_csCell == null) {
				m_csCell = m_gobj.AddComponent<EM_Cell> ();
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
