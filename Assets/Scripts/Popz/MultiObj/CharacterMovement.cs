using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

	public float speed = 10.0f;
	public bool canJump;

	private Transform initial;

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().freezeRotation = true;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 zeroZ = new Vector3 (transform.position.x, transform.position.y, 0);
		transform.position = zeroZ;
		updateMove ();
	}

	void FixedUpdate () {
		GetComponent<Rigidbody>().AddForce (3 * Physics.gravity);
		updatePhysics ();
	}

	void updateMove () {
		Vector3 dir = new Vector3 (1, 0, 0);
		if (Input.GetKey (KeyCode.A)) {
			transform.position -= dir * speed * Time.deltaTime;
		} 
		if (Input.GetKey (KeyCode.D)) {
			transform.position += dir * speed * Time.deltaTime;
		}
	}

	void updatePhysics () {
		if (canJump && (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.Space))) {
			GetComponent<Rigidbody>().AddForce(new Vector3(0, 45, 0), ForceMode.Impulse);
		}
		if (Input.GetKey (KeyCode.S)) {
		}
	}
}
