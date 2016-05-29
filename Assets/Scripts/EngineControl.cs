using UnityEngine;
using System.Collections;

/* Source of the mechanics calculations (swedish):
 * http://www.mek.lth.se/fileadmin/mek/Education/FMEA05oFMEA15/formelsamling.pdf
 * (Momentekvationen)
 */
public class EngineControl : MonoBehaviour {
	[SerializeField]
	private int ENGINE_MAX_EFFECT = 1016; //Engine Max Effect, both engines combined: kW
	[SerializeField]
	private int ROTOR_MASS = 500; //Rotor mass: kg, estimate
	[SerializeField]
	private int ROTORBLADE_DIAMETER = 12; 	//Rotor size: meters
	private float thrust_level = 0; //Current thrust level: 0-100 %
	private int INERTIA = 0; //Total inertia
	private float currentDT = 0.0;

	// Use this for initialization
	public void Start () {
		// Calculate the total inertia in the rotor blade system
		INERTIA = ROTOR_MASS * 2 * ROTORBLADE_DIAMETER * ROTORBLADE_DIAMETER / 12;
	}

	// Update call
	public void doUpdate(float dt) {
		currentDT = dt;

		// Check for engine increase and decrease
		if (Input.GetKey(KeyCode.W)) {
			thrust_level += dt * 50;
			if (thrust_level > 100) {
				thrust_level = 100;
			}
		} else if (Input.GetKey(KeyCode.S)) {
			thrust_level -= dt * 50;
			if (thrust_level < 0) {
				thrust_level = 0;
			}
		}
	}

	//Huvudberäkning float UpdateAngularVelocity(float dt, int enginePercent, float oldAngularVelocity)
	public float currentAngularVelocity(float prevAngularVelocity) {
		float airResistance = 2 * currentAirResistance();
		return prevAngularVelocity + ((ENGINE_MAX_EFFECT * thrust_level / prevAngularVelocity) - airResistance) * currentDT / INERTIA;
	}

	//Air resistance template function
	public float currentAirResistance() {
		return 0.0;
	}

	/*
	 * Luftmotsåtndsberäkning (air resistance)
	 * Move to Rotor Blade System once implementation shouldbe done
	 */
		/*
	LUFTMOSTÅNDSKONSTANT = map<vinkel -> motståndskoefficient>
		//luftmoståndscoefficient - kolla upp modell

	float beräkna_luftmostånd(gamla_w) {
		CD = average for each slice (LUFTMOSTÅNDSKONSTANT<attack_angle>)
		//http://www.aerospaceweb.org/question/aerodynamics/q0194.shtml
		//https://www.google.se/search?q=drag+coefficient+naca+0012&espv=2&biw=1366&bih=643&tbm=isch&imgil=fikXNCXeAk1PjM%253A%253BC0MCFhwRK8o_jM%253Bhttp%25253A%25252F%25252Fwww.aerospaceweb.org%25252Fquestion%25252Fairfoils%25252Fq0259c.shtml&source=iu&pf=m&fir=fikXNCXeAk1PjM%253A%252CC0MCFhwRK8o_jM%252C_&usg=__YQrTMptVVn60a0aubrctPlZw2BE%3D&ved=0ahUKEwjh-LSz1ffMAhUDkSwKHZ1rAGYQyjcIJg&ei=B-JGV6HZF4OisgGd14GwBg#imgrc=fikXNCXeAk1PjM%3A


		rho * |bredd_på_rotorblad| * CD * gamla_vinkelhastighet^2 * 
		|rotorbladsdiameter|^4 / 16
	} */
}
