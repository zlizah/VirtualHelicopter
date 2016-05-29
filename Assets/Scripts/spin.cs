using UnityEngine;
using System.Collections;

public class spin : MonoBehaviour {

	public Vector3 spinRotor(float w, float thrust) {
		float revpersec = w / (2 * Mathf.PI);
		revpersec *= Mathf.Rad2Deg * 0.3f; //Scaling for animation.. (Modification)

		if (thrust != 0) {
			revpersec *= (thrust / 100); //Slight modification
		}

		print ("REVVING: " + revpersec);
		return new Vector3 (0, 0, -revpersec);
	}
}