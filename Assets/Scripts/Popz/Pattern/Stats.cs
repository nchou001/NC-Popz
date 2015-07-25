using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {

	private PatternLevelManager levelManager;

	// Use this for initialization
	void Start () {
		levelManager = GameObject.FindGameObjectWithTag ("PatternLevelManager").GetComponent<PatternLevelManager> ();
	}

	void OnGUI () {
		//GUI.Label (new Rect(Screen.width - 100, Screen.height - 70, 100, 20), "Rounds: " + levelManager.numRounds.ToString());
		//GUI.Label (new Rect(Screen.width - 100, Screen.height - 50, 100, 20), "Lives: " + levelManager.numLives.ToString());
		//GUI.Label (new Rect (Screen.width - 100, Screen.height - 30, 100, 30), "Hints: " + levelManager.numHints.ToString());
	}
}
