/*
 * 
 * 
 * OLD
 * 
 * 
 * using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Species { Plant1, Plant2, Plant3, Plant4, Sphere, Cube, Capsule }; // Possible plant species

public class Collectible : MonoBehaviour {

	public Color color; 
	public Species species; 
	public bool selectable = true;
	public int type;

	public AudioClip success;
	public AudioClip fail;
	
	private Pattern pattern;
	private PatternLevelManager levelManager;

	void Start () {
		pattern = GameObject.FindGameObjectWithTag ("Pattern").GetComponent<Pattern> ();
		levelManager = GameObject.FindGameObjectWithTag ("PatternLevelManager").GetComponent<PatternLevelManager> ();
	}

	void OnMouseDown () {
		if (!selectable || pattern.display) { return; }
		if(pattern.patternCount != 0)
		{
			if (pattern.current.color == color && pattern.current.species == species) 
			{
				Destroy (gameObject);
				pattern.foundCollectible();
				AudioSource.PlayClipAtPoint(success, transform.position);
			}	 
			else 
			{
				levelManager.FailedPattern();
				AudioSource.PlayClipAtPoint(fail, transform.position);
			}
		}
	}

	// If collectible is off screen, delete it
	void OnBecameInvisible () {
		if (selectable) {
			//Destroy (gameObject);
		}
	}

}***/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Species { Plant1, Plant2, Plant3, Plant4, Plant5, Sphere, Cube, Capsule }; // Possible plant species

public class Collectible : MonoBehaviour {
	
	public Color color; 
	public Species species; 
	public bool selectable = true;
	public int type;
	
	public AudioClip success;
	public AudioClip fail;
	
	private Pattern pattern;
	private PatternLevelManager levelManager;
	bool moveUp = false;
	
	void Start () {
		pattern = GameObject.FindGameObjectWithTag ("Pattern").GetComponent<Pattern> ();
		levelManager = GameObject.FindGameObjectWithTag ("PatternLevelManager").GetComponent<PatternLevelManager> ();
	}
	void Update()
	{
		if(moveUp)
		{
			gameObject.transform.Translate(Vector2.up * Time.deltaTime*30);
			if(gameObject.transform.position.y >= 400)
			{
				moveUp = false;
			}
		}

	}
	public void collectibleGoUp()
	{
		moveUp = true;

	}
	
	void OnMouseDown () {
		if (!selectable || pattern.display) { return; }
		if(pattern.patternCount != 0)
		{
			if (pattern.current.color == color && pattern.current.species == species) 
			{
				//Destroy (gameObject);
				pattern.foundCollectible();
				AudioSource.PlayClipAtPoint(success, transform.position);
				//Play Bubble Animation
				gameObject.GetComponentInChildren<Bubble>().CorrectChoice();
			}	 
			else 
			{
				levelManager.FailedPattern();
				//AudioSource.PlayClipAtPoint(fail, transform.position);
				StartCoroutine("PopWait",.5f);	

				gameObject.GetComponentInChildren<Bubble>().InCorrectChoice();

			}
		}
	}

	IEnumerator PopWait(float displayTime)
	{
		yield return new WaitForSeconds(displayTime); 
		AudioSource.PlayClipAtPoint(fail, transform.position);
	
	}



	// If collectible is off screen, delete it
	void OnBecameInvisible () {
		if (selectable) {
			//Destroy (gameObject);
		}
	}
	
}

