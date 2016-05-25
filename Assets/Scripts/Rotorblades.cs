using UnityEngine;
using System.Collections;

public class RotorbladeSystem : MonoBehaviour {
	//Inner class for each rotorblade
	public class Rotorblade : MonoBehaviour {
		//Inner class representing a slice
		public class RotorbladeSlice : MonoBehaviour {
			//Constructor
			public RotorbladeSlice() {
				Rigidbody r = new Rigidbody;
				r.velocity
			}

		

			//UiSlice
			private Vector3  

			//Ui
			public Vector3 getAirflowVelocity() {
				Vector3 y = this->getVectorFromRotor;
			}

			//Normal
			public Vector3 getNormal() {
			}

			//Size
			public float getArea() {
			}

			//Rotor - slice direction (yi)
			public Vector3 getVectorFromRotor() {
				
			}

			//Change vector from rotor (vinkelhasitghet och avstånd till rotorn) - ropas innan kraften beräknas
			public void updateSlicePosition() {
			}

			//Change internal angle of attack
			public void updateAngle() {
			}
		}

		//The slices for ths rotorblade
		private RotorbladeSlice[] slices;
	}

	//Other constants
	[SerializeField]
	private float rho = 1.225;
	[SerializeField]
	private float k = 1.0;
	[SerializeField]
	private Transform target;

	//Rotor angularl velocity
	private float w = 0.0;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		float dT = Time.deltaTime;
		//update w


	}

	// Tilt helicopter which affects normal
	void updateNormal(Vector3 rotation) {
		//TODO
	}

	void turn
}
