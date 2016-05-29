using UnityEngine;
using System.Collections;

public class Rotorblades : MonoBehaviour {
	//Other constants
	[SerializeField]
	public static float rho = 1.225f;
	[SerializeField]
	private static float k = 1.0f;
	[SerializeField]
	private GameObject tar;

	private static GameObject target;

	static float rotorbladeLength = 11.938f / 2; //meters (radius)
	static float weight = 4300f; //kg
	static float initialBladeRotation = -8f; //Degrees
	static float initialRotorRotation = -90; //Degrees
	static float bladeWidth = 0.18f; //meters
	static int nSlices = 36;
	static float sliceLength = rotorbladeLength / nSlices;
	//Rotor angularl velocity
	private static float w = 0.0001f; //Magnitude only (!) Vector is always up..

	private ArrayList rotorblades = new ArrayList ();


	//Inner class for each rotorblade
	public class Rotorblade : MonoBehaviour {
		//Inner class representing a slice
		Vector3 endPos = new Vector3(0,0,0);
		//The slices for ths rotorblade
		ArrayList slices = new ArrayList();

		public void initBlade(Vector3 endPos, float rot) {
			this.endPos = endPos * rotorbladeLength;
			this.transform.position = target.transform.position;
			this.transform.localPosition = endPos;
			for(int i = 0; i < nSlices; ++i) {
				GameObject s = new GameObject ();
				s.AddComponent<RotorbladeSlice> ();
				RotorbladeSlice rs = s.GetComponent<RotorbladeSlice> ();
				rs.initSlice (i * sliceLength, (i + 1) * sliceLength);
				s.transform.SetParent(this.transform); //Setting transform to component
				slices.Add (s);
			}
			//this.rotateAngle (new Vector3 (0, Mathf.Deg2Rad *initialRotorRotation, Mathf.Deg2Rad*initialBladeRotation)); //Rotate around y axis (FIX)
			this.rotateAngle(new Vector3(-98, 90*rot, 0));
		}


		//Return force for each slice in this rotorblade
		public float getForce() {
			float res = 0.0f;
			for(int i = 0; i < slices.Count; ++i) {
				RotorbladeSlice s = ((GameObject) slices [i]).GetComponent<RotorbladeSlice> ();
				res += s.getForce ();
			}
			return res;
		}

		//Important to rotate correctly...
		public void rotateAngle(Vector3 rot) {
			this.transform.Rotate(rot);
		}

		//Needed??
		public void updateBladePosition(Vector3 pos) {
			this.transform.position = pos;
		}

		public class RotorbladeSlice : MonoBehaviour {
			//Constructor
			float area = 0.0f;
			float disToRotor = 0.0f;
			public void initSlice(float sPos, float ePos) {
				this.area = (ePos - sPos) * bladeWidth; //Kinda the same for evry..
				this.disToRotor = ePos - (ePos - sPos)/2; //Center of slice
				this.transform.localPosition = this.getVectorFromRotor();  //init
			}
		
			//Ui
			public Vector3 getAirflowVelocity() {
	
				Vector3 wVector = this.transform.parent.up * w;
				Vector3 y = this.getVectorFromRotor ();
				Vector3 ubody = target.GetComponent<Rigidbody> ().velocity; //Not sure about this one
				return ubody + Vector3.Cross(y, wVector); 
			}

			//Normal
			public Vector3 getNormal() {
				Vector3 a = this.transform.position;
				Vector3 b = a + new Vector3(sliceLength / 2, 0.0f, bladeWidth / 2);
				Vector3 c = a  + new Vector3(sliceLength / 2, 0.0f, -bladeWidth / 2);
				Vector3 perp = Vector3.Cross(b-a, c-a);
				return perp.normalized; 
			}

			//Size
			public float getArea() {
				return area;
			}

			//Rotor - slice direction (yi)
			public Vector3 getVectorFromRotor() {
				return this.transform.localPosition * disToRotor; 
			}
				
			
			//Fi
			public float getForce() {
				float a = Vector3.Dot (rho * this.getAirflowVelocity (), this.getNormal());
				float b = Vector3.Dot (a * this.getArea () * this.getAirflowVelocity (), this.getNormal ());
				float f = k * b * nSlices;
				return f;
			}
		}

	}

	//Rotate all blades on the z axis with the specified amount.
	public void rotateBlades(float amount) {

		for (int i = 0; i < rotorblades.Count; ++i) {
			GameObject b = (GameObject)rotorblades [i];
			Rotorblade rb = b.GetComponent<Rotorblade> ();
			rb.rotateAngle (new Vector3 (amount, 0, 0)); //z x y
		}
	}



	// Use this for initialization
	Vector3[] heliPos = {new Vector3(1,0,0), new Vector3(0,0,-1), new Vector3(-1, 0,0),  new Vector3(0,0,1)};


	void rotateBlades(Vector3 rot) {
		for (int i = 0; i < rotorblades.Count; ++i) {
			GameObject go = (GameObject) rotorblades [i];
			Rotorblade rbb = go.GetComponent<Rotorblade> ();
			rbb.rotateAngle (rot);
		}
	}
	void Start () {
		target = tar;
		for (int i = 0; i < heliPos.Length; ++i) {
			GameObject r = new GameObject ();
			Rotorblade rb = r.AddComponent <Rotorblade> ();
			rb.initBlade(heliPos[i], i);
			//r.transform.SetParent (this.transform); //Setting component parent = setting gameobject parent
			//r.transform.SetParent(target.transform);
			rotorblades.Add (r);
		}
		target.AddComponent<EngineControl> ();
		target.AddComponent<TiltingControl> ();
		target.AddComponent<spin> ();
	}
		
	// Update is called once per frame
	void Update () {
		float dT = Time.deltaTime;

		//Engine
		EngineControl ec = target.GetComponent<EngineControl> ();
		ec.doUpdate (dT);
		w = ec.currentAngularVelocity (w);

		//Tilting
		TiltingControl tc = target.GetComponent<TiltingControl> ();
		Vector3 tiltRotations = tc.doUpdate(dT);
		//RotorBlades.rotateBlades (tiltRotations);
		target.transform.Rotate(tiltRotations);

		if (Input.GetKey (KeyCode.A)) {
			target.transform.Rotate (0, -40 * dT, 0);
		//	rotateBlades(new Vector3 (0, 0, 10 * dT));
				
		} else if (Input.GetKey (KeyCode.D)) {
			target.transform.Rotate (0, 40 * dT, 0);
		//	rotateBlades(new Vector3 (0, 0, -10 * dT));
		}

		spin spinner = target.GetComponent<spin> ();
		Vector3 rotorSpin = spinner.spinRotor (w, ec.getThrust());
		var rotor = target.transform.Find ("Rotor_Control");
//		print ("ROTATING... " + rotorSpin);
		rotor.transform.Rotate (rotorSpin);

		float forceMag = 0.0f;
		for (int i = 0; i < rotorblades.Count; ++i) {
			GameObject g = (GameObject)rotorblades [i];
			Rotorblade rb = g.GetComponent<Rotorblade> ();
			forceMag += rb.getForce ();
		}
//		print ("Angular Velocity: " + w);
//		print ("Force size: " + forceMag);
//		print ("Target velocity:" + target.GetComponent<Rigidbody> ().velocity);
		target.GetComponent<Rigidbody> ().AddForce (target.transform.up * forceMag); //Should be correct
	}



}

