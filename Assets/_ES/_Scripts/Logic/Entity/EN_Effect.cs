using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 类名 : U3D - 特效管理
/// 作者 : Canyon
/// 日期 : 2017-01-04 11:10
/// 功能 :  特效包含粒子和Animator
/// </summary>
[System.Serializable]
public class EN_Effect : System.Object
{
    // 多个粒子特效
    EN_Particle m_particle = new EN_Particle();

    // 多个Animator
    List<ED_Ani> list = new List<ED_Ani>();
    int lens = 0;

    public float curSpeed { get; set; }

    float up_deltatime = 0.0f;
	float timeOut = 0;

    public bool isEnd
    {
        get
        {
            return m_particle.isEnd && IsEndAnimator();
        }
    }

    public EN_Effect() { }

	public EN_Effect(GameObject gobj)
    {
        DoReInit(gobj);
    }

	public EN_Effect(GameObject gobj,float timeOut)
	{
		DoReInit(gobj,timeOut);
	}

	public void DoReInit(GameObject gobj,float timeOut = 0)
    {
        DoClear();
		DoInit(gobj,timeOut);
    }

	void DoInit(GameObject gobj,float timeOut = 0)
    {
		this.timeOut = timeOut;
        m_particle.DoReInit(gobj);

        Animator[] anis = gobj.GetComponentsInChildren<Animator>();
        lens = anis.Length;
        ED_Ani tmp = null;
        for (int i = 0; i < lens; i++)
        {
            tmp = new ED_Ani(anis[i]);
            tmp.ResetAniState(0);
            if(tmp.CurState == null)
            {
                continue;
            }

            list.Add(tmp);
        }
    }

    public void DoClear()
    {
        curSpeed = 1;
		timeOut = 0;
        m_particle.DoClear();
        OnClearAnimator();
    }

    void OnClearAnimator()
    {
        lens = list.Count;
        for(int i = 0;i < lens; i++)
        {
            (list[i]).DoClear();
        }
        list.Clear();
        lens = 0;
    }

    public void DoStart()
    {
		m_particle.DoStart(this.timeOut);
        lens = list.Count;
        for (int i = 0; i < lens; i++)
        {
            (list[i]).DoStart();
        }
    }

    public void DoUpdate(float deltatime)
    {
        up_deltatime = deltatime * curSpeed;
        lens = list.Count;
        // Debug.Log("== DBU3D_Effect delta = " + up_deltatime);
        for (int i = 0; i < lens; i++)
        {
            (list[i]).DoUpdateAnimator(up_deltatime,1);
        }
        m_particle.DoUpdate(up_deltatime);
    }

    public void SetScale(float _scale)
    {
        m_particle.SetScale(_scale);
    }

    public bool IsEndAnimator()
    {
        lens = list.Count;
        for (int i = 0; i < lens; i++)
        {
            if(!(list[i]).isEndFirst)
            {
                return false;
            }
        }
        return true;
    }

    public void DoDestory()
    {
        m_particle.DoDestory();
        DoClear();
    }
}
