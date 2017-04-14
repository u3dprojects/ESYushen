using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;

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

	// 时长
	public float m_fDuration;

	// 是否跟随
	public bool m_isFollow = true;

	// 特效是否在指定点位置播放
	public bool m_isAppointPos = false;
    
    // 创建出来的实体对象
    EN_Effect _m_eEffect;

    public EDT_Effect() : base()
    {
		this.m_emType = EventType.Effect;
		this.m_fScale = 1;
		this.m_isInitedFab = false;
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

			if (!this.m_isFollow) {
				trsfGobj.parent = null;
			}

			_m_eEffect = new EN_Effect(gobj,m_fDuration);
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

		this.m_fDuration = 0;
		this.m_emType = EventType.Effect;
		this.m_fScale = 1;

        OnClearEffect();

		m_strEffectFolderVal = "";
		_pathFolders = null;
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

	public override void OnReInit (float castTime, JsonData jsonData)
	{
		base.OnReInit (castTime, jsonData);

		string resName = (string)jsonData ["m_resName"];
		bool isOkey = DoReInit (resName, m_emType);
		if (!isOkey) {
			return;
		}

		this.m_iJoint = (int)jsonData ["m_joint"];

		IDictionary dicJsonData = (IDictionary)jsonData;

		if (dicJsonData.Contains("m_scale")) {
			this.m_fScale = float.Parse (jsonData ["m_scale"].ToString ());
		} else {
			this.m_fScale = 1f;
		}

		if (dicJsonData.Contains("m_duration")) {
			this.m_fDuration = float.Parse (jsonData ["m_duration"].ToString ());
		}

		if (dicJsonData.Contains("m_isBindInAttacker")) {
			this.m_isFollow = (bool)jsonData ["m_isBindInAttacker"];
		}

		if (dicJsonData.Contains("m_posType")) {
			this.m_isAppointPos = ((int)jsonData ["m_posType"]) == 1;
		}

		JsonData tmp = null;
		if (dicJsonData.Contains ("m_pos")) {
			tmp = jsonData ["m_pos"];
		}
		if (tmp != null && tmp.IsObject) {
			float x = float.Parse(tmp ["x"].ToString());
			float y = float.Parse(tmp ["y"].ToString());
			float z = float.Parse(tmp ["z"].ToString());
			this.m_v3OffsetPos = new Vector3 (x, y, z);
		} else {
			this.m_v3OffsetPos = Vector3.zero;
		}

		if (dicJsonData.Contains ("m_angle")) {
			tmp = jsonData ["m_angle"];
		}

		if (tmp != null && tmp.IsObject) {
			float x = float.Parse(tmp ["x"].ToString());
			float y = float.Parse(tmp ["y"].ToString());
			float z = float.Parse(tmp ["z"].ToString());
			this.m_v3EulerAngle = new Vector3 (x, y, z);
		} else {
			this.m_v3EulerAngle = Vector3.zero;
		}

		this.m_isJsonDataToSelfSuccessed = true;
	}

	public override JsonData ToJsonData ()
	{
		if (!this.m_isInitedFab)
			return null;
		
		JsonData ret = new JsonData ();
		ret["m_typeInt"] = (int)this.m_emType;
		ret["m_resName"] = this.m_sNameNoSuffix;
		ret["m_joint"] = this.m_iJoint;
		ret["m_scale"] = Round2D(this.m_fScale,2);
		ret["m_duration"] = Round2D(this.m_fDuration,2);
		ret["m_isBindInAttacker"] = this.m_isFollow;
		ret["m_posType"] = this.m_isAppointPos ? 1 : 0;

		JsonData pos = new JsonData ();
		pos ["x"] = Round2D(this.m_v3OffsetPos.x,2);
		pos ["y"] = Round2D(this.m_v3OffsetPos.y,2);
		pos ["z"] = Round2D(this.m_v3OffsetPos.z,2);
		ret["m_pos"] = pos;

		pos = new JsonData ();
		pos ["x"] = Round2D(this.m_v3OffsetPos.x,2);
		pos ["y"] = Round2D(this.m_v3OffsetPos.y,2);
		pos ["z"] = Round2D(this.m_v3OffsetPos.z,2);
		ret["m_angle"] = pos;
		return ret;
	}

	public override string GetPathByNameType(string objName, EventType type)
	{
		if (type != EventType.Effect) {
			return "类型不对";
		}

		return GetPath (objName);
	}


	static string m_defFolder = "Assets\\PackResources\\Arts\\Effect\\Prefabs\\";
	static string m_strEffectFolderVal = "";
	static string[] _pathFolders = null;
	static public string[] PathFolders{
		get{
			if (string.IsNullOrEmpty (m_strEffectFolderVal)) {
				string path = "Assets/EffectFolder.txt";
				bool isExists = File.Exists(path);
				if (isExists) {
					UnityEngine.TextAsset obj = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.TextAsset)) as UnityEngine.TextAsset;
					m_strEffectFolderVal = obj.text;
				} else {
					Debug.LogWarning ("路径path = [" + path + "],不存在！！！");
				}
			}
			if (_pathFolders == null && !string.IsNullOrEmpty (m_strEffectFolderVal)) {
				_pathFolders = m_strEffectFolderVal.Split ("\r\n\t".ToCharArray (),System.StringSplitOptions.RemoveEmptyEntries);
			}
			return _pathFolders;
		}
	}

	static public string GetPath(string objName){
		string path = "";
		_pathFolders = PathFolders;
		bool isExists = false;
		if (_pathFolders != null) {
			foreach (var item in _pathFolders) {
				if (string.IsNullOrEmpty (item))
					continue;

				isExists = Directory.Exists(item);
				if (isExists) {
					path = item + objName+".prefab";
					isExists = File.Exists(path);
					if (isExists) {
						break;
					}
				}
			}
		}

		if (!isExists)
			path = m_defFolder + objName+".prefab";
		
		return path;
	}
}
