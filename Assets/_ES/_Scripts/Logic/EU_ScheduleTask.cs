using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 类名 : 任务调度器
/// 作者 : Canyon
/// 日期 : 2017-02-08 14:20
/// 功能 : 
/// </summary>
/// [ExecuteInEditMode]

public class EU_ScheduleTask : MonoBehaviour{

	private static EU_ScheduleTask _m_instance;

	static public EU_ScheduleTask m_instance
	{
		get
		{
			_NewMono ();
			return _m_instance;
		}
	}

	static void _NewMono(){
		if (_m_instance == null)
		{
			GameObject gobj = GameObject.Find("EU_ScheduleTimer");
			if (gobj == null)
			{
				gobj = new GameObject("EU_ScheduleTimer");
			}

			_m_instance = gobj.GetComponent<EU_ScheduleTask>();
			if (_m_instance == null)
			{
				_m_instance = gobj.AddComponent<EU_ScheduleTask>();

				#if UNITY_EDITOR
				EditorApplication.update += _m_instance.OnUpdate;
				#endif
			}
		}
	}

	#if UNITY_EDITOR
	void OnEnable()
	{
		EditorApplication.update += OnUpdate;
	}

	void OnDisable()
	{
		EditorApplication.update -= OnUpdate;
	}
	#endif

	List<TaskEntity> list = new List<TaskEntity>();
	List<TaskEntity> rmlist = new List<TaskEntity>();
	TaskEntity temp;
	int lens = 0;

	bool isRunning = true;

	public void AddTask(float doT,System.Action call){
		TaskEntity em = new TaskEntity (doT, call);
		list.Add (em);
	}

	public void DoTask(float doT,System.Action call){
		AddTask (doT, call);
		DoStart();
	}

	protected void OnUpdate()
	{
		if (!isRunning)
			return;
		
		rmlist.Clear ();

		lens = list.Count;
		if (lens <= 0)
			return;

		for (int i = 0; i < lens; i++) {
			temp = list [i];
			temp.OnUpdate ();

			if (temp.isEnd) {
				rmlist.Add (temp);
			}
		}

		lens = rmlist.Count;
		for (int i = 0; i < lens; i++) {
			temp = rmlist [i];
			list.Remove (temp);
		}
	}

	public void DoStart()
	{
		isRunning = true;
	}

	public void DoClear(){
		isRunning = false;
		rmlist.Clear ();

		lens = list.Count;
		for (int i = 0; i < lens; i++) {
			temp = list [i];
			temp.DoClear ();
		}
		list.Clear ();
		temp = null;
		lens = 0;
	}

	public void DoDestroy(){
		#if UNITY_EDITOR
		EditorApplication.update -= OnUpdate;
		#endif
		isRunning = false;

		GameObject.DestroyImmediate (m_instance.gameObject);
	}

	// Update is called once per frame
	void Update () {
		#if !UNITY_EDITOR
		OnUpdate();
		#endif
	}
}

/// <summary>
/// 类名 : 任务 实体对象
/// 作者 : Canyon
/// 日期 : 2017-02-08 14:20
/// 功能 : 
/// </summary>
internal sealed class TaskEntity{
	
	EN_Time m_time = new EN_Time();

	float m_fDelay;
	float m_fDuration;
	bool m_isLoop;
	System.Action m_call;

	bool m_isFirstDo = false;

	bool isRunnging = true;
	public bool isEnd = false;

	public TaskEntity(float delay,System.Action call){
		this.m_fDelay = delay;
		this.m_call = call;
		this.m_fDuration = 0;
		this.m_isLoop = false;
	}

	public TaskEntity(float delay,float duration,System.Action call){
		this.m_fDelay = delay;
		this.m_call = call;
		this.m_fDuration = duration;
		this.m_isLoop = duration > 0;
	}

	public void OnUpdate()
	{
		if (!isRunnging)
			return;
		
		if (isEnd) {
			DoClear ();
			return;
		}

		OnUpCall ();
	}

	void OnUpCall(){
		m_time.DoUpdateTime();
		bool isCanCall = false;
		if (!m_isFirstDo) {
			isCanCall = m_time.ProgressTime >= m_fDelay;
		} else {
			if (m_isLoop) {
				isCanCall = m_time.ProgressTime >= m_fDuration;
			}
		}

		if (isCanCall) {
			m_isFirstDo = true;
			DoCall ();
			m_time.DoReInit (false);
		}

		if (!m_isLoop && m_isFirstDo) {
			DoClear ();
			isEnd = true;
		}
	}

	void DoCall(){
		if (m_call != null) {
			m_call ();
		}
	}

	public void DoClear(){
		isRunnging = false;
		m_call = null;
		m_time = null;
	}
}
