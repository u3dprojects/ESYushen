using UnityEngine;
using System.Collections;

public class EU_Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		trsf = transform;
		ctrlCam.doBegin (gameObject, true);
	}

	Transform trsf;

	public float speed = 5;
	public EU_Camera ctrlCam;

	public float Second = 0.0f;
	float curSecond  = 0.0f;

	// Update is called once per frame
	void Update () {
		 Axis();
		if (Second > 0) {
			curSecond += Time.deltaTime;
			if (curSecond >= Second) {
				curSecond = 0.0f;
			}
		} else {
			curSecond = 0.0f;
		}

		// ctrlCam.resetRotateAngle (trsf.localEulerAngles.y, false);
	}

	public float rotationSpeed = 100.0F;

	void Axis(){
		float translation = Input.GetAxis("Vertical") * speed;
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		translation *= Time.deltaTime;
		rotation *= Time.deltaTime;
		trsf.Translate(0, 0, translation);
		trsf.Rotate(0, rotation, 0);
	}
}
