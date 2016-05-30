using UnityEngine;
using System.Collections;
using System;

/* Source of the mechanics calculations (swedish):
 * http://www.mek.lth.se/fileadmin/mek/Education/FMEA05oFMEA15/formelsamling.pdf
 * (Momentekvationen)
 */
public class EngineControl : MonoBehaviour {
	[SerializeField]
	private int ENGINE_MAX_EFFECT = 100000; //Engine Max Effect, both engines combined: kW
	[SerializeField]
	private int ROTOR_MASS = 500; //Rotor mass: kg, estimate
	[SerializeField]
	private int ROTORBLADE_DIAMETER = 12; 	//Rotor size: meters
	private float thrust_level = 0f; //Current thrust level: 0-100 %
	private int INERTIA = 0; //Total inertia
	private float currentDT = 0.0f;
	private float realism_constant = 0.1f;

	// Use this for initialization
	public void Start () {
		// Calculate the total inertia in the rotor blade system
		INERTIA = ROTOR_MASS * 2 * ROTORBLADE_DIAMETER * ROTORBLADE_DIAMETER / 12;
	}

	// Update call
	public void doUpdate(float dt) {
		currentDT = dt;

		// Check for engine increase and decrease
		if (TiltingControl.inputSelector == 0) {
			if (Input.GetKey (KeyCode.W)) {
				thrust_level += dt * 50;
				if (thrust_level > 100) {
					thrust_level = 100;
				}
			} else if (Input.GetKey (KeyCode.S)) {
				thrust_level -= dt * 50;
				if (thrust_level < 0) {
					thrust_level = 0;
				}
			}
		} else {
			thrust_level = ((Input.GetAxisRaw ("Engine-Axis") + 1) * 50);
		}
	}

	//Huvudberäkning float UpdateAngularVelocity(float dt, int enginePercent, float oldAngularVelocity)
	public float currentAngularVelocity(float prevAngularVelocity) {
	//	print ("Curent engine thrust: " + thrust_level);
		float airResistance = 2 * currentAirResistance(prevAngularVelocity);
		return prevAngularVelocity + ((ENGINE_MAX_EFFECT * thrust_level / (100 * prevAngularVelocity)) - airResistance) * currentDT / INERTIA;
	}

	//Air resistance template function
	public float currentAirResistance(float angularVelocity) {
		float cd = 0.02f; //air resistance coefficient, should be based on attack angle
		float rho = Rotorblades.rho;
		return (float)(rho * (double)ROTORBLADE_DIAMETER 
			* cd 
			* Math.Pow((double)angularVelocity, 2) 
			* Math.Pow((double)ROTORBLADE_DIAMETER, 4) 
			) / 16.0f;
	}

	public float getThrust() {
		return thrust_level;
	}
}
