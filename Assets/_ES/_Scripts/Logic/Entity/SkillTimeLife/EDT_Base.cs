using UnityEngine;
using System.Collections;
using System.IO;
using System.ComponentModel;
using LitJson;

/// <summary>
/// 类名 : Editor 的时间轴上的Event实体基类
/// 作者 : Canyon
/// 日期 : 2017-01-11 14:06
/// 功能 : 编辑器模式处理一个创建物体事件外，还要需要一个驱动动作特效函数，
/// </summary>
public class EDT_Base {

	public enum EventType
	{
		[Description("无")]
		None = 0,

		[Description("特效")]
		Effect = 1,

		[Description("音效")]
		Audio = 2,

		[Description("震屏")]
		Shake = 3,

		[Description("停顿")]
		Stay = 4,

		[Description("直接命中目标")]
		HitTarget = 5,

		[Description("区域内伤害")]
		HitArea = 6,

		[Description("修改属性-永久")]
		Property = 7,

		[Description("修改属性-短时")]
		Attribute = 8,

		[Description("Buff")]
		Buff = 9,

		[Description("子弹")]
		Bullet = 10,

		[Description("召唤/分身")]
		Summon = 11,

		[Description("受击状态-默认")]
		BeHitDefault = 14,

		[Description("被击退")]
		BeHitBack = 15,

		[Description("被击飞")]
		BeHitFly = 16,
	}

    // 对象的唯一标识 计数器
    static int EVENT_CORE_CURSOR = 0;

	// 保留小数
	static public double Round2D(float org, int acc)
	{
		double pow = Mathf.Pow(10, acc);
		double temp = org * pow;
		return Mathf.RoundToInt((float)temp) / pow;
	}


	// 当前事件类型
	// public int m_iCurType = -1;
	public EventType m_emType = EventType.None;

    // 自身唯一标识
    public int m_iCurID;

    // 触发时间
    public float m_fCastTime;
    public string m_sName;
    public string m_sNameNoSuffix;
    public string m_sObjPath;

    UnityEngine.Object _m_objOrg;
    public UnityEngine.Object m_objOrg
    {
        get { return _m_objOrg; }
        set
        {
            if(_m_objOrg != value)
            {
                _m_objOrg = value;
                DoInit(_m_objOrg);
            }
        }
    }

    // 执行函数
	protected bool _m_isDoEvent = false;
    
    // 用来表示可以进行更新了
	protected bool _m_isRunning = false;

    protected float m_fProgress = 0.0f;

    // 播放是否结束
	public bool m_isEnd = false;

	public bool m_isJsonDataToSelfSuccessed = false;
	public JsonData m_jsonData = null;

	public bool m_isInitedFab = false;

    public EDT_Base()
    {
        this.m_iCurID = ++EVENT_CORE_CURSOR;
		this.m_isInitedFab = true;
    }

	~ EDT_Base(){
		DoClear ();
	}

	public void DoReInit(JsonData jsonData){
		DoReInit (0, jsonData);
	}

	public void DoReInit(float castTime, JsonData jsonData){
		DoClear ();
		OnReInit (castTime, jsonData);
	}

	public virtual void OnReInit(float castTime, JsonData jsonData){
		this.m_fCastTime = castTime;
		this.m_jsonData = jsonData;
		if (this.m_jsonData != null) {
			IDictionary map = (IDictionary)this.m_jsonData;
			if (map.Contains ("m_typeInt")) {
				int v = ((int)m_jsonData ["m_typeInt"]);
				this.m_emType = (EventType)v;
			}
		}
	}

