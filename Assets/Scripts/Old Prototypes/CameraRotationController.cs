using UnityEngine;
using System.Collections;

public class CameraRotationController : MonoBehaviour {

	// time durations for rotation and translation animations
	private float rotationDuration = 1.0f;
	private float translateDuration = 0.5f;

	// enum sets to handle input control and state machines
	private enum ROTATION { NONE = 0, RIGHT = 1, LEFT = 2 };
	private enum ROTATION_STATE { TRANSFORM = 0, ROTATE = 1, PROJECT = 2, DONE = 3 };
	private enum SUB_STATE { SETUP = 0, PERFORM = 1, COMPLETE = 2 };
	private ROTATION rotating;
	private ROTATION_STATE rState;
	private SUB_STATE subState;

	// keeps track of current view
	public enum VIEW { FRONT = 0, RIGHT = 1, BACK = 2, LEFT = 3 };
	public VIEW view;

	// used to keep track of elapsed time for animations
	private float t;

	// specifies the current rotation of camera and the rotation to animate it to
	//		(it's really the rotation angle of the SpinController, which moves the camera)
	public float oldRotation;
	public float newRotation;

	// keep track of platform game objects, their real position, and positions for animating them from 2D to 3D and vice versa
	private GameObject[] platforms;
	private Vector3[] platformPos;
	private Vector3[] oldPlatformPos;
	private Vector3[] newPlatformPos;

	// keep track of modifier game objects, their real position, and positions for animating them from 2D to 3D and vice versa
	private GameObject[] mods;
	private Vector3[] modPos;
	private Vector3[] oldModPos;
	private Vector3[] newModPos;

	// keep track of player game object, and new position to move player to after a rotation
	//		(the player's position should already be correct when it is moved by its parent platform, but it doesn't hurt to be careful)
	private GameObject player;
	private PlayerController plyrCtrl;
	private Vector3 newPlayerPos;

	// id of the last platform the player has jumped onto
	public int platformID;
	
	void Start () {
		initGameObjects();
		initVars();
		projectPlatformsAndModifiersOnto2D();
		setGameObjectsIn2D();
	}

	void Update () {
		if( plyrCtrl.freeRoamMode ) {
			if( Input.GetKeyDown("right") && rotating == ROTATION.NONE ) {
				performRotateRight();
			}
			else if( Input.GetKeyDown("left") && rotating == ROTATION.NONE ) {
				performRotateLeft();
			}
		}

		if( rotating != ROTATION.NONE ) {
			rotationController();
		}
		
	}

	public void performRotateRight() {
		rotating = ROTATION.RIGHT;
		rState = ROTATION_STATE.TRANSFORM;
		subState = SUB_STATE.SETUP;
	}
	
	public void performRotateLeft() {
		rotating = ROTATION.LEFT;
		rState = ROTATION_STATE.TRANSFORM;
		subState = SUB_STATE.SETUP;
	}
	
	void initGameObjects() {
		player = GameObject.FindGameObjectWithTag("Player");
		plyrCtrl = player.GetComponent<PlayerController>();

		platforms = GameObject.FindGameObjectsWithTag("Platform");
		mods = GameObject.FindGameObjectsWithTag ("Modifier");
	}

	void initVars() {
		rotating = ROTATION.NONE;
		view = VIEW.FRONT;
		
		platformPos = new Vector3[platforms.Length];
		oldPlatformPos = new Vector3[platforms.Length];
		newPlatformPos = new Vector3[platforms.Length];
		int i = 0;
		foreach(GameObject platform in platforms) {
			platformPos[i] = platform.transform.position;
			platform.GetComponent<PlatformID>().ID = i;
			++i;
		}

		modPos = new Vector3[mods.Length];
		oldModPos = new Vector3[mods.Length];
		newModPos = new Vector3[mods.Length];
		i = 0;
		foreach(GameObject mod in mods) {
			modPos[i] = mod.transform.position;
			++i;
		}
	}

