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

	public EM_Base() : base(){
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


		OnClone (org);

		OnResetGobjInfo ();

		m_isOpenFoldout = true;
		DoActiveInHierarchy ();
	}

	protected virtual bool OnClone(GameObject org){
		if (org == null)
			return false;
		m_gobj = org;
		m_trsf = org.transform;
		return true;
	}

	protected virtual void OnCloneData(EM_Base org){
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
	}

	void ResetGobjInstanceID(){
		if (this.m_gobj) {
			this.m_iGobjInstanceID = this.m_gobj.GetInstanceID ();
		}
	}

	protected virtual void OnResetGobjName(){
		SetGobjName ("Base");
	}

	public void SetGobjName(string name){
		this.m_sGName = name;
		if (m_gobj) {
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

	public void OnCloneTrsfInfo(Transform trsf){
		if (trsf != null && m_trsf != null) {
			m_trsf.position = trsf.position;
			m_trsf.eulerAngles = trsf.eulerAngles;
		}
	}

	public virtual void OnChangeTransform(Transform trsf){
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

	public void DoDestroyChild(){
		if (m_trsf) {
			GameObject gobjChild;
			while (true) {
				if (m_trsf.childCount <= 0)
					break;
				
				gobjChild = m_trsf.GetChild (0).gameObject;
				GameObject.DestroyImmediate(gobjChild);
			}
			m_trsf.DetachChildren ();
		}

		OnDestroyChild ();
	}

	protected virtual void OnDestroyChild(){}

	public void AddChild(GameObject child,bool isReset = true){
		if (child != null && m_trsf != null) {
			Transform trsf = child.transform;
			trsf.parent = m_trsf;
			if (isReset) {
				trsf.localPosition = Vector3.zero;
				trsf.localEulerAngles = Vector3.zero;
				trsf.localScale = Vector3.one;
			}
		}
	}

	/// <summary>
	/// 在Scene窗口中绘制回调
	/// </summary>
	public virtual void OnSceneGUI(){
	}
}
