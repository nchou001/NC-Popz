using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlantSpawner : MonoBehaviour {

	//Publics
	public GameObject field;
	public GameObject player;
	public GameObject plant;
	public int successes = 0;
	public int failures = 0;
	public bool plantRestart = true;
	public GameObject PopzGameMan;
	public bool startSpawning = false;
//	public Text successText;
//	public Text failureText;


	//WayPoints
	public WayPoint wayPointReference;
	private WayPoint topBound;
	private WayPoint bottomBound;
	private WayPoint leftBound;
	private WayPoint rightBound;

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
	}
	
	// Update is called once per frame
	void Update () {

		if(startSpawning == true)
		{
		var bottomLeftCorner = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0));
		var upperRightCorner = Camera.main.ViewportToWorldPoint (new Vector3 (1f, 1f, 0));
		
		//		topBound.transform.position = upperRightCorner;
//		bottomBound.transform.position = bottomLeftCorner;
//		leftBound.transform.position = bottomLeftCorner;
		rightBound.transform.position = upperRightCorner;
		if (plantRestart == true) {
			spawnPlants ();
			plantRestart = false;
		}

		MultiObjGameManager multiObjGameManager = GameObject.Find ("MultiObjGameManager").GetComponent<MultiObjGameManager>();
		
		//var successText = GameObject.Find ("SuccessText").GetComponent<Text> ();
		//successText.text = ("Successes: " + multiObjGameManager.successes);

		//var failureText = GameObject.Find ("FailureText").GetComponent<Text> ();
		//failureText.text = ("Strikes: " + multiObjGameManager.failures);
		}

	}

	void spawnPlants () {
		Debug.Log ("spawnPlants function is running!");

//		float xMin = leftBound.transform.position.x;
		float xMax = rightBound.transform.position.x;
//		float yMax = topBound.transform.position.y;
//		float yMin = bottomBound.transform.position.y;
//		Debug.Log ("This is xMax: " + xMax);
//		Debug.Log ("This is where the plant would spawn: " + (xMax + 5));
		var spawnPosition = new Vector2 ((xMax + 2), -2);

		Transform newPlant = Instantiate (plant, spawnPosition, Quaternion.identity) as Transform;
		Debug.Log ("NewPlant created!");

	}

	public void plantDestroyed(string code){
		MultiObjGameManager multiObjGameManager = GameObject.Find ("MultiObjGameManager").GetComponent<MultiObjGameManager>();
		if (code == "success") {
//			Debug.Log ("Successful polination!");
//			this.successes += 1;
//			Debug.Log ("Successes: " + this.successes);
			multiObjGameManager.successes += 1;
		} 
		else if(code == "failure") {
//			Debug.Log ("Unsuccessful polination =[");
//			this.failures += 1;
//			Debug.Log ("Strikes: " + this.failures);
			multiObjGameManager.failures += 1;
		}
	}
}
