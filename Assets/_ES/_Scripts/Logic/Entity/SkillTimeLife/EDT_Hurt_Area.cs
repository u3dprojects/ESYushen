using UnityEngine;
using System.Collections;
using System.ComponentModel;
using LitJson;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 类名 : 伤害区域 hit Area
/// 作者 : Canyon
/// 日期 : 2017-01-13 10:30
/// 功能 : 
/// </summary>
public class EDT_Hurt_Area : EDT_Base{
	
	public enum HurtAreaType{
		[Description("无")]
		None = 0,

		[Description("圆形")]
		Circle = 1,

		[Description("弧形")]
		Arc = 2,

		[Description("矩形")]
		Rectangle = 3,
	}

	// 类型
	public HurtAreaType m_emType = HurtAreaType.None;

	// 相对偏移(相对于人物的位置偏移的位置) 就是产生点的位置
	public Vector3 m_v3Offset = Vector3.zero;

	// 伤害范围,最远的一边(圆，弧表示半径,矩形就是长边)
	public float m_fRange;

	// 矩形宽
	public float m_fWidth;

	// 逆时针旋转角度偏移量,就是y轴值
	public float m_fRotation;

	// 角度[0~360] 360表示圆,0就不绘制
	public float m_fAngle;

	// 区域颜色
	public Color m_cAreaColor = new Color(Random.Range(0f,1.0f),Random.Range(0f,1.0f),Random.Range(0f,1.0f),0.15f);

	// 是否绘制伤害区域
	public bool m_isShowArea;

	public EDT_Hurt_Area() : base(){
		m_emType = HurtAreaType.None;
	}

	public override void OnReInit (float castTime, JsonData jsonData)
	{
		base.OnReInit (castTime, jsonData);

		int tpId = (int)jsonData ["m_id"];
		this.m_emType = (HurtAreaType)tpId;

		float x = float.Parse(jsonData ["m_offsetX"].ToString());
		float z = float.Parse(jsonData ["m_offsetZ"].ToString());
		this.m_v3Offset = new Vector3 (x, 0, z);

		IDictionary dicJsonData = (IDictionary)jsonData;

		if (dicJsonData.Contains("m_r")) {
			this.m_fRange = float.Parse (jsonData ["m_r"].ToString ());
		}

		if (dicJsonData.Contains("m_len")) {
			this.m_fRange = float.Parse (jsonData ["m_len"].ToString ());
		}

		if (dicJsonData.Contains("m_rad")) {
			this.m_fAngle = float.Parse (jsonData ["m_rad"].ToString ());
		}

		if (dicJsonData.Contains("m_rot")) {
			this.m_fRotation = float.Parse (jsonData ["m_rot"].ToString ());
		}

		if (dicJsonData.Contains("m_width")) {
			this.m_fWidth = float.Parse (jsonData ["m_width"].ToString ());
		}

		this.m_isJsonDataToSelfSuccessed = true;
		this.m_isInitedFab = true;
	}

	public override JsonData ToJsonData (){
		if (this.m_fRange <= 0 || m_emType == HurtAreaType.None) {
			return null;
		}

		if (m_emType == HurtAreaType.Rectangle) {
			if (this.m_fWidth <= 0) {
				return null;
			}
		} else if (m_emType == HurtAreaType.Arc) {
			if (this.m_fAngle <= 0 || this.m_fAngle > 360) {
				return null;
			}
		}

		JsonData ret = new JsonData ();
		ret ["m_id"] = (int)this.m_emType;
		ret ["m_offsetX"] = EDT_Hurt.Round2D(m_v3Offset.x,2);
		ret ["m_offsetZ"] = EDT_Hurt.Round2D(m_v3Offset.z,2);

		switch (m_emType) {
		case HurtAreaType.Arc:
			ret ["m_r"] = EDT_Hurt.Round2D(m_fRange,2);
			ret ["m_rad"] = EDT_Hurt.Round2D(m_fAngle,2);
			ret ["m_rot"] = EDT_Hurt.Round2D(m_fRotation,2);
			break;
		case HurtAreaType.Circle:
			ret ["m_r"] = EDT_Hurt.Round2D(m_fRange,2);
			break;
		case HurtAreaType.Rectangle:
			ret ["m_len"] = EDT_Hurt.Round2D(m_fRange,2);
			ret ["m_width"] = EDT_Hurt.Round2D(m_fWidth,2);
			ret ["m_rot"] = EDT_Hurt.Round2D(m_fRotation,2);
			break;
		}
		return ret;
	}

	public override void OnSceneGUI (Transform trsfOrg)
	{
		base.OnSceneGUI (trsfOrg);

		DrawAreaInSceneView (trsfOrg);
	}

	// 在区域里面绘制
	void DrawAreaInSceneView(Transform trsfOrg){
		if (!m_isShowArea) {
			return;
		}

		#if UNITY_EDITOR
		if(trsfOrg == null){
			return;
		}

		if (this.m_fRange <= 0 || m_emType == HurtAreaType.None) {
			return;
		}

		if (m_emType == HurtAreaType.Rectangle) {
			if (this.m_fWidth <= 0) {
				return;
			}
		} else if (m_emType == HurtAreaType.Arc) {
			if (this.m_fAngle <= 0 || this.m_fAngle > 360) {
				return;
			}
		}

		Handles.color = this.m_cAreaColor;

		Vector3 posOrg = trsfOrg.position;
		Vector3 dirOrg = trsfOrg.forward;


		Vector3 pos = posOrg + this.m_v3Offset;
		pos.y = posOrg.y;

		Quaternion quaternion = Quaternion.AngleAxis(m_fRotation,Vector3.up);
		Vector3 dir = (quaternion * dirOrg).normalized;

		switch (m_emType) {
		case HurtAreaType.Arc:
			dir = (Quaternion.AngleAxis(-(m_fAngle / 2),Vector3.up) * dir).normalized;
			Handles.DrawSolidArc(pos,Vector3.up,dir,this.m_fAngle,this.m_fRange);
			break;
		case HurtAreaType.Circle:
			 Handles.DrawSolidDisc(pos,Vector3.up,this.m_fRange);
			break;
		case HurtAreaType.Rectangle:
			float hfw = this.m_fWidth / 2;
			float hfl = this.m_fRange / 2;
			float hfr = Mathf.Sqrt(Mathf.Pow(this.m_fWidth,2)+Mathf.Pow(this.m_fRange,2));
			pos = posOrg;

			Vector3 pos01 = new Vector3(pos.x - hfw,0,pos.z - hfl);
			Vector3 pos1 = quaternion * pos01.normalized * hfr + this.m_v3Offset;
			pos1.y = pos.y;

			Vector3 pos02 = new Vector3(pos.x - hfw,0,pos.z + hfl);
			Vector3 pos2 = quaternion * pos02.normalized * hfr + this.m_v3Offset;
			pos2.y = pos.y;

			Vector3 pos03 = new Vector3(pos.x + hfw,0,pos.z + hfl);
			Vector3 pos3 = quaternion * pos03.normalized * hfr + this.m_v3Offset;
			pos3.y = pos.y;

			Vector3 pos04 = new Vector3(pos.x + hfw,0,pos.z - hfl);
			Vector3 pos4 = quaternion * pos04.normalized * hfr + this.m_v3Offset;
			pos4.y = pos.y;

			Vector3[] verts = new Vector3[] { 
				pos1,pos2,pos3,pos4
			};
			Handles.DrawSolidRectangleWithOutline(verts,this.m_cAreaColor, new Color( 0, 0, 0, 1 ));
			break;
		}
		Handles.color = Color.white;
		#endif
	}
}
