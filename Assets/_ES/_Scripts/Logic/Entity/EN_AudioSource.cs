using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : 音效实体
/// 作者 : Canyon
/// 日期 : 2017-01-17 14:30
/// 功能 : 管理整个 AudioSource 的数据
/// </summary>
[System.Serializable]
public class EN_AudioSource : System.Object{
	// 声音对象
	public AudioSource m_auido;

	// 声音长度
	public float m_fLens;

	// 音量
	public float m_fVolume;

	// 是否循环
	public bool m_isLoop;

	// 是否可以中断
	public bool m_isInterrupt;

	// 0-默认,1-play,2-pause,3-stop/end
	public int m_iCurType = 0;

	// 计算
	// 当前阶段(在一个周期中的阶段)
	float m_fPhase = 0.0f;

	// 当前状态进度时间 = normalizedTime
	float m_fProgressTime = 0.0f;

	// 当前循环次数
	public int m_iLoopCount = 0;

	public void DoReInit(string gobjName,bool isInterrupt){
		DoReInit (new GameObject (gobjName), isInterrupt);
	}

	public void DoReInit(GameObject gobjEntity,bool isInterrupt){
		AudioSource ad = gobjEntity.GetComponent<AudioSource> ();
		if (ad == null) {
			ad = gobjEntity.AddComponent<AudioSource> ();
		}
		DoReInit (ad, isInterrupt);
	}

	public void DoReInit(AudioSource ad,bool isInterrupt){
		DoClear ();
		DoInit (ad,isInterrupt);
	}

	public void DoInit(AudioSource ad,bool isInterrupt){
		OnClearAttrs ();
		OnClearTimer ();

		this.m_auido = ad;
		this.m_isInterrupt = isInterrupt;
	}

	public void DoClear(){
		OnClearAudio ();

		OnClearAttrs();

		OnClearTimer ();
	}

	void OnClearAudio(){
		if (this.m_auido != null) {
			GameObject gobj = this.m_auido.gameObject;
			gobj.SetActive(false);
			#if UNITY_EDITOR
			GameObject.DestroyImmediate(gobj);
			#else
			GameObject.Destroy(gobj);
			#endif

			this.m_auido = null;
		}
	}

	void OnClearAttrs(){
		this.m_fLens = 0;
		this.m_fVolume = 0;
		this.m_isLoop = false;
		this.m_isInterrupt = false;
		this.m_iCurType = 0;
	}

	void OnClearTimer(){
		this.m_fPhase = 0.0f;
		this.m_fProgressTime = 0.0f;
		this.m_iLoopCount = 0;
	}

	public void DoStart(AudioClip clip,float volume,bool isLoop){
		if (this.m_auido == null) {
			return;
		}

		if (this.m_iCurType != 3) {
			if (!this.m_isInterrupt) {
				return;
			}
		}

		Stop ();

		this.m_fVolume = volume;
		this.m_isLoop = isLoop;
		this.m_fLens = clip.length;
		this.m_isInterrupt = false;

		this.m_auido.clip = clip;
		this.m_auido.volume = this.m_fVolume;
		this.m_auido.loop = this.m_isLoop;

		Play ();
	}

	void Stop(){
		this.m_iCurType = 3;
		if (this.m_auido != null) {
			this.m_auido.Stop ();
		}
	}

	void Pause(){
		this.m_iCurType = 2;
		if (this.m_auido != null) {
			this.m_auido.Pause();
		}
	}

	void Play(){
		this.m_iCurType = 1;
		if (this.m_auido != null) {
			this.m_auido.Play();
		}
	}

	public void DoUpdateTime(float deltatime){
		OnUpdateTime (deltatime);
	}

	protected void OnUpdateTime(float deltatime)
	{
		if (this.m_iCurType != 1) {
			return;
		}

		this.m_fPhase += deltatime;
		this.m_fProgressTime += deltatime;
		if (this.m_fPhase >= this.m_fLens) {
			this.m_fPhase -= this.m_fLens;
			this.m_iLoopCount++;
			if (!this.m_isLoop) {
				Stop ();
			}
		}
	}
}
