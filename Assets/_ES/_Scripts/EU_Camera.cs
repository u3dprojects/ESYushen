using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : 3D 摄像机控制
/// 作者 : Canyon
/// 日期 : 2017-03-01 17:16
/// 功能 : 跟随,朝向
/// </summary>
// [RequireComponent (typeof(Camera))]
public class EU_Camera : MonoBehaviour
{

	float defHeight = 4.5f;
	float defDistance = 5.5f;
	float defOffsetHeigh = 0.0f;
	float defRotateAngle = 0.0f;
	string defGobjName = "Camera3D";


	float offsetHeigh = 0.0f;
	float preHeight = 0.0f;
	public float height = 0.0f;
	float curHeight = 0.0f;
	float heightDamping = 2.0f;
	bool isHeightDamping = false;

	float preDistance = 0.0f;
	public float distance = 0.0f;
	float curDistance = 0.0f;
	float distanceDamping = 2.0f;
	bool isDistanceDamping = false;

	float initAngleY = -30.0f;
	float reTargetAngle = -180.0f;
	float rotationDamping = 1.0f;
	float minAbsAngle = 0.1f;
	bool isRotateDamping = false;

	float wantedRotationAngle = 0.0f;
	float currentRotationAngle = 0.0f;
	Quaternion currentRotation = Quaternion.identity;

	Camera m_camera;
	GameObject gobjLook;

	Transform target;
	bool isLookAt = false;
	bool isRunning = false;


	void Awake(){
		gameObject.name = defGobjName;
		gobjLook = GameObject.Find ("LookGobj");
		if (gobjLook == null) {
			gobjLook = new GameObject ("LookGobj");
		}
		m_camera = gameObject.GetComponentInChildren<Camera> (true);

		reInitAfterClear();
	}

	void doClear(){
		parentTrsf(this.gobjLook,null,true);
	}

	public void reInitAfterClear(){
		this.doClear ();
		this.OnInitAttrs ();
	}
	
	// Use this for initialization
//	void Start ()
//	{
//	}

	// Update is called once per frame
//	void Update ()
//	{	
//	}

	void OnInitAttrs ()
	{
		this.offsetHeigh = defOffsetHeigh;

		// 控制高度
		this.height = defHeight;
		// 当前 高度 值
		this.curHeight = 0;
		this.isHeightDamping = false;
		// 控制 高度 平滑过得的速度
		this.heightDamping = 2;

		// 控制距离
		this.distance = defDistance;
		// 当前 距离 值
		this.curDistance = 0;
		this.isDistanceDamping = false;
		// 控制 距离 平滑过得的速度
		this.distanceDamping = 2;

		// 控制旋转
		this.isRotateDamping = false;
		this.initAngleY = -30;
		this.reTargetAngle = -180;
		this.rotationDamping = 1;
		this.minAbsAngle = 0.1f;

		this.wantedRotationAngle = defRotateAngle;
		this.currentRotationAngle = 0;
		this.currentRotation = Quaternion.identity;
	}

	void OnInit (GameObject _target, bool _isRotate)
	{
		setTarget (_target);

		if (target == null)
			return;

		setIsRotateDamping (_isRotate);

		// 计算旋转角度
		if (this.isRotateDamping) {
			this.currentRotation = Quaternion.Euler (0, this.initAngleY, 0) * this.target.rotation;
			this.wantedRotationAngle = this.target.eulerAngles.y + this.reTargetAngle;
		} else {
			this.currentRotation = Quaternion.Euler (0, 0, 0);
		}

		Vector3 newPos = this.currentRotation * Vector3.back * this.distance;

		newPos = this.target.position + newPos;
		newPos.y = this.target.position.y + this.height;
		this.transform.position = newPos;

		this.curHeight = this.height;
		this.curDistance = this.distance;
		this.isHeightDamping = false;
		this.isDistanceDamping = false;

		reLook (true);
		this.currentRotationAngle = this.transform.eulerAngles.y;
	}

	void parentTrsf (GameObject gobj, GameObject gobjParent, bool isLocal = false)
	{
		var trsfSelf = gobj.transform;
		Transform trsfParent = null;
		if (gobjParent != null) {
			trsfParent = gobjParent.transform;
		}

		trsfSelf.parent = trsfParent;
		if (isLocal) {
			trsfSelf.localPosition = Vector3.zero;
			trsfSelf.localScale = Vector3.one;
			trsfSelf.localEulerAngles = Vector3.zero;
		}
	}

	void setIsRotateDamping (bool isRotateDamping)
	{
		this.isRotateDamping = (isRotateDamping == true);
	}

	public void doBegin (GameObject _target, bool _isRotate)
	{
		OnInit (_target, _isRotate);
		setIsLookAt (true);
		setIsRunning (true);
	}

