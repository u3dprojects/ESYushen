using UnityEngine;
using System.Collections;
using LitJson;

/// <summary>
/// 类名 : Editor 下面的时间轴上的音效
/// 作者 : Canyon
/// 日期 : 2017-01-11 14:50
/// 功能 : 
/// </summary>
public class EDT_Audio : EDT_Base {

	// 音量
	public float m_fVolume;

	// 是否循环
	public bool m_isLoop;

	// 创建出来的实体对象
	EN_AudioSource _m_eAudio;

	public EDT_Audio() : base()
	{
		this.m_iCurType = 2;
	}

	public override void OnReInit (float castTime, LitJson.JsonData jsonData)
	{
		base.OnReInit (castTime, jsonData);

		string resName = (string)jsonData ["m_audioName"];
		bool isOkey = DoReInit (resName, 2);
		if (!isOkey) {
			return;
		}

		this.m_isJsonDataToSelfSuccessed = true;
	}

	public override JsonData ToJsonData ()
	{
		if (!this.m_isInitedFab)
			return null;

		JsonData ret = new JsonData ();
		ret["m_typeInt"] = this.m_iCurType;
		ret["m_audioName"] = this.m_sNameNoSuffix;
		return ret;
	}

	protected override bool OnCallEvent ()
	{
		if(this.m_objOrg != null && this.m_objOrg is AudioClip)
		{
			_m_eAudio = new EN_AudioSource ();
			_m_eAudio.DoReInit (this.m_sNameNoSuffix, true);
			_m_eAudio.DoStart ((AudioClip)this.m_objOrg, this.m_fVolume, this.m_isLoop);
			return true;
		}
		return false;
	}

	protected override void OnCallUpdate (float upDeltaTime)
	{
		if (this._m_eAudio != null) {
			this._m_eAudio.DoUpdateTime (upDeltaTime);
			this.m_isEnd = !this._m_eAudio.m_isLoop && this._m_eAudio.m_iLoopCount > 0;
		}
	}

	public override string GetPathByNameType(string objName, int type)
	{
		if (type != 2) {
			return "类型不对";
		}
		return "Assets\\PackResources\\Arts\\Effect\\Prefabs\\"+objName+".prefab";
	}

	public override void OnClear ()
	{
		base.OnClear ();
		this.m_iCurType = 2;

		OnClearAudio ();
	}

	void OnClearAudio()
	{
		if (_m_eAudio != null)
		{
			_m_eAudio.DoClear ();
			_m_eAudio = null;
		}
	}

	public override void DoEnd()
	{
		base.DoEnd();

		OnClearAudio();
	}
}