	protected bool DoReInit(string objName,EventType type)
    {
        string path = GetPathByNameType(objName, type);
        bool isExists = File.Exists(path);
        if (!isExists)
        {
            Debug.LogWarning("资源路径path = ["+path + "],不存在！！！");
			return false;
        }
        UnityEngine.Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));

        DoReInit(obj);
		return true;
    }

	public virtual string GetPathByNameType(string objName,EventType type)
	{
		return "";
	}

    public void DoReInit(UnityEngine.Object obj)
    {
        DoClear();

        DoInit(obj);
    }

    public void DoInit(UnityEngine.Object obj)
    {
        if (obj == null)
            return;

        string path = "";

        UnityEngine.Object parentObject = obj;
        UnityEditor.PrefabType p_type = UnityEditor.PrefabUtility.GetPrefabType(obj);
        switch (p_type)
        {
            case UnityEditor.PrefabType.PrefabInstance:
                parentObject = UnityEditor.PrefabUtility.GetPrefabParent(obj);
                break;
            case UnityEditor.PrefabType.Prefab:
                break;
        }

        path = _GetPath(parentObject);

        this.m_sObjPath = path;
        this.m_sName = Path.GetFileName(this.m_sObjPath);
        this.m_sNameNoSuffix = Path.GetFileNameWithoutExtension(this.m_sObjPath);
        this.m_objOrg = parentObject;
		this.m_isInitedFab = true;

        OnInit();
    }

    public virtual void OnInit()
    {
    }

    string _GetPath(UnityEngine.Object parentObj)
    {
        return UnityEditor.AssetDatabase.GetAssetPath(parentObj);
    }

    public void SetIsDoEvent(bool isCalled)
    {
        this._m_isDoEvent = isCalled;
    }

    public void DoClear()
    {
		m_emType = EventType.None;

        m_sObjPath = "";
        m_sName = "";
        m_sNameNoSuffix = "";
        m_objOrg = null;

        m_fCastTime = 0.0f;
        _m_isDoEvent = false;
        _m_isRunning = false;

		m_isEnd = false;
		m_jsonData = null;
		m_isJsonDataToSelfSuccessed = false;

		m_isInitedFab = false;

        OnClear();
    }

    public virtual void OnClear() { }

    public virtual void DoStart(bool isReStart = false) {
        if (!isReStart)
        {
            if (this._m_isDoEvent || this.m_isEnd)
                return;
        }else
        {
            this.m_isEnd = true;
            DoEnd();
        }
        
        this._m_isRunning = true;
        this._m_isDoEvent = false;
        this.m_isEnd = false;
        this.m_fProgress = 0.0f;
    }

    public void DoUpdate(float upDeltaTime)
    {
        if (!_m_isRunning)
            return;

        if (m_isEnd)
            return;

        this.m_fProgress += upDeltaTime;

        DnCallEvent();

        OnCallUpdate(upDeltaTime);
    }

    void DnCallEvent()
    {
        if (this._m_isDoEvent)
            return;

        if (this.m_fProgress >= this.m_fCastTime)
        {
            this._m_isDoEvent = OnCallEvent();
        }
    }

    // 子类需要实现的事件
    protected virtual bool OnCallEvent()
    {
        return true;
    }

    void DoCallUpdate(float upDeltaTime)
    {
        if (!this._m_isDoEvent)
            return;

        OnCallUpdate(upDeltaTime);
    }

    // 子类需要实现的更新
    protected virtual void OnCallUpdate(float upDeltaTime)
    {
        
    }

    public virtual void DoEnd()
    {

    }

	public virtual JsonData ToJsonData(){
		return this.m_jsonData;
	}

	public void DoSceneGUI(Transform trsfOrg){
//		if (!this._m_isDoEvent) {
//			return;
//		}
		OnSceneGUI (trsfOrg);
	}

	public virtual void OnSceneGUI(Transform trsfOrg){
	}

	#region == 新对象静态方法 ==

	static public T NewEntity<T>() where T : EDT_Base,new(){
		return new T();
	}

	static public T NewEntity<T>(JsonData jsonData,float castTime = 0) where T : EDT_Base,new()
	{
		T one = new T();
		one.DoReInit (castTime, jsonData);
		if (one.m_isJsonDataToSelfSuccessed) {
			return one;
		} else {
			one.DoClear ();
		}
		return null;
	}

	#endregion
}
