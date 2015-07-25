using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiObjGameManager : MonoBehaviour {
	
	public int level;
	public int stage;
	public float pushSpeed = 6.0f;
	
	public GameObject field;
	public GameObject player;
	public List<Transform> trackingObjects;
	public GameObject invisibleBoundary;
	public bool restartBugs = false;
	public List<Transform> colors;
	
	public int successes = 0;
	public int failures = 0;
	public GameObject plantSpawner;
	
	// Game State Variables
	private bool gameRunning = false;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (gameRunning) {
			checkGameEnd ();
		}
		
		/*
		if (restartBugs == true) {
			restartBugs = false;
//			cleanupLevel();
//			restartLevel ();
		}
		*/
		
		FieldInfo info = Util.getFieldInfo(field);
		
		//Game progression added here
		//If 3 successful polinations, then add another bug
		if (successes >= 3) {
			//add another bug
			var spawnPosition = new Vector3 (Random.Range (info.lowerX + 1, info.upperX + 7), Random.Range (info.upperY + 1, info.upperY + 3), -2);
			int rand = Random.Range (0, trackingObjects.Count);
			Transform creature = Instantiate (trackingObjects [rand], spawnPosition, Quaternion.identity) as Transform;
			colors.Add(trackingObjects[rand]);
			successes = 0;
			
		} else if (failures >= 1) {
			//find a random bug to remove
			var bugToRemove = GameObject.FindGameObjectWithTag ("Bug");
			var colorToRemove = bugToRemove.GetComponent<CloakControl>().type;
			
			//remove that bug from the list as well
			for(int i = 0; i < colors.Count -1; i++){
				if( colorToRemove == colors[i].GetComponent<CloakControl>().type){
					colors.RemoveAt(i);
					//call its removal function
					bugToRemove.GetComponent<Movement>().leaveScene();
					//					Destroy (bugToRemove);
					failures = 0;
					return;
				}
			}
		}
	}
	
	void startCreatures () {
		FieldInfo info = Util.getFieldInfo(field);
		for (int i = 0; i < level - 3 ; ++i) {
			// Universal parameters for all creatures
			var spawnPosition = new Vector3(Random.Range (info.lowerX + 1, info.upperX + 7),
			                                Random.Range (info.upperY + 1, info.upperY + 3),
			                                -2);
			
			int rand = Random.Range (0, trackingObjects.Count);
			Transform creature = Instantiate (trackingObjects[rand], spawnPosition, Quaternion.identity) as Transform;
			colors.Add(trackingObjects[rand]);
			//			creature.transform.parent = player.transform;
			
			creature.gameObject.GetComponent<Movement>().field = field;
			creature.gameObject.GetComponent<Movement>().pushSpeed = pushSpeed;
		}
	}
	
	public void startLevel () {
		plantSpawner.GetComponent<PlantSpawner>().startSpawning = true;
		if (stage > level) {
			++level;
			stage = 1;
		}
		
		if (stage <= 0) {
			stage = 1;
		}
		
		startCreatures ();
		gameRunning = true;
	}
	
	void cleanupLevel () {
		gameRunning = false;
		var creatures = FindObjectsOfType<CloakControl> ();
		for (int i = 0; i < creatures.Length; ++i)
			Destroy (creatures [i].gameObject);
		player.GetComponentInChildren<MultiObjPlayer> ().numCloakedObtained = 0;
	}
	
	void restartLevel() {
		++stage;
		startLevel ();
	}
	
	void checkGameEnd () {
		var numCloakedObtained = player.GetComponentInChildren<MultiObjPlayer> ().NumCloakedObtained ();
		if (level == numCloakedObtained) {
			cleanupLevel();
			restartLevel();
		}
	}
}
