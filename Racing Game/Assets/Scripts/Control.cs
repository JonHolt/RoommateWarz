using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
    public float maxSpeed = 05f;
    public float accel = 15f;
    public float turning = 3f;
    public float hoverForce = 3f;

    Rigidbody car;

    void Start() {
        car = GetComponent<Rigidbody>();
    }

	void FixedUpdate () {
        if (Input.GetButton("Gas")) {
            car.AddForce(transform.forward * accel);
            if(car.velocity.magnitude > maxSpeed) {
                car.velocity.Normalize();
                car.velocity *= maxSpeed;
            }
        }
        if (Input.GetButton("Brake")) {
            car.AddForce(transform.forward * accel / -2);
            if (car.velocity.magnitude < maxSpeed / -2) {
                car.velocity.Normalize();
                car.velocity *= maxSpeed / -2;
            }
        }
        if(Input.GetAxis("Horizontal") != 0) {
            transform.Rotate(new Vector3(0, 1, 0), turning * Input.GetAxis("Horizontal"));
        }
	}

    void OnTriggerEnter(Collider other) {
        car.AddForce(transform.up * hoverForce);
    }
    void OnTriggerStay(Collider other) {
        car.AddForce(transform.up * hoverForce);
    }
}