	// State machine to handle rotation sequence
	//     This sequence has 4 steps:
	//         Transform: pauses the player and moves the platforms and modifiers back to their real position in 3D
	//         Rotate: rotates the camera to the next view
	//         Project: moves the platforms and modifiers back to a 2D position relative to the new view
	//         Done: resumes the player and resets the rotating variable
	//     Each step has 3 parts (excluding Done):
	//         Setup: initialize variables, calculate new positions/rotation, etc.
	//         Perform: perform the actual animation (translation, rotation, etc)
	//         Complete: finish up step, set enum value to next step, etc.
	void rotationController() {
		switch(rState) {
			case ROTATION_STATE.TRANSFORM:
				switch(subState) {
					case SUB_STATE.SETUP:
						pausePlayer();
						transformPlatformsAndModifiersBackTo3D();
						t = 0.0f;
						subState = SUB_STATE.PERFORM;
						break;
					case SUB_STATE.PERFORM:
						if( interpolatePlatformsAndModifiers() ) {
							subState = SUB_STATE.COMPLETE;
						}
						break;
					case SUB_STATE.COMPLETE:
						rState = ROTATION_STATE.ROTATE;
						subState = SUB_STATE.SETUP;
						break;
				}
				break;
			case ROTATION_STATE.ROTATE:
				switch(subState) {
					case SUB_STATE.SETUP:
						t = 0.0f;
						oldRotation = this.transform.eulerAngles.y;
						setNewRotation();
						subState = SUB_STATE.PERFORM;
						break;
					case SUB_STATE.PERFORM:
						if( interpolateCamera() ) {
							subState = SUB_STATE.COMPLETE;
						}
						break;
					case SUB_STATE.COMPLETE:
						setNewView();
						rState = ROTATION_STATE.PROJECT;
						subState = SUB_STATE.SETUP;
						break;
				}
				break;
			case ROTATION_STATE.PROJECT:
				switch(subState) {
					case SUB_STATE.SETUP:
						t = 0.0f;
						projectPlatformsAndModifiersOnto2D();
						subState = SUB_STATE.PERFORM;
						break;
					case SUB_STATE.PERFORM:
						if( interpolatePlatformsAndModifiers() ) {
							subState = SUB_STATE.COMPLETE;
						}
						break;
					case SUB_STATE.COMPLETE:
						rState = ROTATION_STATE.DONE;
						break;
				}
				break;
			case ROTATION_STATE.DONE:
				resumePlayer();
				rotating = ROTATION.NONE;
				break;
		}
	}

	// method that sets up values for moving platforms and modifiers from their real 3D position to an appropriate 2D projection
	//     for front and back view, projects platforms and modifiers to z = 0
	//     for side views, projects platforms and modifiers to x = 0
	void projectPlatformsAndModifiersOnto2D() {
		float newX, newZ;
		int i;
		switch(view) {
			case VIEW.FRONT:
			case VIEW.BACK:
				newZ = 0.0f;
				i = 0;
				foreach(Vector3 pos in platformPos) {
					oldPlatformPos[i] = pos;
					newPlatformPos[i] = pos;
					newPlatformPos[i].z = newZ;
					++i;
				}
				i = 0;
				foreach(Vector3 pos in modPos) {
					oldModPos[i] = pos;
					newModPos[i] = pos;
					newModPos[i].z = newZ;
					++i;
				}
				break;
			case VIEW.RIGHT:
			case VIEW.LEFT:
				newX = 0.0f;
				i = 0;
				foreach(Vector3 pos in platformPos) {
					oldPlatformPos[i] = pos;
					newPlatformPos[i] = pos;
					newPlatformPos[i].x = newX;
					++i;
				}
				i = 0;
				foreach(Vector3 pos in modPos) {
					oldModPos[i] = pos;
					newModPos[i] = pos;
					newModPos[i].x = newX;
					++i;
				}
				break;
		}
	}

	// method that sets up values for moving platforms and modifiers from their current 2D projected position back to their real 3D position
	void transformPlatformsAndModifiersBackTo3D() {
		int i = 0;
		foreach( Vector3 pos in platformPos) {
			oldPlatformPos[i] = newPlatformPos[i];
			newPlatformPos[i] = pos;
			++i;
		}
		i = 0;
		foreach( Vector3 pos in modPos) {
			oldModPos[i] = newModPos[i];
			newModPos[i] = pos;
			++i;
		}
	}

	// linearly interpolate between the old position and new position
	//   lasts for 'translateDuration' seconds before reaching it's destination
	bool interpolatePlatformsAndModifiers() {
		t += Time.deltaTime / translateDuration;
		int i = 0;
		if( t >= 1.0f ) {
			foreach(GameObject platform in platforms) {
				platform.transform.position = newPlatformPos[i];
				++i;
			}
			i = 0;
			foreach(GameObject mod in mods) {
				mod.transform.position = newModPos[i];
				++i;
			}
			return true;
		}
		else {
			foreach(GameObject platform in platforms) {
				Vector3 newPos = oldPlatformPos[i] * (1.0f-t) + newPlatformPos[i] * t;
				platform.transform.position = newPos;
				++i;
			}
			i = 0;
			foreach(GameObject mod in mods) {
				Vector3 newPos = oldModPos[i] * (1.0f-t) + newModPos[i] * t;
				mod.transform.position = newPos;
				++i;
			}
			return false;
		}
	}

