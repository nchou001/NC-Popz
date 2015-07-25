using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BugType {
	Red,
	Green,
	Blue,
	Yellow,
	White,
};

public class CloakControl : MonoBehaviour {
	
	// Bug Config
	public AudioClip success;
	public AudioClip fail;
	public BugType type;
	public Sprite revealSprite;
	public Sprite cloakSprite;
	
	// Cloak State Variables
	private float revealTicker;	
	
	// Use this for initialization
	void Start () {
		// Use cloak sprite on start
		GetComponent<SpriteRenderer> ().sprite = cloakSprite;
		
		timedReveal (4.5f);
	}
	
	// Update is called once per frame
	void Update () {
		updateCloak();
	}
	
	private void updateCloak () {
		if (revealTicker > 0) {
			revealTicker -= Time.deltaTime;
			if (GetComponent<SpriteRenderer>().sprite != revealSprite) {
				GetComponent<SpriteRenderer>().sprite = revealSprite;
			}
		}
		else {
			if (GetComponent<SpriteRenderer>().sprite != cloakSprite) {
				GetComponent<SpriteRenderer>().sprite = cloakSprite;
			}
		}
	}
	
	public void timedReveal (float time) {
		revealTicker = time;
	}
	
	public bool isRevealed () {
		return revealTicker > 0;	
	}
	
	public bool isCloaked () {
		return !isRevealed ();
	}
	
	//	public void setColorSet (List<Color> cset) {
	//		colorSet = cset;
	//	}
	
	//	public Color getRevealColor () {
	//		//return revealMaterial.color;
	//	}
}
