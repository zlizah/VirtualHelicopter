using UnityEngine;
using System.Collections;

public class spin : MonoBehaviour {

	public Vector3 spinRotor(float w, float dT) {
		float revpersec = w / (2 * Mathf.PI);
		revpersec *= Mathf.Rad2Deg * 0.3f; //Scaling for animation..
		return new Vector3 (0, 0, -revpersec);
	}
}