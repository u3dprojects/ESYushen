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
    List<EDT_Base> m_lEvent = new List<EDT_Base>();
    EDT_Base m_tmpEvent;

    int lens = 0;
    bool isPause = false;
    float curSpeed = 1.0f;
    //更新间隔
    float m_InvUpdate = 0.05f;
    // 当前值
    float m_CurInvUp = 0.0f;

    public T NewEvent<T>() where T : EDT_Base,new()
    {
        T ret = new T();
        m_lEvent.Add(ret);
        return ret;
    }

    public void RmEvent(EDT_Base en)
    {
        if (en == null)
            return;
        en.DoRemove();
    }

    public List<T> GetList<T>() where T : EDT_Base
    {
        List<T> ret = new List<T>();
        lens = m_lEvent.Count;
        for (int i = 0; i < lens; i++)
        {
            m_tmpEvent = m_lEvent[i];
            if(m_tmpEvent is T)
            {
                ret.Add((T)m_tmpEvent);
            }
        }
        return ret;
    }

    public void NewEffect()
    {
        NewEvent<EDT_Effect>();
    }

    public List<EDT_Effect> m_lEffects
    {
        get
        {
            return GetList<EDT_Effect>();
        }
    }

    public void DoStart()
    {
        lens = m_lEvent.Count;
        for (int i = 0; i < lens; i++)
        {
            m_tmpEvent = m_lEvent[i];
            m_tmpEvent.DoStart(true);
        }
        m_tmpEvent = null;
    }
    
    public void OnUpdate(float deltatime, bool isImm = false)
    {
        lens = m_lEvent.Count;
        if (lens <= 0)
            return;

        if (!isImm)
        {
            if (isPause)
                return;
        }

        m_CurInvUp += deltatime;
        if (m_CurInvUp < m_InvUpdate)
            return;

        // Debug.Log("== EDM_Particle delta = " + m_CurInvUp);

        for (int i = 0; i < lens; i++)
        {
            m_tmpEvent = m_lEvent[i];
            m_tmpEvent.DoUpdate(m_CurInvUp * curSpeed);
            if (m_tmpEvent.m_isEnd)
            {
                m_tmpEvent.DoEnd();
            }
        }

        m_CurInvUp = 0.0f;
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
        m_lEvent.Clear();
        
        m_tmpEvent = null;
        isPause = false;
        curSpeed = 1.0f;
    }

    public void DoEnd()
    {
        lens = m_lEvent.Count;
        for (int i = 0; i < lens; i++)
        {
            m_tmpEvent = m_lEvent[i];
            m_tmpEvent.DoEnd();
        }
        m_tmpEvent = null;
    }
}
