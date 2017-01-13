using UnityEngine;
using System.Collections;
using LitJson;

/// <summary>
/// 类名 : Editor 下面的时间轴上的伤害处理(打击点检测hitCheck)
/// 作者 : Canyon
/// 日期 : 2017-01-13 09:50
/// 功能 : 
/// </summary>
public class EDT_Hurt : EDT_Base {

	// 优先目标
	public int m_iTargetFilter;

	// 攻击数量
	public int m_iTargetCount;

	public EDT_Hurt():base(){
		this.m_iCurType = 6;
	}
}
