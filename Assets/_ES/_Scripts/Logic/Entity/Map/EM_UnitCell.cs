using UnityEngine;
using System.Collections;
using LitJson;

/// <summary>
/// 类名 : 地图基础类之单元
/// 作者 : Canyon
/// 日期 : 2017-02-04 09:36
/// 功能 : 主要是处理地图上面物体单元信息等
/// </summary>
[System.Serializable]
public class EM_UnitCell : EM_Cube {
	// monsterID 数据表结构里面的唯一标识ID
	// [SerializeField]
	int _m_iUnqID;
	public int m_iUnqID;

	// 刷怪时间间隔
	public float m_fReliveInv;
	
	// 出生点
	public Vector3 m_v3Pos = new Vector3(0,10,0);

	// 旋转角度偏移量,就是y轴值
	public float m_fRotation;

	// 等级
	public int m_iLevel;

	// 显示模型
	public bool m_isShowModel;
	bool _m_isShowModel;

	[System.NonSerialized]
	public GameObject m_gobjModel;

	float m_fDefaultY = 999;

	public EM_UnitCell() : base(){
	}

	protected override bool OnInit ()
	{
		if (m_jdOrg != null) {
			IDictionary map = (IDictionary)m_jdOrg;
			if (map.Contains ("monsterID")) {
				this.m_iUnqID = (int)m_jdOrg ["monsterID"];
			} else {
				this.m_iUnqID = (int)m_jdOrg ["ID"];
			}

			if (map.Contains ("rotateDegree")) {
				this.m_fRotation = float.Parse (m_jdOrg ["rotateDegree"].ToString ());
			}
			m_v3Pos.x = float.Parse (m_jdOrg ["positionX"].ToString ());
			m_v3Pos.z = float.Parse (m_jdOrg ["positionZ"].ToString ());

			if (map.Contains ("reliveInterval")) {
				this.m_fReliveInv = float.Parse (m_jdOrg ["reliveInterval"].ToString ());
			}

			if (map.Contains ("level")) {
				this.m_iLevel = (int)m_jdOrg ["level"];
			}

			_m_iUnqID = m_iUnqID;

			DoNew ();

			ToTrsfData ();
			return true;
		}
		return false;
	}

	public override JsonData ToJsonData ()
	{
		if (this.m_iUnqID <= 0)
			return null;

		JsonData ret = new JsonData ();
		ret ["ID"] = this.m_iUnqID;
		ret ["rotateDegree"] = Round2D(this.m_fRotation,2);
		ret ["reliveInterval"] = Round2D(this.m_fReliveInv,2);
		ret ["positionX"] = Round2D(this.m_v3Pos.x,2);
		ret ["positionZ"] = Round2D(this.m_v3Pos.z,2);
		ret ["level"] = this.m_iLevel;

		return ret;
	}

	public override void OnChangeTransform (Transform trsf)
	{
		base.OnChangeTransform (trsf);
		if (trsf == m_trsf) {
			ToData ();

			CheckNavmesh ();
		}
	}

	protected override void OnCloneData (EM_Base org)
	{
		if(org == null)
		{
			return;
		}
		base.OnCloneData (org);
		if(org is EM_UnitCell){
			EM_UnitCell tmp = (EM_UnitCell)org;
			this.m_fReliveInv = tmp.m_fReliveInv;
			this.m_fRotation = tmp.m_fRotation;
			this.m_iUnqID = tmp.m_iUnqID;
			this.m_v3Pos = tmp.m_v3Pos;
			this.m_isShowModel = tmp.m_isShowModel;
			this.m_iLevel = tmp.m_iLevel;

			this._m_iUnqID = m_iUnqID;
		}
	}

	public void DoMakeNew(){
		DoNew ();
		ToData ();
	}

	public void ToData(){
		m_v3Pos = m_trsf.position;
		m_fRotation = m_trsf.eulerAngles.y;
	}

	protected void ToTrsfRotation(){
		Vector3 v3Rotation = m_trsf.eulerAngles;
		v3Rotation.y = m_fRotation;
		m_trsf.eulerAngles = v3Rotation;
	}

	public void ToTrsfData(){
		m_trsf.position = m_v3Pos;
		ToTrsfRotation ();
	}

	public void DoRaycast(){
		OnRaycast ();
		ToTrsfData ();
	}


	void OnRaycast(){
		// 默认一个y值
		m_v3Pos.y = m_fDefaultY;
		ToTrsfData ();

		RaycastHit[] hits = null;

		// hits = Physics.RaycastAll (m_v3Pos, Vector3.down, m_fDefaultY + 10);

		int layerTerrain = LayerMask.NameToLayer("Terrain");
		LayerMask mask = 1 << layerTerrain;
		hits = Physics.RaycastAll (m_v3Pos, Vector3.down, m_fDefaultY + 10, mask);

		RaycastHit hit;
		if (hits != null && hits.Length > 0) {
			hit = hits [0];
			// 取得碰撞点位置
			var item = hit.point;
			m_v3Pos.y = item.y + m_trsf.lossyScale.y / 2f;
		} else {
			m_v3Pos.y = 10;
		}

		CheckNavmesh ();
	}

	protected override void OnDestroyChild ()
	{
		base.OnDestroyChild ();
		m_gobjModel = null;
	}

	public void AddModel(GameObject model){
		m_gobjModel = model;
		AddChild (model);
		ModelActiveStatus ();
	}

	public void ModelActiveStatus(){
		if (_m_isShowModel == m_isShowModel)
			return;

		_m_isShowModel = m_isShowModel;

		if (m_gobjModel != null) {
			m_gobjModel.SetActive (m_isShowModel);
			SetRenderEnable(!m_isShowModel);
		}
	}

	public void CheckUnqIDChange(){
		if (_m_iUnqID == m_iUnqID)
			return;
		_m_iUnqID = m_iUnqID;

		m_isShowModel = false;
		DoDestroyChild ();
	}

	protected void CheckNavmesh(){
		NavMeshHit navHit;
		// 寻找最近边缘
		NavMesh.FindClosestEdge (m_v3Pos, out navHit, NavMesh.AllAreas);
		bool isHasNav = false;
		if (navHit.hit) {
//			Vector3 diff = navHit.position - m_v3Pos;
//			isHasNav = diff.sqrMagnitude <= 0.001f;
			isHasNav = true;
		}
		if (isHasNav) {
			this.m_curbeMatColor = Color.blue;
		} else {
			this.m_curbeMatColor = Color.red;
		}

		OnResetColor ();
	}
}
