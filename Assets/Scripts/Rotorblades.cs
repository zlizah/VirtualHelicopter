using UnityEngine;
using System.Collections;

public class RotorbladeSystem : MonoBehaviour {
	//Other constants
	[SerializeField]
	private float rho = 1.225;
	[SerializeField]
	private float k = 1.0;
	[SerializeField]
	private Transform target;

	static float rotorbladeLength = 11.938; //meters
	static float weight = 4300; //kg
	static float initialBladeRotation = -8; //Degrees
	static float bladeWidth = 0.18; //meters
	static int nSlices = 36;
	static float sliceLength = rotorbladeLength / nSlices;
	//Rotor angularl velocity
	private float w = 0.0;

	private ArrayList rotorblades = new ArrayList ();

	public RotorbladeSystem() {	
		Rotorblade r1 = new Rotorblade (Vector3 (0, 0, 1));
		Rotorblade r2 = new Rotorblade (Vector3 (1, 0, 0));
		Rotorblade r3 = new Rotorblade (Vector3 (0, 0, -1));
		Rotorblade r4 = new Rotorblade (Vector3 (-1, 0, 0));
		rotorblades.Add (r1);
		rotorblades.Add (r2);
		rotorblades.Add (r3);
		rotorblades.Add (r4);
		//Todo: Set parent to target..?
	}

	//Inner class for each rotorblade
	public class Rotorblade : MonoBehaviour {
		//Inner class representing a slice
		Vector3 endPos = null;
		//The slices for ths rotorblade
		ArrayList slices = new ArrayList();

		public Rotorblade(Vector3 endPos) {
			this.endPos = endPos * rotorbladeLength;

			this.transform.localPosition = endPos;
			for(int i = 0; i < nSlices; ++i) {
				RotorbladeSlice s = new RotorbladeSlice(i*sliceLength, (i+1)*sliceLength);
				s.transform.SetParent(this.transform);
			}
		}

		public class RotorbladeSlice : MonoBehaviour {
			//Constructor
			float area = 0.0;
			float disToRotor = 0.0;
			public RotorbladeSlice(float sPos, float ePos) {
				this.area = (ePos - sPos) * bladeWidth; //Kinda the same for evry..
				this.disToRotor = ePos - (ePos - sPos)/2; //Center of slice
				this.transform.position = this.getVectorFromRotor(); //?
				this.transform.Rotate(initialBladeRotation, 0, 0); //Eh..rotate around z or x axis..? Or both..
			}

		
			//UiSlice


			//Ui
			public Vector3 getAirflowVelocity() {
				Vector3 y = this->getVectorFromRotor;
			}

			//Normal
			public Vector3 getNormal() {
				Vector3 a = this.transform.position;
				Vector3 b = this.transform.position + (bladeWidth / 2) + (sliceLength / 2);
				Vector3 c = this.transform.position - (bladeWidth / 2); + (sliceLength / 2);
				Vector3 perp = Vector3.Cross(b-a, c-a);
				return perp.normalized; //Normalized normal.. HAH!
			}

			//Size
			public float getArea() {
				return area;
			}

			//Rotor - slice direction (yi)
			public Vector3 getVectorFromRotor() {
				return this.transform.parent.localPosition * disToRotor; //Correct?
			}

			//Change vector from rotor (vinkelhasitghet och avstånd till rotorn) - ropas innan kraften beräknas
			public void updateSlicePosition() {
			}

			//Change internal angle of attack
			public void updateAngle() {
			}

			public float getForce() {
				float f = k*(rho*
					this.getAirflowVelocity()*
					this.getNormal()*
					this.getArea()*
					this.getAirflowVelocity()*
					this.getNormal())*nSlices;
				return f;
			}
		}

	}


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
