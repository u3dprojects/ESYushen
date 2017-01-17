using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ED_DT2 : MonoBehaviour {

    public GameObject gobjEffect;
    public bool isAddToSelf = false;

	EN_Effect _m_eEffect;
	EN_Time _m_eTime = new EN_Time();

	// 添加绘制圆区域
	public float shieldArea = 5;

    [InitializeOnLoadMethod]
    static public void Test()
    {
        Debug.Log("= InitializeOnLoadMethod = test = ");
    }

    // OnUpdate Update
    public void OnUpdate()
    {
		if(_m_eEffect != null)
		{
			_m_eTime.DoUpdateTime ();
			_m_eEffect.DoUpdate(_m_eTime.DeltaTime);
			if (_m_eEffect.isEnd) {
				_m_eEffect.DoDestory ();
				_m_eEffect = null;
			}
			UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
		}
    }

    [ContextMenu("DoPlay")]
    public void DoPlay()
    {
        if (gobjEffect == null)
            return;
		Debug.Log("=== DoPlay ===");

		UnityEngine.GameObject gobj = GameObject.Instantiate(this.gobjEffect, Vector3.zero, Quaternion.identity) as GameObject;
		Transform trsfGobj = gobj.transform;
		if (isAddToSelf)
		{
			trsfGobj = transform;
		}

		trsfGobj.localPosition = Vector3.zero;
		trsfGobj.localEulerAngles = Vector3.zero;
		trsfGobj.localScale = Vector3.one;

		_m_eEffect = new EN_Effect(gobj,0);
		_m_eEffect.DoStart();
		_m_eEffect.SetScale(1);
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

	#region ========= 播放声音 ===

	public AudioClip testClip;
	public double testLatency = .1d; //how much time is given to schedule playback (press space to switch sources. If you hear clipping, try a higher testLatency value)
	public int stopSample; //on which sample to stop (press enter to stop, again to play)

	AudioClip _createdClip;
	AudioSource[] _sources;

	//ref for switching sources
	AudioSource _currentSource;
	AudioSource _otherSource;

	void Awake()
	{
		_sources = new AudioSource[2];

		for(int i=0;i<2;i++)
		{
			_sources[i] = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
			_sources[i].loop = true;
			_sources[i].clip = testClip;
		}

		_currentSource = _sources[0]; //init refSources
		_otherSource = _sources[1];

		// _currentSource.Play();

	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			SwitchSources(testLatency);
		}

		if(Input.GetKeyDown (KeyCode.KeypadEnter))
		{
			if(_currentSource.isPlaying)
			{
				StopSourceOnSample(stopSample,_currentSource);
			}
			else
			{
				_currentSource.timeSamples = 0;
				_currentSource.Play ();
			}
		}
	}

	void SwitchSources(double deltaTime) //deltaTime : in how much time will the switch occur
	{
		int deltaSamples = (int)(deltaTime*testClip.frequency); //deltaTime in samples
		double dspTime = AudioSettings.dspTime; //cache dspTime to be safe (not on the same thread)
		int timeSamples = _currentSource.timeSamples; //idem

		_otherSource.timeSamples = (timeSamples+deltaSamples)%testClip.samples; //adjust the other sources playback position(timeSamples) abd make sure it wraps (%)
		_otherSource.PlayScheduled(dspTime+deltaTime);
		_currentSource.SetScheduledEndTime(dspTime+deltaTime); //this one deserves a kiss, thanks guys! Eliminates A LOT of ugly stopping coroutines I had to write to stop a source on a precise sample.

		AudioSource tmpSource = _currentSource; //Switching source refs
		_currentSource = _otherSource;
		_otherSource = tmpSource;
	}

	void StopSourceOnSample(int stopSample,AudioSource source)
	{
		int timeSamples = source.timeSamples; //cache timeSamples and dsp time
		double dspTime = AudioSettings.dspTime;

		int deltaSamples;
		if(stopSample>timeSamples)
		{
			deltaSamples = stopSample - timeSamples;
		}
		else //clip will loop before we stop
		{
			deltaSamples = source.clip.samples - timeSamples + stopSample;
		}

		double deltaTime = (double)deltaSamples/source.clip.frequency;

		source.SetScheduledEndTime(dspTime + deltaTime);
	}

	#endregion
}
