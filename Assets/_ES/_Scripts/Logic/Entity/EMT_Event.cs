using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

/// <summary>
/// 类名 : 技能时间事件管理
/// 作者 : Canyon
/// 日期 : 2016-12-27 10:10
/// 功能 : 
/// </summary>
public class EMT_Event : EMT_HitEvent{
	
    bool isPause = false;

    float curSpeed = 1.0f;

    //更新间隔
    float m_InvUpdate = 0.05f;

    // 当前值
    float m_CurInvUp = 0.0f;

	#region ====== common func ====

	public void DoReInit(string json){
		DoClear();
		DoInit (json);
	}

	// 数据解析
	public void DoInit(string json){
		JsonData m_jsonData = JsonMapper.ToObject (json);
		if(!m_jsonData.IsArray){
			return;
		}

		int lens = m_jsonData.Count;
		JsonData tmpJsonData = null;
		float casttime = 0.0f;
		for (int i = 0; i < lens; i++) {
			tmpJsonData = m_jsonData [i];

			if (!tmpJsonData.IsObject)
				continue;

			casttime = float.Parse(tmpJsonData ["m_timing"].ToString());
			tmpJsonData = tmpJsonData ["m_castEvts"];
			DoInit(tmpJsonData, casttime);
		}
	}

	public override void DoStart()
    {
		base.DoStart ();
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
		DoUpdate(m_CurInvUp * curSpeed);
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

	public override void DoClear()
    {
		base.DoClear ();
        isPause = false;
        curSpeed = 1.0f;
    }

	public override string ToJsonString ()
	{
		return ToJsonStrCast ();
	}
	#endregion
}
