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

	// 对象的唯一标识 计数器
	static int CORE_CURSOR = 0;

	int m_iCoreCursorCube = 0;

	MeshRenderer m_meshRender;
	Material m_matCub;

	// 区域颜色
	// m_cAreaColor = new Color(Random.Range(0f,1.0f),Random.Range(0f,1.0f),Random.Range(0f,1.0f),0.5f);
	public Color m_cAreaColor = Color.blue;

	public EM_Cube():base(){
		m_sPrefixName = "Cube";
		m_iCoreCursorCube = (CORE_CURSOR++);
	}

	protected override void OnNew ()
	{
		m_gobj =  GameObject.CreatePrimitive (PrimitiveType.Cube);
		m_trsf = m_gobj.transform;

		m_meshRender = m_gobj.GetComponent<MeshRenderer> ();
		m_matCub = new Material (Shader.Find ("Diffuse"));
		m_meshRender.material = m_matCub;

		OnResetColor ();
	}

	public void OnResetColor(){
		if (m_matCub != null && m_matCub.HasProperty ("_Color")) {
			m_matCub.SetColor ("_Color", this.m_cAreaColor);
		}
	}

	protected override void OnResetGobjName ()
	{
		if (string.IsNullOrEmpty (m_sGName)) {
			m_sGName = m_sPrefixName + m_iCoreCursorCube;
		}
		ResetGobjName();
	}

	protected override void OnClone (EM_Base org)
	{
		if (org == null)
			return;
		
		base.OnClone (org);

		if (org is EM_Cube) {
			EM_Cube tmp = (EM_Cube)org;
			m_cAreaColor = tmp.m_cAreaColor;

			m_meshRender = m_gobj.GetComponent<MeshRenderer> ();
			m_matCub = new Material (Shader.Find ("Diffuse"));
			m_meshRender.material = m_matCub;

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

	new public static void DoClearStatic ()
	{
		EM_Base.DoClearStatic ();

		CORE_CURSOR = 0;
	}
}
