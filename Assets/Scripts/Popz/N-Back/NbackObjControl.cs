﻿using UnityEngine;
using System.Collections;

//Brandon
// Mineral types
public enum Mineral {
	Yellow,
	Green,
	Red,
	Blue,
	White,
};
public class NbackObjControl : MonoBehaviour {

	public AudioClip success;
	public AudioClip fail;
	public Mineral type;
	//public Sprite cloakSprite;
	private Sprite revealSprite;
	public GameObject revealChild;
	
	private bool isCorrect = false;

	// Use this for initialization
	void Start () {
		revealSprite = GetComponent<SpriteRenderer> ().sprite;
		GetComponent<SpriteRenderer> ().sprite = revealSprite;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateReveal ();
	}
	
	void OnCollisionEnter2D(Collision2D col) {
		if (isCorrect) {
			AudioSource.PlayClipAtPoint(success, this.transform.position);
			NbackGenerator.correctInaRow++;
			NbackGenerator.wrongInaRow = 0;
		} else {
			AudioSource.PlayClipAtPoint(fail, this.transform.position);
			NbackGenerator.correctInaRow = 0;
			NbackGenerator.wrongInaRow++;
		}
		Destroy (this.gameObject);
	}
	
	void UpdateReveal() {
		// Reveal when past halfway of the screen
		float halfway = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth / 2, 0)).x;
		
		if (transform.position.x < halfway) {
			if (NbackGenerator.nLevel == NbackGenerator.prevnLevel) {
				revealChild.GetComponent<Animator> ().SetInteger ("RevealState", 1);
			}else{
				this.enabled = false;
			}
		}
	}
	
	void Cloak() {
		//GetComponent<SpriteRenderer> ().sprite = cloakSprite;
	}
	
	public void MarkCorrect() {
		isCorrect = true;
	}
}

