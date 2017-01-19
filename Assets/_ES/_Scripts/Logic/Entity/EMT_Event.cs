using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 类名 : 时间事件管理
/// 作者 : Canyon
/// 日期 : 2016-12-27 10:10
/// 功能 : 
/// </summary>
public class EMT_Event{

	EDT_TEvents m_eTEvent = new EDT_TEvents ();

    bool isPause = false;
    float curSpeed = 1.0f;
    //更新间隔
    float m_InvUpdate = 0.05f;
    // 当前值
    float m_CurInvUp = 0.0f;

	#region ====== common func ====

	public void RmEvent(EDT_Base en)
	{
		m_eTEvent.RmEvent (en);
	}

	public void DoReInit(string json){
		m_eTEvent.DoReInit (json);
	}

	public void ResetOwner(Transform trsfOwner){
		m_eTEvent.m_trsfOwner = trsfOwner;
	}

    public void DoStart()
    {
		m_eTEvent.DoStart ();
		isPause = false;
    }
    
    public void OnUpdate(float deltatime, bool isImm = false)
    {
        
        if (!isImm)
        {
            if (isPause)
                return;
        }

        m_CurInvUp += deltatime;
        if (m_CurInvUp < m_InvUpdate)
            return;

        // Debug.Log("== EDM_Particle delta = " + m_CurInvUp);
		m_eTEvent.OnUpdate(m_CurInvUp * curSpeed);
        m_CurInvUp = 0.0f;
    }

	public void OnSceneGUI(){
		m_eTEvent.OnSceneGUI();
	}
    
    public void DoPause()
    {
        isPause = true;
    }

    public void DoResume()
    {
        isPause = false;
    }

    public void SetSpeed(float speed)
    {
        curSpeed = speed;
    }

    public void DoClear()
    {
		m_eTEvent.DoClear ();
        isPause = false;
        curSpeed = 1.0f;
    }

    public void DoEnd()
    {
		m_eTEvent.DoEnd ();
    }

	public string ToJsonString(){
		return m_eTEvent.ToStrJsonData ();
	}

	#endregion

	// 特效
	public void NewEffect()
	{
		m_eTEvent.NewEvent<EDT_Effect>();
	}

	public List<EDT_Effect> m_lEffects
	{
		get
		{
			return m_eTEvent.GetLEffects();
		}
	}

	// 打击点
	public void NewHurt(){
		m_eTEvent.NewEvent<EDT_Hurt>();
	}

	public List<EDT_Hurt> m_lHurts
	{
		get
		{
			return m_eTEvent.GetLHurts ();
		}
	}

	// 音效
	public void NewAudio(){
		m_eTEvent.NewEvent<EDT_Audio>();
	}

	public List<EDT_Audio> m_lAudios
	{
		get
		{
			return m_eTEvent.GetLAudios ();
		}
	}

	// 震屏
	public void NewShake(){
		m_eTEvent.NewEvent<EDT_Shake>();
	}

	public List<EDT_Shake> m_lShakes
	{
		get
		{
			return m_eTEvent.GetLShakes();
		}
	}
}
