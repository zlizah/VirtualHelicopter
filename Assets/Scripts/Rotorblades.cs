using UnityEngine;
using System.Collections;

public class RotorbladeSystem : MonoBehaviour {
	//Other constants
	[SerializeField]
	private static float rho = 1.225f;
	[SerializeField]
	private static float k = 1.0f;
	[SerializeField]
	private GameObject target;

	static float rotorbladeLength = 11.938f; //meters
	static float weight = 4300f; //kg
	static float initialBladeRotation = -8f; //Degrees
	static float bladeWidth = 0.18f; //meters
	static int nSlices = 36;
	static float sliceLength = rotorbladeLength / nSlices;
	//Rotor angularl velocity
	private float w = 0.0f;

	private ArrayList rotorblades = new ArrayList ();


	//Inner class for each rotorblade
	public class Rotorblade : MonoBehaviour {
		//Inner class representing a slice
		Vector3 endPos = new Vector3(0,0,0);
		//The slices for ths rotorblade
		ArrayList slices = new ArrayList();

		public void initBlade(Vector3 endPos) {
			this.endPos = endPos * rotorbladeLength;

			this.transform.localPosition = endPos;
			for(int i = 0; i < nSlices; ++i) {
				GameObject s = new GameObject ();
				s.AddComponent<RotorbladeSlice> ();
				s.GetComponent<RotorbladeSlice> ().initSlice (i * sliceLength, (i + 1) * sliceLength);
				s.transform.SetParent(this.transform);
				slices.Add (s);
			}
		}

		//Return force for each slice in this rotorblade
		public float getForce() {
			float res = 0.0f;
			for(int i = 0; i < slices.Count; ++i) {
				RotorbladeSlice s = (RotorbladeSlice) slices [i];
				res += s.getForce ();
			}
			return res;
		}

		public class RotorbladeSlice : MonoBehaviour {
			//Constructor
			float area = 0.0f;
			float disToRotor = 0.0f;
			public void initSlice(float sPos, float ePos) {
				this.area = (ePos - sPos) * bladeWidth; //Kinda the same for evry..
				this.disToRotor = ePos - (ePos - sPos)/2; //Center of slice
				this.transform.position = this.getVectorFromRotor(); //?
				this.transform.Rotate(initialBladeRotation, 0, 0); //Eh..rotate around z or x axis..? Or both..
			}

		
			//UiSlice


			//Ui
			public Vector3 getAirflowVelocity() {
				Vector3 y = this.getVectorFromRotor();
				return y; //TODO!!
			}

			//Normal
			public Vector3 getNormal() {
				Vector3 a = this.transform.position;
				Vector3 b = a + new Vector3(sliceLength / 2, 0.0f, bladeWidth / 2);
				Vector3 c = a  + new Vector3(sliceLength / 2, 0.0f, -bladeWidth / 2);
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
				float a = Vector3.Dot (rho * this.getAirflowVelocity (), this.getNormal());
				float b = Vector3.Dot (a * this.getArea () * this.getAirflowVelocity (), this.getNormal ());
				float f = k * b * nSlices;
				return f;
			}
		}

	}


	// Use this for initialization
	void Start () {
		GameObject r1 = new GameObject ();
		Rotorblade r1b = r1.AddComponent <Rotorblade> ();
		r1b.initBlade(new Vector3 (0, 0, 1));

		GameObject r2 = new GameObject ();
		Rotorblade r2b = r2.AddComponent <Rotorblade> ();
		r2b.initBlade(new Vector3 (1, 0, 0));

		GameObject r3 = new GameObject ();
		Rotorblade r3b = r3.AddComponent <Rotorblade> ();
		r3b.initBlade(new Vector3 (0, 0, -1));

		GameObject r4 = new GameObject ();
		Rotorblade r4b = r4.AddComponent <Rotorblade> ();
		r4b.initBlade(new Vector3 (-1, 0, 0));

		rotorblades.Add (r1);
		rotorblades.Add (r2);
		rotorblades.Add (r3);
		rotorblades.Add (r4);
		//Todo: Set parent to target..?
	}

	// Update is called once per frame
	void Update () {
		float dT = Time.deltaTime;
		//update w
		float forceMag = 0.0f;
		for (int i = 0; i < rotorblades.Count; ++i) {
			Rotorblade rb = (Rotorblade)rotorblades [i];
			forceMag += rb.getForce ();
		}
		target.GetComponent<Rigidbody> ().AddForce (target.transform.up * forceMag); //Should be correct
	}

	// Tilt helicopter which affects normal
	void updateNormal(Vector3 rotation) {
		//TODO
	}


}