	void LateUpdate ()
	{
		if (!this.isRunning) {
			return;
		}

		if (this.target == null) {
			return;
		}

		var _deltaTime = Time.deltaTime;

		this.currentRotationAngle = calcAngle (this.currentRotationAngle, this.wantedRotationAngle, this.rotationDamping * _deltaTime);
		this.currentRotation = Quaternion.Euler (0, this.currentRotationAngle, 0);

		this.curDistance = calcDistance (this.curDistance, this.distance, this.distanceDamping * _deltaTime);
		this.curHeight = calcHeight (this.curHeight, this.height, this.heightDamping * _deltaTime);
		this.isDistanceDamping = this.curDistance != this.distance;
		this.isHeightDamping = this.curHeight != this.height;

		Vector3 newPos = this.target.position + this.currentRotation * Vector3.back * this.curDistance;
		newPos.y = this.target.position.y + this.curHeight;
		this.transform.position = newPos;

		reLook ();
	}

	void resetGobjNm (string _nmGobj)
	{
		this.gameObject.name = string.IsNullOrEmpty (_nmGobj) ? defGobjName : _nmGobj;
	}

	void setPos (Vector3 _pos)
	{
		this.transform.position = _pos;
	}

	void setTarget (GameObject target)
	{
		parentTrsf (this.gobjLook, target, true);
		if (target != null) {
			this.target = this.gobjLook.transform;
		} else {
			this.target = null;
		}
	}

	Transform getTarget ()
	{
		return this.target;
	}

	void reLook (bool isCan = false)
	{
		if (!this.isLookAt && !isCan) {
			return;
		}
		var lookPos = this.target.position + new Vector3 (0, this.offsetHeigh / 2, 0);
		lookAtPos (lookPos);
	}

	void lookAtPos (Vector3 _pos)
	{
		this.transform.LookAt (_pos);
	}


	void resetOffHeight (float _offset)
	{
		this.offsetHeigh = _offset <= 0 ? defOffsetHeigh : _offset;
	}

	public void resetRotateAngle (float rotateAngle, bool isImmdiateAngle = true)
	{
		this.wantedRotationAngle = rotateAngle;
		if (isImmdiateAngle) {
			this.currentRotationAngle = this.wantedRotationAngle;
		}
		this.isRotateDamping = this.currentRotationAngle != this.wantedRotationAngle;
	}

	void resetDH (float d, float h, bool isReInitCur)
	{
		d = d <= 0 ? defDistance : d;
		h = h <= 0 ? defHeight : h;
		if (d == 0 || h == 0) {
			return;
		}

		this.preDistance = this.distance <= 0 ? defDistance : this.distance;
		this.preHeight = this.height <= 0 ? defHeight : this.height;

		this.distance = d;
		this.height = h;
		if (isReInitCur) {
			this.curDistance = this.distance;
			this.curHeight = this.height;
		}

		this.isDistanceDamping = this.curDistance != this.distance;
		this.isHeightDamping = this.curHeight != this.height;
	}

	void reBarkDH (bool isReInitCur)
	{
		resetDH (this.preDistance, this.preHeight, isReInitCur);
	}

	void setBgColor (float r, float g, float b, float a)
	{
		r = r <= 0 ? 49 : r;
		g = g <= 0 ? 77 : g;
		b = b <= 0 ? 121 : b;
		a = a <= 0 ? 0.2f : a;

		float max = 255f;
		this.m_camera.backgroundColor = new Color (r / max, g/ max, b/ max, a/ max);
	}

	void setIsRunning (bool isRunning)
	{
		this.isRunning = (isRunning == true);
	}

	void setIsLookAt (bool isLookAt)
	{
		this.isLookAt = (isLookAt == true);
	}

	bool getIsRotateDamping ()
	{
		if (this.isRotateDamping) {
			float _difAngle = AngleDistance (this.currentRotationAngle, this.wantedRotationAngle);
			float _min = this.minAbsAngle;
			float _max = 360 - this.minAbsAngle;
			_max = Mathf.Repeat (_max, 360);
			this.isRotateDamping = _difAngle < _max && _difAngle > _min;
		}
		return this.isRotateDamping;
	}

	public void resetTarget (GameObject target, bool isImmdiateDis)
	{
		if (target != null) {
			if (isImmdiateDis) {
				this.curDistance = this.distance;
			} else {
				float _diffDis = Vector3.Distance (target.transform.position, this.transform.position);
				float _disAbs = Mathf.Abs (this.curDistance - _diffDis);
				if (_disAbs > 2) {
					this.curDistance = _diffDis;
				}
			}
			this.isDistanceDamping = this.curDistance != this.distance;
		}
		setTarget (target);
	}

	Camera getCamera ()
	{
		return this.m_camera;
	}

	float calcAngle (float a, float b, float t)
	{
		if (getIsRotateDamping ()) {
			a = Mathf.LerpAngle (a, b, t);
		} else {
			a = b;
		}
		return a;
	}

	float AngleDistance (float a, float b)
	{
		a = Mathf.Repeat (a, 360);
		b = Mathf.Repeat (b, 360);
		return Mathf.Abs (b - a);
	}

	float calcValLerp (float a, float b, float t)
	{
		if (a != b) {
			a = Mathf.Lerp (a, b, t);
		} else {
			a = b;
		}
		return a;
	}

	float calcDistance (float a, float b, float t)
	{
		if (this.isDistanceDamping) {
			a = calcValLerp (a, b, t);
		} else {
			a = b;
		}
		return a;
	}

	float calcHeight (float a, float b, float t)
	{
		if (this.isHeightDamping) {
			a = calcValLerp (a, b, t);
		} else {
			a = b;
		}
		return a;
	}
}