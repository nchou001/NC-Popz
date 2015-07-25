using UnityEngine;
using System.Collections;

public class Selection : MonoBehaviour {
	
	static private GameObject closestObj = null;
	static private GameObject highlight = null;
	public MultiObjPlayer player;
	
	void Awake () {
		if (highlight == null) {
			highlight = Instantiate(Resources.Load("TrackerHighlight")) as GameObject;
		}
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		UpdateClosest ();
		DisplaySelection ();
	}
	
	void UpdateClosest() {
		if (closestObj == null) {
			closestObj = this.gameObject;
		}
		
		var closestDist = Util.getDistance2D (Selection.closestObj, player.gameObject);
		var myDist = Util.getDistance2D (this.gameObject, player.gameObject);
		if (myDist < closestDist) {
			Selection.closestObj = this.gameObject;
		}
	}
	
	void DisplaySelection() {
		if (this.gameObject.GetComponent<CloakControl> ().isRevealed ()) {
			return;
		}
		if (this.gameObject == Selection.closestObj) {
			GetComponent<Renderer>().material.color = Color.cyan;
		} else {
			GetComponent<Renderer>().material.color = Color.black;
		}
	}
	/*
	static public void DoSelection() {
		if (Selection.closestObj.GetComponent<CloakControl>().isRevealed()) {
			return;
		}

		// Verify object is in selection radius
		var selectionRadius = Selection.closestObj.GetComponent<Selection>().player.GetComponentInChildren<SphereCollider> ();
		var radius = selectionRadius.gameObject.transform.localScale.x / 2.0f;
		var dist = Util.getDistance2D (selectionRadius.gameObject, Selection.closestObj);
		if (dist > radius) {
			Debug.Log ("Can't grab that object... Out of reach");
			return;
		}

		Selection.closestObj.GetComponent<CloakControl> ().validate ();
		Selection.closestObj.gameObject.SetActive(false);
		Selection.closestObj = null;
	}*/
}
