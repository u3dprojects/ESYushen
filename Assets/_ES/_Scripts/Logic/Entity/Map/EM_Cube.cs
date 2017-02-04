using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : 地图基础类之方块
/// 作者 : Canyon
/// 日期 : 2017-02-03 17:06
/// 功能 : 主要是添加怪兽刷出信息等
/// </summary>
public class EM_Cube : EM_Base{

	// 对象的唯一标识 计数器
	static int CORE_CURSOR = 0;

	int m_iCoreCursorCube = 0;

	protected Material m_matCub;

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

		MeshRenderer mRender = m_gobj.GetComponent<MeshRenderer> ();
		m_matCub = new Material (Shader.Find ("Diffuse"));
		if (m_matCub.HasProperty ("_Color")) {
			m_matCub.SetColor ("_Color", this.m_cAreaColor);
		}
		mRender.material = m_matCub;
	}

	protected override void OnResetGobjName ()
	{
		if (string.IsNullOrEmpty (m_sGName)) {
			m_sGName = m_sPrefixName + m_iCoreCursorCube;
		}
		ResetGobjName();
	}

	public static void DoClearStatic ()
	{
		EM_Base.DoClearStatic ();

		CORE_CURSOR = 0;
	}
}
