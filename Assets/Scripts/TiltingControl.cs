using UnityEngine;
using System.Collections;

public class TiltingControl : MonoBehaviour {
	[SerializeField]
	private Vector2 currentTilt = new Vector2(0.0f, 0.0f);
	[SerializeField]
	public static int inputSelector = 0p;

	// Use this for initialization
	void Start () {
	}
	
	// Update call
	public Vector3 doUpdate(float dt) {
		float h = 0.0f;
		float v = 0.0f;
		float scaleFactor = 0.0f;
		if (inputSelector == 0) {
			//Detect tilt input via mouse
			h = Input.GetAxis ("Mouse X");
			v = Input.GetAxis ("Mouse Y");
			scaleFactor = 0.1f;
		} else {
			h = Input.GetAxis ("X-Axis");
			v = Input.GetAxis ("Y-Axis");
			scaleFactor = 0.3f;
		}
		Vector2 deltaVec = new Vector2 (v, h);

		Vector2 newVec = currentTilt + deltaVec;
		if (newVec.magnitude > 1.0f) {
			newVec.Normalize ();
		}
		currentTilt = newVec;

		return new Vector3 (scaleFactor * currentTilt.x, 0.0f, scaleFactor * currentTilt.y);
	}
}
