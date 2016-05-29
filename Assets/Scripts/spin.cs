using UnityEngine;
using System.Collections;

public class spin : MonoBehaviour {

	public Vector3 spinRotor(float w, float thrust) {
		float revpersec = w / (2 * Mathf.PI);
		revpersec *= Mathf.Rad2Deg * 0.3f; //Scaling for animation.. (Modification)



		return new Vector3 (0, 0, -revpersec);
	}
}