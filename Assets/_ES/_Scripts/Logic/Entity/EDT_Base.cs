using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;

/// <summary>
/// 类名 : Editor 的时间轴上的Event实体基类
/// 作者 : Canyon
/// 日期 : 2017-01-11 14:06
/// 功能 : 编辑器模式处理一个创建物体事件外，还要需要一个驱动动作特效函数，
/// </summary>
public class EDT_Base {

    // 对象的唯一标识 计数器
    static int EVENT_CORE_CURSOR = 0;

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
    bool _m_isDoEvent = false;
    
    // 用来表示可以进行更新了
    bool _m_isRunning = false;

    protected float m_fProgress = 0.0f;

    // 播放是否结束
	public bool m_isEnd = false;

    // 当前事件类型
    public int m_iCurType = -1;

	public bool m_isJsonDataToSelfSuccessed = false;
	public JsonData m_jsonData = null;

	public bool m_isInitedData = false;

    public EDT_Base()
    {
        this.m_iCurID = ++EVENT_CORE_CURSOR;
    }

	public void DoReInit(float castTime, JsonData jsonData){
		DoClear ();
		OnReInit (castTime, jsonData);
	}

	public virtual void OnReInit(float castTime, JsonData jsonData){
		this.m_fCastTime = castTime;
		this.m_jsonData = jsonData;
	}

	public bool DoReInit(string objName,int type)
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

	public virtual string GetPathByNameType(string objName,int type)
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
		this.m_isInitedData = true;

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
        m_sObjPath = "";
        m_sName = "";
        m_sNameNoSuffix = "";
        m_objOrg = null;

        m_fCastTime = 0.0f;
        _m_isDoEvent = false;
        _m_isRunning = false;

		m_isEnd = false;
		m_iCurType = -1;
		m_jsonData = null;
		m_isJsonDataToSelfSuccessed = false;

		m_isInitedData = false;

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
}
