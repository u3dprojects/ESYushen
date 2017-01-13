using UnityEngine;
using System.Collections;
using System.ComponentModel;
using LitJson;

/// <summary>
/// 类名 : 伤害区域 hit Area
/// 作者 : Canyon
/// 日期 : 2017-01-13 10:30
/// 功能 : 
/// </summary>
public class EDT_Hurt_Area {
	
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
	public HurtAreaType m_emType = HurtAreaType.Circle;

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

	public JsonData ToJsonData(){
		if (this.m_fRange <= 0 || m_emType == HurtAreaType.None) {
			return null;
		}

		if (m_emType == HurtAreaType.Rectangle) {
			if (this.m_fWidth <= 0) {
				return null;
			}
		} else if (m_emType == HurtAreaType.Arc) {
			if (this.m_fAngle <= 0) {
				return null;
			}
		}

		JsonData ret = new JsonData ();
		ret ["m_id"] = (int)this.m_emType;
		ret ["m_offsetX"] = EDT_Hurt.Round2D(m_v3Offset.x,2);
		ret ["m_offsetZ"] = EDT_Hurt.Round2D(m_v3Offset.z,2);

		return ret;
	}
}
