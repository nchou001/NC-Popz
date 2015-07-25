using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PatternLevelManager : MonoBehaviour {
	
	public int numLives = 3; // Number of mistakes player can make before pattern length is decremented
	public int numHints = 3;
	public int numRounds = 5; // Number of rounds player must complete before pattern length is incremented
	public int patternLength = 2; // Number of collectibles in the pattern
	public bool collectReverse;
	private int livesPerCollection;
	private int roundsPerCollection;
	
	public int maxPatternLength;
	private Pattern pattern;
	private Player player;
	private bool displayCollectionStatus = false;
	public GameObject displayCollectionGUI;
	
	private GameObject nextTerrainChunk;
	
	private GameObject tempClosest = null;
	private bool justOnceReset = false;
	private bool gameStarted  = false;
	private TerrainGenerator terrainGen;
	
	public GameObject LivesText;
	public GameObject RoundsText;
	
	public AudioClip fail;



	int progressionThreshold = 3;

	int upStartProgression = 1;
	int downStartProgression = 1;

	int upNextProgression =  3;
	int downNextProgression = 1;

	int correctCounter = 0;
	int incorrectCounter = 0;
	
	// Use this for initialization
	void Start () {
		Grid grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		//maxPatternLength = 4;//grid.numCellsX - 1;
		pattern = GameObject.FindGameObjectWithTag ("Pattern").GetComponent<Pattern> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		terrainGen = GameObject.FindGameObjectWithTag ("TerrainGen").GetComponent<TerrainGenerator> ();
		collectReverse = false;
		roundsPerCollection = numRounds;
		livesPerCollection = numLives;
	}
	
	IEnumerator RevealCollectionStatus(float displayTime) {
		displayCollectionStatus = true;
		yield return new WaitForSeconds(displayTime);  
		displayCollectionStatus = false;
		
	}
	
	public void CallPatternGeneration(float time)
	{
		StartCoroutine("PatternGenerationWait",time);	
		
	}
	public void StartPatternGame()
	{
		gameStarted = true;
	}
	IEnumerator PatternGenerationWait(float displayTime)
	{
		yield return new WaitForSeconds(displayTime);  
		//pattern.GeneratePattern(patternLength);
		gameStarted = true;
		//Debug.Log (patternLength);
	}
	public void seqCompleted()
	{	
		correctCounter++;
		numLives = livesPerCollection;
		int progressionThreshold = 3;
		
		int upStartProgression = 1;
		int downStartProgression = 1;
		
		int upNextProgression =  3;
		int downNextProgression = 1;

		if(correctCounter == upStartProgression)
		{
			patternLength++;
		}

		/*if (numRounds > 1) 
		{
			numRounds--;
		}
		else 
		{
			numLives = livesPerCollection;
			//numRounds = roundsPerCollection;
			if (patternLength < maxPatternLength)
			{
				patternLength++;
			}
		}*/
	}
	
	public void CloudFailed()
	{
		
		AudioSource.PlayClipAtPoint(fail, transform.position);
		if (numLives > 3) 
		{
			numLives -= 2;
		}
		else
		{
			Application.LoadLevel(0);
			
		}
		
	}
	public void CloudCompleted()
	{
		//numLives++;
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 bottomLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
		//Debug.Log ("BottomLeft: " +  bottomLeft);
		LivesText.GetComponent<Text>().text = "Lives: "+numLives.ToString();
		//RoundsText.GetComponent<Text>().text = "Rounds: "+numRounds.ToString();
		
		
		if(gameStarted)
		{
			//Debug.Log("Spawn Pattern NOW");
			int rand = Random.Range (0, 2);
			if(rand == 0)
			{
				pattern.isReverse = true;
			}
			else
			{
				pattern.isReverse = false;
			}
			pattern.GeneratePattern(patternLength);
			gameStarted = false;
		}
		
		if(pattern.startGeneratingPlatforms)
		{
			//Debug.Log("Spawn Cloud NOW");
			pattern.startGeneratingPlatforms = false;
			terrainGen.GenerateCloud();
		}
		
		
		/*if(pattern.startGeneratingPlatforms)
		{
			terrainGen.genPlants = true;
			terrainGen.genPlatforms = true;
			pattern.startGeneratingPlatforms = false;
		}*/
		
		
		
		// Skips and generates new pattern is "s" is pressed
		/*if (Input.GetKeyDown ("s")) {
			int rand = Random.Range (0, 2);
			
			if(rand == 0)
			{
				collectReverse = true;

				StartCoroutine("RevealCollectionStatus",2f);
			}
			else
			{
				collectReverse = false;
			}
			pattern.GeneratePattern(patternLength);
			numLives = livesPerCollection;
		}
		//Press h
		if (Input.GetKeyDown ("h") && numHints > 0 && !pattern.display) {
			pattern.RevealPattern();
			numHints--;
		}*/
	}
	
	public void FailedPattern () {
		if (numLives > 1) {
			numLives--;
			//pattern.RevealPattern ();
		}
		else {
			//DONE
			Application.LoadLevel(0);
			
			//numRounds = roundsPerCollection;
			//numLives = livesPerCollection;
			//if (patternLength > 2) {
			//	patternLength--;
			//}
			
			
			
			
			/*int rand = Random.Range (0, 2);
			
			if(rand == 0)
			{
				collectReverse = true;
				
				StartCoroutine("RevealCollectionStatus",2f);
			}
			else
			{
				collectReverse = false;
			}*/
			//pattern.GeneratePattern(patternLength);
		}
	}
}
