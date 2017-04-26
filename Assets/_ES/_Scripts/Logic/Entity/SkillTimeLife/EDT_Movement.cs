using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;

/// <summary>
/// 类名 : Editor 下面的时间轴上的受击者的表现形式(无，退，飞，倒，晕)
/// 作者 : Canyon
/// 日期 : 2017-01-24 10:35
/// 功能 : 
/// </summary>
public class EDT_Movement : EDT_Base {

	// 0代表可以距离不可变, 1代表距离可变
	public int m_iMvType = 0;

	// 速度
	public float m_fSpeed = 0;

	// 持续时间
	public float m_fDuration = 0;

	// 移动
	public Vector3 m_v3Movement = Vector3.zero;

	// 受影响的对象
	public Transform m_trsfAffected = null;

	public EDT_Movement():base(){
	}

	public override void OnClear ()
	{
		base.OnClear ();
		m_trsfAffected = null;
	}

	protected override bool OnCallEvent ()
	{
		if (m_trsfAffected != null) {
			if (m_iMvType == 1) {
				m_v3Movement = m_trsfAffected.forward;
			} else {
				m_v3Movement = m_trsfAffected.position + Vector3.left;
			}
			m_v3Movement = m_v3Movement.normalized;
		} else {
			m_v3Movement = Vector3.zero;
		}
		return base.OnCallEvent ();
	}

	protected override void OnCallUpdate (float upDeltaTime)
	{
		base.OnCallUpdate (upDeltaTime);
		if (m_trsfAffected != null && m_v3Movement != Vector3.zero) {
			float speed = m_fSpeed * upDeltaTime;
			Vector3 finalMovement = m_v3Movement * speed;
			m_trsfAffected.Translate(finalMovement);
		}
	}

	public string ToFmtTpValStr(){
		float distance = m_fDuration * m_fSpeed;
		float begtime = m_fCastTime;
		float endtime = m_fCastTime + m_fDuration;
		return m_iMvType + "," + distance+ "," + begtime+ "," + endtime;
	}

	/// <summary>
	/// 格式 tp,distance,begtime,endtime
	/// </summary>
	/// <returns>The by.</returns>
	/// <param name="notJson">Not json.</param>
	static public EDT_Movement Parse(string notJson){
		string[] arrs = notJson.Split (",".ToCharArray (), System.StringSplitOptions.RemoveEmptyEntries);
		if (arrs.Length < 4)
			return null;

		try {
			int tp = int.Parse(arrs[0]);
			if(tp == 0)
				return null;
			
			float distance = float.Parse(arrs[1]);
			float begtime = float.Parse(arrs[2]);
			float endtime = float.Parse(arrs[3]);
			float revtime = endtime - begtime;

			if(revtime <= 0 || distance <= 0)
				return null;

			EDT_Movement ret = new EDT_Movement();
			ret.m_fCastTime = begtime;
			ret.m_iMvType = tp;
			ret.m_fDuration = revtime;
			ret.m_fSpeed = distance / revtime;

			return ret;
		} catch (System.Exception ex) {
		}
		return null;
	}

	/// <summary>
	/// 转为字符串格式: tp,distance,begtime,endtime;tp,distance,begtime,endtime
	/// </summary>
	/// <returns>The string fmt.</returns>
	/// <param name="list">List.</param>
	static public string ToStrFmt(List<EDT_Movement> list){
		if (list == null || list.Count <= 0)
			return "";

		System.Text.StringBuilder builder = new System.Text.StringBuilder ();

		EDT_Movement temp = null;
		int lens = list.Count;
		for (int i = 0; i < lens; i++) {
			temp = list [i];
			builder.Append (temp.ToFmtTpValStr ());
			if(i < lens - 1)
				builder.Append (";");
		}
		return builder.ToString ();
	}
}
