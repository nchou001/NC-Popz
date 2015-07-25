using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private float walkVel = 1.0f;	// velocity to apply to make the character move
	private float walkMax = 2.0f;	// max velocity to restrict character from moving too fast
	private float jumpVel = 5.0f;	// amount of velocity to apply to make the character jump
	public bool isMovingRight, isMovingLeft, isJumping;	// flags to interface w/ input controls

	public bool boostMod;
	private float boostAnimTimer;
	private float boostAnimDuration = 0.5f;

	private CameraRotationController camRotCtrl;	// variable to interface with CameraRotationController script

	public bool freeRoamMode = false;

	void Start () {
	
		// init variables
		GameObject spinCtrl = GameObject.FindGameObjectWithTag("SpinController");
		camRotCtrl = spinCtrl.GetComponent<CameraRotationController>();

		if( freeRoamMode ) {
			isMovingRight = false;
			isMovingLeft = false;
			isJumping = false;
		}
		else {
			isMovingRight = true;
			isMovingLeft = false;
			isJumping = false;
		}

		boostMod = false;
		boostAnimTimer = 0.0f;

		// restrict the character from moving in the z-direction
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
	}
	
	void Update () {
	
		//===== Input Interface =====//
	
		if( freeRoamMode ) {
			if( Input.GetKey(KeyCode.D) ) {
				isMovingRight = true;
			}
			else {
				isMovingRight = false;
			}

			if( Input.GetKey(KeyCode.A) ) {
				isMovingLeft = true;
			}
			else {
				isMovingLeft = false;
			}
		}

		if( Input.GetKeyDown(KeyCode.Space) ) {
			isJumping = true;
		}
		else {
			isJumping = false;
		}

		// update platform ID
		setPlatformID();
	}

	void FixedUpdate() {
	
		// when the rigidbody is kinematic, it won't move, so exit FixedUpdate()
		if( GetComponent<Rigidbody>().isKinematic ) return;

		// otherwise, perform movements if input was given
		if( boostMod ) {
			if( boostAnimTimer >= boostAnimDuration ) {
				boostMod = false;
				boostAnimTimer = 0.0f;
			}
			else {
				boostAnimTimer += Time.deltaTime;
			}
		}
		else {
			if( isMovingRight ) {
				moveRight();
			}

			if( isMovingLeft ) {
				moveLeft();
			}

			if( isJumping ) {
				GetComponent<Rigidbody>().velocity += new Vector3(0, jumpVel, 0);
			}
		}
	}

	void moveRight() {
		switch(camRotCtrl.view) {	// movement is determined by the current view
			case CameraRotationController.VIEW.FRONT:
				GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;	// lock z-movement
				if( GetComponent<Rigidbody>().velocity.x >= walkMax ) return;
				GetComponent<Rigidbody>().velocity += new Vector3(walkVel, 0, 0);	// move in +x-direction
				break;
			case CameraRotationController.VIEW.RIGHT:
				GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;	// lock x-movement
				if( GetComponent<Rigidbody>().velocity.z >= walkMax ) return;
				GetComponent<Rigidbody>().velocity += new Vector3(0, 0, walkVel);	// move in +z-direction
				break;
			case CameraRotationController.VIEW.BACK:
				GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;	// lock z-movement
				if( GetComponent<Rigidbody>().velocity.x <= -walkMax ) return;
				GetComponent<Rigidbody>().velocity += new Vector3(-walkVel, 0, 0);	// move in -x-direction
				break;
			case CameraRotationController.VIEW.LEFT:
				GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;	// lock x-movement
				if( GetComponent<Rigidbody>().velocity.z <= -walkMax ) return;
				GetComponent<Rigidbody>().velocity += new Vector3(0, 0, -walkVel);	// move in -z-direction
				break;
		}
	}

	void moveLeft() {	// same as moveRight() just in the opposite direction
		switch(camRotCtrl.view) {
		case CameraRotationController.VIEW.FRONT:
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
			if( GetComponent<Rigidbody>().velocity.x <= -walkMax ) return;
			GetComponent<Rigidbody>().velocity += new Vector3(-walkVel, 0, 0);
			break;
		case CameraRotationController.VIEW.RIGHT:
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
			if( GetComponent<Rigidbody>().velocity.z <= -walkMax ) return;
			GetComponent<Rigidbody>().velocity += new Vector3(0, 0, -walkVel);
			break;
		case CameraRotationController.VIEW.BACK:
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
			if( GetComponent<Rigidbody>().velocity.x >= walkMax ) return;
			GetComponent<Rigidbody>().velocity += new Vector3(walkVel, 0, 0);
			break;
		case CameraRotationController.VIEW.LEFT:
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
			if( GetComponent<Rigidbody>().velocity.z >= walkMax ) return;
			GetComponent<Rigidbody>().velocity += new Vector3(0, 0, walkVel);
			break;
		}
	}

	void setPlatformID() {
		RaycastHit hit;
		if( Physics.Raycast(this.transform.position, -Vector3.up, out hit) ) {
			if( hit.transform.gameObject.tag == "Platform" ) {
				camRotCtrl.platformID = hit.transform.gameObject.GetComponent<PlatformID>().ID;
			}
		}
	}

}
