﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NbackGameManager : MonoBehaviour {

	// Game configuration variables
	
	// Display Rate.

	void Start(){
		if (Settings.toggleNback == false) {
			gameObject.SetActive (false);
		} else {
			gameObject.SetActive (true);
		}
	}
}
