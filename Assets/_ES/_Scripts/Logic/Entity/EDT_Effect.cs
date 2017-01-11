using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : Editor 下面的时间轴上的特效
/// 作者 : Canyon
/// 日期 : 2017-01-11 14:50
/// 功能 : 
/// </summary>
public class EDT_Effect : EDT_Base {

    // 挂节点
    public int m_iJoint;

    public Transform m_trsfParent;

    // 偏移
    public Vector3 m_v3OffsetPos = Vector3.zero;

    // 旋转
    public Vector3 m_v3EulerAngle = Vector3.zero;

    // 缩放
    public float m_fScale;
    
    // 创建出来的实体对象
    EN_Effect _m_eEffect;

    public EDT_Effect() : base()
    {
    }

    public override void OnInit()
    {
        base.OnInit();

        this.m_iCurType = 1;
        this.m_fScale = 1;
    }

    public override string GetPathByNameType(string objName, int type)
    {
        return base.GetPathByNameType(objName, type);
    }

    protected override bool OnCallEvent()
    {
        if(this.m_objOrg != null && this.m_objOrg is GameObject)
        {
            UnityEngine.GameObject gobj = GameObject.Instantiate(this.m_objOrg, Vector3.zero, Quaternion.identity) as GameObject;
            Transform trsfGobj = gobj.transform;
            if(this.m_trsfParent != null)
            {
                trsfGobj.parent = m_trsfParent;
            }

            trsfGobj.localPosition = m_v3OffsetPos;
            trsfGobj.localEulerAngles = m_v3EulerAngle;
            trsfGobj.localScale = Vector3.one;

            _m_eEffect = new EN_Effect(gobj);
            _m_eEffect.DoStart();
            _m_eEffect.SetScale(m_fScale);
            return true;
        }
        return false;
    }

    protected override void OnCallUpdate(float upDeltaTime)
    {
        if(_m_eEffect != null)
        {
            _m_eEffect.DoUpdate(upDeltaTime);
            this.m_isEnd = _m_eEffect.isEnd;
        }
    }

    public override void OnClear()
    {
        base.OnClear();
        OnClearEffect();
    }

    void OnClearEffect()
    {
        if (_m_eEffect != null)
        {
            _m_eEffect.DoDestory();
            _m_eEffect = null;
        }
    }

    public override void DoEnd()
    {
        base.DoEnd();

        OnClearEffect();
    }
}
