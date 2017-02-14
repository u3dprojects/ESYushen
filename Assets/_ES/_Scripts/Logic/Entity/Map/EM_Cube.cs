using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : 地图基础类之方块
/// 作者 : Canyon
/// 日期 : 2017-02-03 17:06
/// 功能 : 主要是添加怪兽刷出信息等
/// </summary>
[System.Serializable]
public class EM_Cube : EM_Base{
	
	protected MeshRenderer m_meshRender;
	Material m_matCub;

	// 区域颜色
	// m_cAreaColor = new Color(Random.Range(0f,1.0f),Random.Range(0f,1.0f),Random.Range(0f,1.0f),0.5f);
	public Color m_cAreaColor = Color.blue;

	public EM_Cube():base(){
	}

	protected override void OnNew ()
	{
		m_gobj =  GameObject.CreatePrimitive (PrimitiveType.Cube);
		m_trsf = m_gobj.transform;

		ResetMeshMaterial ();

		OnResetColor ();
	}

	public void OnResetColor(){
		if (m_matCub != null && m_matCub.HasProperty ("_Color")) {
			m_matCub.SetColor ("_Color", this.m_cAreaColor);
		}
	}

	void ResetMeshMaterial(){
		m_meshRender = m_gobj.GetComponent<MeshRenderer> ();
		if (m_meshRender != null) {
			m_matCub = new Material (Shader.Find ("Diffuse"));
			m_meshRender.material = m_matCub;
		}
	}

	protected override void OnCloneData (EM_Base org)
	{
		if (org == null)
			return;
		
		base.OnCloneData (org);

		if (org is EM_Cube) {
			EM_Cube tmp = (EM_Cube)org;
			m_cAreaColor = tmp.m_cAreaColor;

			ResetMeshMaterial ();

			OnResetColor ();
		}
	}

	protected override void OnClear ()
	{
		base.OnClear ();
		if (m_meshRender != null) {
			m_meshRender.material = null;
			m_meshRender = null;
		}

		if (m_matCub != null) {
			Material.DestroyImmediate (m_matCub);
			m_matCub = null;
		}
	}
}
