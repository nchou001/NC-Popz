using UnityEngine;
using System.Collections;

public class GoodCreatureSelect : MonoBehaviour {

	private Creature cObject;
	public ContainerButtons selection;
	private SpawnCreatures spawner;
	public LevelManager manager;
	public int selectionID;
	public string colorCode;
	private int hasBeenSelected;
	public bool canBeSelected;

	public AudioSource correctSound;
	public AudioSource wrongSound;

	// Use this for initialization
	void Start () {
		hasBeenSelected = 0;
		canBeSelected = true;
		cObject = gameObject.GetComponent<Creature>();
		spawner = GameObject.FindGameObjectWithTag("spawner").GetComponent<SpawnCreatures>();
	}
	
	// Update is called once per frame
	void Update () {
		canBeSelected = cObject.getSelectable();
		checkSelectFlags();
	}

	void checkSelectFlags () {
		hasBeenSelected = selection.getSelection();
	}
/*
	void OnMouseDown () {
		if(canBeSelected && hasBeenSelected == selectionID) {
			spawner.removeCreature();
			Destroy(gameObject);
		}
		else if(canBeSelected && hasBeenSelected != selectionID) {
			manager.setPenalty();
		}
	}
*/
	
	public void setSelectID (string cc) {
		colorCode = cc;
//		string buttonCode = "";
		switch (cc) {
		case "r":
//			buttonCode = "Red";
			selectionID = 1;
			break;
		case "b":
			selectionID = 0;
//			buttonCode = "Blue";
			break;
		case "g":
			selectionID = 3;
//			buttonCode = "Green";
			break;
		case "m":
			selectionID = 4;
//			buttonCode = "Magenta";
			break;
		case "y":
			selectionID = 2;
//			buttonCode = "Yellow";
			break;
		default:
			selectionID = -1;
			break;
		}
//		selection.addSelectID(buttonCode);
	}

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<Carrier>().selectionID == selectionID) {
			spawner.removeCreature();
			Destroy(gameObject);
			correctSound.Play();
		} 
		else {
			manager.setPenalty();
			wrongSound.Play();
		}
	}
}
