using UnityEngine;
using System.Collections;

public class TiltingControl : MonoBehaviour {
	[SerializeField]
	private Vector2 currentTilt = new Vector2(0.0f, 0.0f);

	// Use this for initialization
	void Start () {
	}
	
	// Update call
	public Vector3 doUpdate(float dt) {
		//Detect tilt input via mouse
		float h = Input.GetAxis("Mouse X");
		float v = Input.GetAxis("Mouse Y");
		Vector2 deltaVec = new Vector2 (v, h);

		Vector2 newVec = currentTilt + deltaVec;
		if (newVec.magnitude > 1.0f) {
			newVec.Normalize();
		}
		currentTilt = newVec;

		return new Vector3(0.1f * currentTilt.x, 0.0f, 0.1f * currentTilt.y);
	}
}
