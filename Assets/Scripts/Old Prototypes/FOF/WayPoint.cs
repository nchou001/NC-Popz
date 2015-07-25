using UnityEngine;
using System.Collections;

public class WayPoint : MonoBehaviour {

	public bool beingTargeted;

	// Use this for initialization
	void Start () {
		beingTargeted = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void aimAtTarget () {
		beingTargeted = true;
	}
}
