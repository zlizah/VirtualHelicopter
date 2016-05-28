using UnityEngine;
using System.Collections;

public class spin : MonoBehaviour {

    [SerializeField]
    private Transform target;
    

    [SerializeField]
    private float acc = 10.0f;
    
    [SerializeField]
    private float dec = -10.0f;
    
    [SerializeField]
    private float maxSpeed = 1000;
    
    private float speed = 0.0f;
    

	
	// Update is called once per frame
	void Update () {
        
        if(Input.GetKey("w")) {
            if(speed < maxSpeed) {
                if(speed + acc * Time.deltaTime >= maxSpeed) {
                    speed = maxSpeed;
                } else {
                    speed = speed + acc * Time.deltaTime;
                }
            }

        } else {
            if(speed > 0) {
                if(speed + dec * Time.deltaTime <= 0) {
                    speed = 0;
                } else {
                    speed = speed + dec * Time.deltaTime;
                }
            }
        }
        
		target.transform.Rotate(0, 0, -speed * Time.deltaTime);
	}
}