	// similar to interpolate platforms and modifiers, but with extra cases because of wrap around (360 degrees = 0 degrees)
	bool interpolateCamera() {
		t += Time.deltaTime / rotationDuration;
		if( t >= 1.0f ) {
			Vector3 newAngle = this.transform.eulerAngles;
			newAngle.y = newRotation;
			this.transform.eulerAngles = newAngle;
			return true;
		}
		else {
			if( oldRotation == 0.0f && newRotation == 270.0f ) {
				Vector3 newAngle = this.transform.eulerAngles;
				if( t == 0 )
					newAngle.y = oldRotation;
				else
					newAngle.y = 360.0f * (1.0f-t) + newRotation * t;
				this.transform.eulerAngles = newAngle;
				return false;
			}
			else if( oldRotation == 270.0f && newRotation == 0.0f ) {
				Vector3 newAngle = this.transform.eulerAngles;
				newAngle.y = oldRotation * (1.0f-t) + 360.0f * t;
				this.transform.eulerAngles = newAngle;
				return false;
			}
			else {
				Vector3 newAngle = this.transform.eulerAngles;
				newAngle.y = oldRotation * (1.0f-t) + newRotation * t;
				this.transform.eulerAngles = newAngle;
				return false;
			}
		}
	}

	// sets the new rotation angle to be used when interpolating the cameras rotation
	void setNewRotation() {
		switch(view) {
			case VIEW.FRONT:
				if( rotating == ROTATION.RIGHT )
					newRotation = 270.0f;
				else
					newRotation = 90.0f;
				break;
			case VIEW.RIGHT:
				if( rotating == ROTATION.RIGHT )
					newRotation = 180.0f;
				else
					newRotation = 0.0f;
				break;
			case VIEW.BACK:
				if( rotating == ROTATION.RIGHT )
					newRotation = 90.0f;
				else
					newRotation = 270.0f;
				break;
			case VIEW.LEFT:
				if( rotating == ROTATION.RIGHT )
					newRotation = 0.0f;
				else
					newRotation = 180.0f;
				break;
		}
	}

	// sets view to the correct view after performing a rotation
	void setNewView() {
		switch(view) {
			case VIEW.FRONT:
				if( rotating == ROTATION.RIGHT )
					view = VIEW.RIGHT;
				else
					view = VIEW.LEFT;
				break;
			case VIEW.RIGHT:
				if( rotating == ROTATION.RIGHT )
					view = VIEW.BACK;
				else
					view = VIEW.FRONT;
				break;
			case VIEW.BACK:
				if( rotating == ROTATION.RIGHT )
					view = VIEW.LEFT;
				else
					view = VIEW.RIGHT;
				break;
			case VIEW.LEFT:
				if( rotating == ROTATION.RIGHT )
					view = VIEW.FRONT;
				else
					view = VIEW.BACK;
				break;
			}
	}

	// function used in Start() to initially project the player, platforms, and modifiers to z = 0
	void setGameObjectsIn2D() {
		float newZ = 0.0f;
		Vector3 newPos = player.transform.position;
		newPos.z = newZ;
		player.transform.position = newPos;
		foreach(GameObject platform in platforms) {
			newPos = platform.transform.position;
			newPos.z = newZ;
			platform.transform.position = newPos;
		}
		foreach(GameObject mod in mods) {
			newPos = mod.transform.position;
			newPos.z = newZ;
			mod.transform.position = newPos;
		}
	}

	// this is sort of the equivalent of setting the rigidbody to inactive, stopping the character from moving
	void pausePlayer() {
		player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		player.GetComponent<Rigidbody>().isKinematic = true;

		// Rotate with camera (works all the time, but does not achieve the desired mechanic we're going for)
		//player.transform.parent = this.transform;

		// Stay with the last platform landed on (the desired mechanic but has some issues when playing free-range mode)
		//player.transform.parent = platforms[platformID].transform;

		// fix warping glitch
		newPlayerPos = player.transform.position;
		if( view == VIEW.FRONT || view == VIEW.BACK ) {
			newPlayerPos.z = platformPos[platformID].z;
		}
		else {
			newPlayerPos.x = platformPos[platformID].x;
		}
		player.transform.position = newPlayerPos;
	}

	// make sure that character is correctly projected onto the appropriate 2D grid and 'reactivate' the rigidbody
	void resumePlayer() {
		newPlayerPos = player.transform.position;
		if( view == VIEW.LEFT || view == VIEW.RIGHT ) {
			newPlayerPos.x = 0.0f;
		}
		else {
			newPlayerPos.z = 0.0f;
		}
		player.transform.position = newPlayerPos;
		player.transform.parent = null;
		player.GetComponent<Rigidbody>().isKinematic = false;
	}

}
