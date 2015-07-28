using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	// Waypoint System Parameters 
	// TODO: CHANGE TO SIMPLE RECTANGLE OBJECT
	public GameObject field;
	public WayPoint wayPointReference;
	private WayPoint topBound;
	private WayPoint bottomBound;
	private WayPoint leftBound;
	private WayPoint rightBound;
	private WayPoint endPoint;
	
	//	private float tChange = 0; // force new direction in the first Update
	//	private float randomX;
	//	private float randomY;
	//	private Vector3 testLocation;
	
	// Movement Parameters
	public Vector3 target;
	public float pushSpeed = 0.35f;
	public float push = 0.3f;
	public float speed = 3.0f;
	public bool plantIsAlive = false;
	private float plantTimer = 0;
	private float swipeTimer = 0;
	private bool pushingBug = false;
	public bool plantTouched = false;
	private bool bugLeavesScene = false;
	private bool isSwiped = false;
	
	private float moveTicker;
	private float moveWaitTime = 3f;
	private float objVelocityX = 0f;
	private float objVelocityY = 1f;
	private float extraSpeed = 1f;
	
	private int direction;
	private float lockPos = 0;
	private Vector2 plantSpeed = (Vector2.right * 3.5f);
	private Vector2 movementSpeed = new Vector2(4.5f, 0.0f);
	private Vector2 translateBy;
	
	// Animations
	public Transform touchSprite;
	private bool endAnimationPlay;
	
	private GameObject plant;
	
	// Use this for initialization
	void Start () {	
		FieldInfo info = Util.getFieldInfo (field);
		
		var bottomLeftCorner = new Vector3 (info.lowerX, info.lowerY, 0);
		var upperRightCorner = new Vector3 (info.upperX, info.upperY, 0);
		
		var blank = wayPointReference;
		topBound = Instantiate (blank, upperRightCorner, transform.rotation) as WayPoint;
		bottomBound = Instantiate (blank, bottomLeftCorner, transform.rotation) as WayPoint;
		leftBound = Instantiate (blank, bottomLeftCorner, transform.rotation) as WayPoint;
		rightBound = Instantiate (blank, upperRightCorner, transform.rotation) as WayPoint;
		
		endAnimationPlay = false;
		
		objVelocityX = Random.Range(-3.0f, 3.0f);
		objVelocityY = Random.Range (1f, 2.0f);
		
		translateBy = movementSpeed;
		transform.Translate (translateBy * Time.deltaTime);
		
	}
	
	// Update is called once per frame
	void Update () {
		//create a fixed rotation
		transform.rotation = Quaternion.Euler (lockPos, lockPos, lockPos);
		
		//		//check if the bug is in bounds before making any new changes
		//		keepInBounds ();
		
		var bottomLeftCorner = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0));
		var upperRightCorner = Camera.main.ViewportToWorldPoint (new Vector3 (1f, 1f, 0));
		
		topBound.transform.position = upperRightCorner;
		bottomBound.transform.position = bottomLeftCorner;
		leftBound.transform.position = bottomLeftCorner;
		rightBound.transform.position = upperRightCorner;
		
		//If plant leave is called, move bug off screen and remove
		if (bugLeavesScene == false) {
			
			//check if the bug is in bounds before making any new changes
			keepInBounds ();
			
			//If the timer is below zero, then time to update the target, velocity, and timer.
			if (moveTicker <= 0) {
				//These two values moves the bug around based on velocity
				objVelocityX = Random.Range (-3.0f, 3.0f);
				objVelocityY = Random.Range (1f, 2.0f);
				
				//Gets a random speed bonus for the velocity
				extraSpeed = Random.Range (1.0f, 1.5f);
				
				//Changes the timer (keeps between 3 to 4.5 seconds
				moveTicker = Random.Range (2.0f, 3.5f);
				
				//			Debug.Log("objVelocity: " + objVelocity);
				//			Debug.Log("moveTicker: " + moveTicker);
				
				//add something to check if the plant is close enough
			}
			
			//If both objVelocityX and objVelocityY is too slow, then get new set of values
			if ((objVelocityX < 0.8f) && (objVelocityX > -0.8f)) {
				if ((objVelocityY < 0.8f) && (objVelocityY > -0.8f)) {
					moveTicker = 0f;
				}
			}
			
			if(swipeTimer <= 0){
				isSwiped = false;
			}
			
			//moveTicker is decreased by time.deltatime. basically a timer
			moveTicker -= Time.deltaTime;
			plantTimer -= Time.deltaTime;
			swipeTimer -= Time.deltaTime;
			
			//Gets plant and bug's current position
			plant = GameObject.FindWithTag ("NewPlant");
			if (plant) {
				var plantCurrentPos = plant.transform.position;
				var bugCurrentPos = this.transform.position;
				
				float distanceX = Mathf.Abs (bugCurrentPos.x - plantCurrentPos.x);
				float distanceY = Mathf.Abs (bugCurrentPos.y - plantCurrentPos.y);
				
				if ((distanceX <= 6.0f) && (distanceY <= 6.0f)) {
					Debug.Log ("Bug is close enough to plant");
					
					//var exclamationMark = this.transform.FindChild ("bugToPlantMarker");
					//exclamationMark.gameObject.SetActive (true);
					
					//if(isSwiped == false){
					//	if (plantTouched == false) {
							//						float step = 2.0f * Time.deltaTime;
							//						transform.position = Vector2.MoveTowards (bugCurrentPos, plantCurrentPos, step);
							////						objVelocityX = 0;
							////						objVelocityY = 0;
							
							//Translate toward plant
					//		Vector3 targetDir = plantCurrentPos - transform.position;
					//		targetDir.Normalize ();
					//		transform.Translate (targetDir * 2.0f * Time.deltaTime);
					//		objVelocityX = 0;
					//		objVelocityY = 0;
					//	}
						
					//	moveTicker = 2.0f;
					}
				// else {
					//var exclamationMark = this.transform.FindChild ("bugToPlantMarker");
					//exclamationMark.gameObject.SetActive (false);
				//}
			}
			//If there is no bug, then remove exclamation point object
			//else {
				//var exclamationMark = this.transform.FindChild ("bugToPlantMarker");
				//exclamationMark.gameObject.SetActive (false);
			}
		    if (bugLeavesScene == true) {
			var bugCurrentPos = this.transform;
			//			objVelocityX = 3.5f;
			//			objVelocityY = 3.0f;
			bugCurrentPos.localScale = new Vector3(-1, 1, 1);
			bugCurrentPos.transform.Translate(new Vector3( -1.0f, 1.0f, 0) * 2.5f * Time.deltaTime);
			if (bugCurrentPos.transform.position.x < leftBound.transform.position.x - 1) {
				Destroy (this.gameObject);
			}
		}
		
		updateMovement ();
	}
	
	
	
	
	
	//updatePosition(): updates the position by incrementing by x and y
	private void updateMovement(){
		
		float xMin = leftBound.transform.position.x;
		float xMax = rightBound.transform.position.x;
		float yMax = topBound.transform.position.y;
		float yMin = bottomBound.transform.position.y;
		
		var currentBugRigidBody = GetComponent<Rigidbody2D> ();
		currentBugRigidBody.velocity = new Vector2(objVelocityX * extraSpeed, objVelocityY * extraSpeed);
		//		Debug.Log ("extraSpeed: " + extraSpeed);
		if (objVelocityX < 0.0f) {
			if(this.transform.localScale.x > -1.0f){
				this.transform.localScale += new Vector3(-0.25f, 0, 0);
			}
		} 
		else {
			if(this.transform.localScale.x < 1.0f){
				this.transform.localScale += new Vector3(0.25f, 0, 0);
			}
		}
		
		
		//Moves bug at the same speed as player
		transform.Translate (translateBy * Time.deltaTime);
		
		//		Debug.Log ("velocity of X: " + currentBugRigidBody.velocity.x);
		//		Debug.Log ("velocity of Y: " + currentBugRigidBody.velocity.y);
	}
	
	
	
	//Not working. Bugs still fly past the boundaries
	private void keepInBounds () {
		float xMin = leftBound.transform.position.x;
		float xMax = rightBound.transform.position.x;
		float yMax = topBound.transform.position.y;
		float yMin = bottomBound.transform.position.y;
		
		
		//This means bug is too far in left corner. Don't want plant to squish it
		if (transform.position.x < xMin + 3.5f) {
			if (transform.position.y < yMin + 6.5f) {
				//				Debug.Log ("DONT GET SQUISHED! Moving up and left");
				objVelocityX = -1.0f;
				objVelocityY = 3.0f;
			}
		}
		//Turn around before you hit the walls:
		//This means bug is too far left. Go RIGHT
		if (transform.position.x < xMin + 1.0f) {
			objVelocityX = Random.Range (1.0f, 3.0f);
			//			Debug.Log("too far left, moving right. objVelocityX: " + objVelocityX);
		}
		//This means bug is too far right. Go LEFT
		if (transform.position.x > xMax - 1.0f) {
			objVelocityX = Random.Range (-3.0f, -1.0f);
			//			Debug.Log("too far right, moving left. objVelocityX: " + objVelocityX);
			
		}
		//This means bug is too far down. Go UP
		if (transform.position.y < yMin + 3.5f) {
			objVelocityY = Random.Range (1.0f, 2.0f);
			//			Debug.Log("too far down, moving up. objVelocityY: " + objVelocityY);
			
		}
		//This means bug is too far up. Go DOWN
		if (transform.position.y > yMax - 1.0f) {
			objVelocityY = Random.Range (-2.0f, -1.0f);
			//			Debug.Log("too far up, moving down. objVelocityY: " + objVelocityY);
		}
	}
	
	public void leaveScene(){
		bugLeavesScene = true;
	}
	/*
	//Check if bug is close enough to plant
	void isItNearPlant(){
		//Gets plant and bug's current position
		plant = GameObject.FindWithTag ("NewPlant");
		if (plant) {
			var plantCurrentPos = plant.transform.position;
			var bugCurrentPos = this.transform.position;
			
			float distanceX = Mathf.Abs (bugCurrentPos.x - plantCurrentPos.x);
			float distanceY = Mathf.Abs (bugCurrentPos.y - plantCurrentPos.y);
			
			if ((distanceX <= 5.0f) && (distanceY <= 5.0f)) {
				Debug.Log ("Bug is close enough to plant");
				objVelocityX = 0;
				objVelocityY = 0;
				
				var exclamationMark = this.transform.FindChild ("bugToPlantMarker");
				exclamationMark.gameObject.SetActive (true);
				
				float step = 2.0f * Time.deltaTime;
				transform.position = Vector2.MoveTowards (bugCurrentPos, plantCurrentPos, step);
				
				moveTicker = 2.0f;
			} else {
				var exclamationMark = this.transform.FindChild ("bugToPlantMarker");
				exclamationMark.gameObject.SetActive (false);
			}
		}
		//If there is no bug, then remove exclamation point object
		else {
			var exclamationMark = this.transform.FindChild ("bugToPlantMarker");
			exclamationMark.gameObject.SetActive (false);
		}
		plantTimer = 1.5f;
	}
*/
	
	//Detect what its hitting and what it can phase through
	void OnCollisionEnter2D(Collision2D other) {
		
		var bugRigidBody = gameObject.GetComponent<Rigidbody2D> ();
		
		//		Debug.Log ("collision detected!");
		if ((other.collider.tag == "Bug")) {
			//			Debug.Log("bug on bug collision");
			moveTicker = 0;
		} 
		else if (other.collider.tag == "NewPlant") {
			//			Debug.Log ("making contact with plant");
			plantTouched = true;
			//Get the location of bug plant and change velocity accordingly (to follow same speed as plant)
			if(plant){
				//				var plantCurrentSpeed = plant.transform.Translate;
				translateBy = plantSpeed;
				objVelocityX = 0;
				objVelocityY = 0;
			}
		}
	}
	
	void OnCollisionStay2D(Collision2D other){
		var bugRigidBody = gameObject.GetComponent<Rigidbody2D> ();
		if ((other.collider.tag == "Bug")) {
			moveTicker = 0;
		} 
		else if (other.collider.tag == "NewPlant") {
			//			Debug.Log ("making contact with plant");
			
			//Get the location of bug plant and change velocity accordingly (to follow same speed as plant)
			if(plant){
				//				var plantCurrentSpeed = plant.transform.Translate;
				translateBy = plantSpeed;
				objVelocityX = 0;
				objVelocityY = 0;
			}
		}
	}
	
	void OnCollisionExit2D(Collision2D other){
		if(other.collider.tag == "NewPlant"){
			if(plant){
				moveTicker = 0;
			}
		}
	}
	
	//Dragging
	private Vector3 flickStart;
	private Vector3 flickEnd;
	private Vector3 flickDirection;
	private float flickSpeed = 3.0f;
	private float flickTimer;
	
	// For touch sprite animations
	private Transform currTouchSprite;
	
	void OnMouseDown() {
		isSwiped = true;
		//		Debug.Log ("Mouse is down");
		plantTimer = 1.5f;
		currTouchSprite = Instantiate (touchSprite, transform.position, Quaternion.identity) as Transform;
		flickStart = Camera.main.ScreenToViewportPoint (Input.mousePosition);
		//		Debug.Log ("flickstart: " + flickStart);
	}
	
	void OnMouseDrag () {
		isSwiped = true;
		//		Debug.Log ("mouse drag!!");
		plantTimer = 1.5f;
		if (currTouchSprite) {
			currTouchSprite.position = transform.position;
		}
	}
	
	void OnMouseUp() {
		swipeTimer = 1.2f;
		isSwiped = true;
		plantTimer = 1.5f;
		if (currTouchSprite) {
			Destroy (currTouchSprite.gameObject);
		}
		
		flickEnd = Camera.main.ScreenToViewportPoint (Input.mousePosition);
		flickDirection = flickEnd - flickStart;
		flickDirection.Normalize ();
		
		//		Debug.Log ("flickDirection.Normalize(): " + flickDirection);
		
		objVelocityX = flickDirection.x * flickSpeed;
		objVelocityY = flickDirection.y * flickSpeed;
		//countdown timer with time.deltatime
		moveTicker = 1.0f;
	}
	
	/*
	void UpdateForceOverTime() {

		var bugCurrentPos = this.transform.position;
		var bugCurrPosX = bugCurrentPos.x;
		var bugCurrPosY = bugCurrentPos.y;

		if (flickTimer <= 0.0f) {
			return;
		}

		flickTimer -= Time.deltaTime;
		transform.position += flickDirection * flickSpeed * Time.deltaTime;

	}
*/
}