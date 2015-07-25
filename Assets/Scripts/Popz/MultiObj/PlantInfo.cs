using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlantInfo : MonoBehaviour {
	
	//Plant info is a component of the plant
	//plant health slowly decreases until down to zero. which would be game over.
	public int health = 100;
	public int depleteBy = 2;
	public float depleteSeconds = 1.0f;
	public Slider healthSlider;
	public int strikes = 0;
	public int successes = 0;
	private bool notHitYet = true;
	
	// Waypoint System Parameters 
	// TODO: CHANGE TO SIMPLE RECTANGLE OBJECT
	public GameObject field;
	public WayPoint wayPointReference;
	private WayPoint topBound;
	private WayPoint bottomBound;
	private WayPoint leftBound;
	private WayPoint rightBound;
	private WayPoint endPoint;
	
	public BugType correctBugType;
	public Color bugColor;
	
	//To make the plant move
	public float speed = 20.0f;
	private CircleCollider2D playerCircleCollider;
	private float plantRemovalX = 0f;
	
	
	// Use this for initialization
	void Start () {
		//Change the plant to have a color same as one of the bugs on screen
		var MultiObjGameManager = GameObject.Find("MultiObjGameManager");
		var bugList = MultiObjGameManager.GetComponent<MultiObjGameManager> ().colors;
		Debug.Log ("buglist: " + bugList.Count);
		if (MultiObjGameManager) {
			int randomBug = Random.Range (0, bugList.Count);
		//	Debug.Log ("type of bug color: " + bugList [randomBug].GetComponent<CloakControl> ().type);
			correctBugType = bugList [randomBug].GetComponent<CloakControl> ().type;
			
			if(correctBugType == BugType.Blue){
				bugColor = Color.blue;
			}
			else if(correctBugType == BugType.Green){
				bugColor = Color.green;
			}
			else if(correctBugType == BugType.Red){
				bugColor = Color.red;
			}
			else if(correctBugType == BugType.White){
				bugColor = Color.white;
			}
			else if(correctBugType == BugType.Yellow){
				bugColor = Color.yellow;
			}
		}
		
		//start plant health
		int health = 100;
		
		//Lets the plant only be hit once
		notHitYet = true;
		
		//Set animator script parameter value
		gameObject.GetComponent<Animator> ().SetFloat("animHealth", health);
		
		PlantSpawner plantSpawner = GameObject.Find ("PlantSpawner").GetComponent<PlantSpawner>();
		
		FieldInfo info = Util.getFieldInfo (field);
		
		var bottomLeftCorner = new Vector3 (info.lowerX, info.lowerY, 0);
		var upperRightCorner = new Vector3 (info.upperX, info.upperY, 0);
		
		var blank = wayPointReference;
		topBound = Instantiate (blank, upperRightCorner, transform.rotation) as WayPoint;
		bottomBound = Instantiate (blank, bottomLeftCorner, transform.rotation) as WayPoint;
		leftBound = Instantiate (blank, bottomLeftCorner, transform.rotation) as WayPoint;
		rightBound = Instantiate (blank, upperRightCorner, transform.rotation) as WayPoint;
		
		//TODO: change this to one of the actual colors
		//		this.GetComponent<Renderer>().material.color = Util.randomColor ();
		this.GetComponent<Renderer>().material.color = bugColor;
		
		//		Debug.Log (health);
		InvokeRepeating ("Depleting", depleteSeconds, depleteSeconds);
		
		var player = GameObject.Find ("Player");
		playerCircleCollider = player.GetComponent<CircleCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		var MultiObjGameManager = GameObject.Find("MultiObjGameManager");

		var bugList = MultiObjGameManager.GetComponent<MultiObjGameManager> ().colors;
		Debug.Log ("buglist: " + bugList.Count);
		//Set animator script parameter value
		//		gameObject.GetComponent<Animator> ().SetFloat("animHealth", health);
		//		gameObject.GetComponent<Animator> ().SetBool("healed", false);
		//		gameObject.GetComponent<Animator> ().SetBool("hurt", false);
		
		//		Debug.Log("animhealth" + gameObject.GetComponent<Animator> ().GetFloat("animHealth"));
		//		Debug.Log ("healed bool: " + gameObject.GetComponent<Animator> ().GetBool("healed"));
		//		Debug.Log ("hurt bool: " + gameObject.GetComponent<Animator> ().GetBool("hurt"));
		
		//Plant is moving along slowly with the player (shows the player running past it
		this.transform.Translate(Vector2.right * speed * Time.deltaTime);
		
		//If Plant is out of playfield, it is removed and calculated if pass or fail
		var bottomLeftCorner = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0));
		var upperRightCorner = Camera.main.ViewportToWorldPoint (new Vector3 (1f, 1f, 0));
		
		topBound.transform.position = upperRightCorner;
		bottomBound.transform.position = bottomLeftCorner;
		leftBound.transform.position = bottomLeftCorner;
		rightBound.transform.position = upperRightCorner;
		
		plantRemovalX = leftBound.transform.position.x;
		var plantCurrPosX = this.transform.position.x;
		
		if (plantCurrPosX <= (plantRemovalX - 1f)) {
			GameObject[] bugMovements;
			bugMovements = GameObject.FindGameObjectsWithTag("Bug");
			foreach (GameObject bugMovement in bugMovements){
				bugMovement.GetComponent<Movement>().plantTouched = false;
			}
			//Debug.Log("Plant destroyed!");
			plantRemovalAndCalc();
		}
		
		//		Debug.Log (health);
		//Calls Planthealthanimation to show current status of plant
		plantHealthAnimation ();
		
	}
	
	void Depleting(){
		if((health - depleteBy) < 100)
			health -= depleteBy;
		if (health <= 0) {
			health = 0;
			CancelInvoke ("Depleting");
		}
	}
	
	//TODO = COMPARE COLORS
	void OnCollisionEnter2D(Collision2D other){
		//Debug.Log ("Colliding to this object on OnCollisionEnter! " + other.collider.name);
		if (other.collider.tag == "Bug") {
		//	Debug.Log ("Plant collider hit bug!");
			var otherBug = other.collider.gameObject;
		//	Debug.Log ("correctBugType: " + correctBugType);
		//	Debug.Log ("otherBug's type: " + otherBug.GetComponent<CloakControl> ().type);
			
			if (notHitYet) {
				if (otherBug.GetComponent<CloakControl> ().type == correctBugType) {
					//				this.health += 25;
					gameObject.GetComponent<Animator> ().SetBool ("healed", true);
					gameObject.GetComponent<Animator> ().SetBool ("hurt", false);
					notHitYet = false;
				} else {
					//				this.health -= 25;
					gameObject.GetComponent<Animator> ().SetBool ("healed", false);
					gameObject.GetComponent<Animator> ().SetBool ("hurt", true);
					notHitYet = false;
				}
			}
		} else if (other.collider.tag == "Player") {
			GameObject[] bugMovements;
			bugMovements = GameObject.FindGameObjectsWithTag ("Bug");
			foreach (GameObject bugMovement in bugMovements) {
				bugMovement.GetComponent<Movement> ().plantTouched = false;
			}
			//Debug.Log ("Plant destroyed!");
			plantRemovalAndCalc ();
			
		}
	}
	
	void OnCollisionStay(Collision other){
		if (other.collider.tag == "Bug")
			this.depleteBy = -8;
	}
	
	
	void OnCollisionExit(Collision other){
		if (other.collider.tag == "Bug")
			this.depleteBy = 2;
	}
	
	//Calculates final health, calls plantDestroyed from PlantSpawner, then destroy object
	void plantRemovalAndCalc(){
		
		PlantSpawner plantSpawner = GameObject.Find ("PlantSpawner").GetComponent<PlantSpawner>();
		Movement bugMovement = GameObject.FindGameObjectWithTag ("Bug").GetComponent<Movement>();
		MultiObjGameManager multiObjGameManager = GameObject.Find ("MultiObjGameManager").GetComponent<MultiObjGameManager>();
		
		//Debug.Log ("bugMovement's name: " + bugMovement.tag);
		
		var healed = gameObject.GetComponent<Animator> ().GetBool("healed");
		var hurt = gameObject.GetComponent<Animator> ().GetBool("hurt");
		
		//		if (this.health >= 80) {
		//			plantSpawner.plantDestroyed ("success");
		//		} 
		//		else {
		//			plantSpawner.plantDestroyed ("failure");
		//		}
		
		//Tells PlantSpawner.cs plantDestroyer function if it was a success or a failure
		if(healed) {
			plantSpawner.plantDestroyed ("success");
		} 
		else if(hurt) {
			plantSpawner.plantDestroyed ("failure");
		}
		Destroy (gameObject);
		plantSpawner.plantRestart = true;
		bugMovement.plantIsAlive = false;
		//		multiObjGameManager.restartBugs = true;
	}
	
	//Plant Health Animation to show what state the plant is. Indicator of health
	void plantHealthAnimation(){
		if (this.health >= 60) {
			//Jolly Animation
		} else if (this.health < 60) {
			//Sad animation
		}
		//			else if ((this.health < 80) && (this.health >= 50)) {
		//			//content idle
		//		} else if ((this.health < 50) && (this.health >= 30)) {
		//			//dying animation
		//		} else if ((this.health < 30)) {
		//			//really sad animation
		//		}
	}
	
	
}
