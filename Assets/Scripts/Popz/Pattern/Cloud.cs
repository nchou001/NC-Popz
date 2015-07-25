/*
 * OLD
 * 
 * using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	public GameObject farthestPlatform;
	private bool farthestPlatformInView = false;
	private PatternLevelManager patternManager;
	private bool genJustOnce = false;
	private Pattern pattern;


	public bool completed = false;

	void Start()
	{
		patternManager = GameObject.FindGameObjectWithTag ("PatternLevelManager").GetComponent<PatternLevelManager> ();
		pattern = GameObject.FindGameObjectWithTag ("Pattern").GetComponent<Pattern> ();

	}
	
	// Update is called once per frame
	void Update () {

		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
		Vector3 topRight = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
	
		if(farthestPlatform != null)
		{
			//FarthestPlatform has left the left-side of the screen
			if(!farthestPlatformInView)
			{
				if(farthestPlatform.transform.position.x < topRight.x)
				{
					//Debug.Log ("Now its in view!");
					farthestPlatformInView = true;
				}
			}

			if( farthestPlatformInView && !farthestPlatform.GetComponent<SpriteRenderer>().isVisible)
			{
				//Debug.Log ("DESTROY!");
				//Call the next Pause+SequenceToShow
				if(!genJustOnce)
				{
					if(pattern.patternCount == 0)//Completed
					{
						Debug.Log ("Cloud Completed");
						patternManager.CloudCompleted();
						completed = true;
					}
					else
					{
						completed = false;
						Debug.Log ("Cloud Not Completed");
						patternManager.CloudFailed();
					}
					patternManager.CallPatternGeneration(1f);
					genJustOnce = true;

				}

				//Then Destroy
				GameObject.Destroy(gameObject);

			}

		}

	}
}
****/

// NEW 



using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {
	
	public GameObject farthestPlatform;
	private bool farthestPlatformInView = false;
	private PatternLevelManager patternManager;
	private bool genJustOnce = false;
	private Pattern pattern;
	
	
	public bool completed = false;
	
	void Start()
	{
		patternManager = GameObject.FindGameObjectWithTag ("PatternLevelManager").GetComponent<PatternLevelManager> ();
		pattern = GameObject.FindGameObjectWithTag ("Pattern").GetComponent<Pattern> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
		Vector3 topRight = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
		
		if(farthestPlatform != null)
		{
			//FarthestPlatform has left the left-side of the screen
			if(!farthestPlatformInView)
			{
				if(farthestPlatform.transform.position.x < topRight.x)
				{
					//Debug.Log ("Now its in view!");
					farthestPlatformInView = true;
				}
			}
			
			if( farthestPlatformInView && !farthestPlatform.GetComponent<SpriteRenderer>().isVisible)
			{
				//Debug.Log ("DESTROY!");
				//Call the next Pause+SequenceToShow
				if(!genJustOnce)
				{
					if(pattern.patternCount == 0)//Completed
					{
						//Debug.Log ("Cloud Completed");
						patternManager.CloudCompleted();
						completed = true;
					}
					else
					{
						completed = false;
						//Debug.Log ("Cloud Not Completed");
						patternManager.CloudFailed();
					}
					patternManager.CallPatternGeneration(1f);
					genJustOnce = true;
					
				}
				
				//Then Destroy
				GameObject.Destroy(gameObject);
				
			}
			
		}
		
	}
}

