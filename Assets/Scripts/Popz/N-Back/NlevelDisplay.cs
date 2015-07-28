using UnityEngine;
using System.Collections;

public class NlevelDisplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (Settings.toggleNback == false) {
			gameObject.SetActive (false);
		} else {
			gameObject.SetActive (true);
		}
	}
}
