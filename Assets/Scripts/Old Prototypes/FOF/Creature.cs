using UnityEngine;
using System.Collections;

public class Creature : MonoBehaviour {

	public WayPoint topBound;
	public WayPoint bottomBound;
	public WayPoint leftBound;
	public WayPoint rightBound;

	private bool cloaked;
	private Material initMaterial;
	private Mesh initMesh;

	private GameObject cloakObjectRef;
	private float cloakTicker;
	private float uncloakWaitTime = 3;

	public Vector3 target;
	private float speed = 3;

	private float moveTicker;
	private float moveWaitTime = 2f;

	private int direction;

	private bool canBeSelected;

	public LevelManager manager;
	private bool endAnimationPlay;
	public WayPoint endPoint;

	// Use this for initialization
	void Start () {
		endAnimationPlay = false;
		moveTicker = 0;
		cloakTicker = -5;
		cloaked = false;
		canBeSelected = false;
		direction = 1;

		initMaterial = this.GetComponent<Renderer>().material;
		initMesh = this.GetComponent<MeshFilter>().mesh;

		target = transform.position;
		cloakObjectRef = GameObject.FindGameObjectWithTag("cloakObj");

		leftBound = GameObject.FindGameObjectWithTag("bPoint1").GetComponent<WayPoint>();
		rightBound = GameObject.FindGameObjectWithTag("bPoint2").GetComponent<WayPoint>();
		topBound = GameObject.FindGameObjectWithTag("bPoint4").GetComponent<WayPoint>();
		bottomBound = GameObject.FindGameObjectWithTag("bPoint3").GetComponent<WayPoint>();
//		currentTarget = 5;
	}
	
	// Update is called once per frame
	void Update () {
		if (!endAnimationPlay) {
			moveTicker += Time.deltaTime;
			cloakTicker += Time.deltaTime;

			moveUpdate();
			changeTarget();
			setCloak();
	//		checkReveal();
			Vector3 zeroZ = new Vector3(transform.position.x, transform.position.y, 0);
			transform.position = zeroZ;
			checkInBounds();
			checkEndState();
		}
		else {
			target = endPoint.transform.position;
			moveUpdate();
		}
	}

	private void moveUpdate () {
		float step = speed * Time.deltaTime;
//		transform.position = Vector3.MoveTowards(transform.position, transform.forward, step);
		transform.Translate(Vector3.forward * step);
		Vector3 targetDir = target - transform.position;
		targetDir.x *= direction;
		targetDir.y *= direction;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
		Debug.DrawRay(transform.position, newDir, Color.red);
		transform.rotation = Quaternion.LookRotation(newDir);
	}

	private void setCloak () {
		if (cloakTicker >= uncloakWaitTime) {
			cloaked = true;
			canBeSelected = true;
		}

		if (cloaked) {
			this.GetComponent<MeshFilter>().mesh = cloakObjectRef.GetComponent<MeshFilter>().mesh;
			GetComponent<Renderer>().material = cloakObjectRef.GetComponent<Renderer>().material;
		}
		else {
			this.GetComponent<MeshFilter>().mesh = initMesh;
			GetComponent<Renderer>().material = initMaterial;
		}
	}

	private void changeTarget () {
		if (moveTicker >= moveWaitTime) {
//		    || transform.position == target 
//		    || transform.position.x > rightBound.transform.position.x
//		    || transform.position.x < leftBound.transform.position.x
//		    || transform.position.y > topBound.transform.position.y
//		    || transform.position.y < bottomBound.transform.position.y) 
		
//			Debug.Log("entering change target");
//			float xMin = GameObject.FindGameObjectWithTag("bPoint1").transform.position.x;
//			float xMax = GameObject.FindGameObjectWithTag("bPoint2").transform.position.x;
//			float yMax = GameObject.FindGameObjectWithTag("bPoint3").transform.position.y;
//			float yMin = GameObject.FindGameObjectWithTag("bPoint4").transform.position.y;

			float xMin = leftBound.transform.position.x;
			float xMax = rightBound.transform.position.x;
			float yMax = topBound.transform.position.y;
			float yMin = bottomBound.transform.position.y;

			Vector3 newTarget = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0);

			target = newTarget;
			moveTicker = 0;
			direction = 1;
		}
	}

	public void reveal () {
//		if (Input.GetKey(KeyCode.Space)) {
			cloaked = false;
			cloakTicker = 0;
			canBeSelected = false;
//		}
	}
/*
	void OnGUI() {
		if (GUI.Button(new Rect(10, Screen.height - 60, 50, 50), "Light")) {
			cloaked = false;
			cloakTicker = 0;
			canBeSelected = false;
		}
	}
*/
	private void checkInBounds () {
		float xMin = leftBound.transform.position.x;
		float xMax = rightBound.transform.position.x;
		float yMax = topBound.transform.position.y;
		float yMin = bottomBound.transform.position.y;

		if (transform.position.x < xMin || transform.position.x > xMax
		    || transform.position.y < yMin) {

			transform.position = new Vector3(xMin + 1, yMax - 1, 0);
			moveTicker = moveWaitTime;
		}
		else if (transform.position.y > yMax) {
			transform.position = new Vector3(transform.position.x, yMax - 1, 0);
			moveTicker = moveWaitTime;
		}
	}

	private void checkEndState () {
		if (!manager.getState()) {
			leftBound.GetComponent<BoxCollider>().enabled = false;
			canBeSelected = false;
			endAnimationPlay = true;
			speed = 60;
			direction = 1;
		}
	}

	public bool getSelectable () {
		return canBeSelected;
	}

	void OnCollisionEnter(Collision collision) {
		direction *= -1;
		moveTicker = 0;
	}
}
