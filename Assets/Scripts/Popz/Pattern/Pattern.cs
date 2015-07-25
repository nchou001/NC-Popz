
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pattern : MonoBehaviour {
	
	public bool display = false; // Displays pattern when true
	public List<Collectible> patternList;
	
	private Queue<Collectible> foundPattern; // Collectibles in the pattern that the player has clicked on already
	private Collectible currentHighlighted; // Collectible that is currently outlined
	private Transform highlight;
	private int resistance = 2;
	private bool hid = true;
	private Player player;
	private PatternLevelManager patternManager;
	public GameObject CubeBG;
	private BackgroundManager BGManager;
	public bool startGeneratingPlatforms = false;
	
	private CollectibleGenerator collectibleGen;
	public Vector3 offset;
	private Grid grid;
	private Vector3 center;
	
	private int previous = -1;
	
	public bool isReverse = false;
	public GameObject revImage;
	
	public Collectible current { 
		get
		{
			if(!isReverse)
			{
				
				return patternList[0];
			}
			else
			{
				return patternList[patternList.Count-1];
				
			}
		}
	}
	
	public int patternCount {
		get 
		{ 
			return patternList.Count;
			
		}
	}
	
	public int length {
		get { return patternList.Count + foundPattern.Count; }
		
	}
	
	
	void Awake () {
		
		collectibleGen = GameObject.FindGameObjectWithTag ("CollectibleGen").GetComponent<CollectibleGenerator> ();
		patternList = new List<Collectible>();
		foundPattern = new Queue<Collectible> ();
		

	}
	
	void Start () {

		patternManager = GameObject.FindGameObjectWithTag ("PatternLevelManager").GetComponent<PatternLevelManager> ();
		
		grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		offset = new Vector3(grid.cellSizeX, 0f, 0f);
		
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
		Vector3 topRight = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
		bottomLeft.y = Camera.main.GetComponent<FixedHeight> ().height - (topRight.y - bottomLeft.y)/2f;
		center = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth/2, 3*Camera.main.pixelHeight/4, 0f));
		transform.position = center;

	}
	
	void Update () {
		
		// Displays pattern when a hint is used or when a new pattern has just been generated
		//Position the Pattern Bottom Left!:
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
		Vector3 topRight = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
		bottomLeft.y = Camera.main.GetComponent<FixedHeight> ().height - (topRight.y - bottomLeft.y)/2f;
		
		if (display) 
		{
			DisplayPattern();
		}
		else if (!hid) 
		{
			HidePattern();
		}
	}
	
	// Displays the pattern to the player
	private void DisplayPattern () {
		hid = false;
		
		foreach (var c in patternList) 
		{
			c.gameObject.GetComponent<Renderer>().enabled = true;
		}
		if(isReverse)
		{
			revImage.SetActive(true);
		}
		//CubeBG.SetActive(true);
		
		
	}
	
	// Hides the pattern from the player
	private void HidePattern () {
		hid = true;
		
		//patternList
		foreach (var c in patternList) 
		{
			c.gameObject.GetComponent<Renderer>().enabled = false;
		}
		if(isReverse)
		{
			revImage.SetActive(false);
		}
		//CubeBG.SetActive(false);
		
	}
	
	// Reveals the pattern to the player for the specified amount of time
	IEnumerator RevealPattern(float displayTime) 
	{
		display = true;
		yield return new WaitForSeconds(displayTime);  
		display = false;
		startGeneratingPlatforms = true;
	}
	
	// Destroys the current pattern
	private void DestroyPattern () {
		if (display) 
		{
			StopCoroutine ("RevealPattern");
			display = false;
		}
		
		//Destroy the foundPattern Queue
		foreach (var c in foundPattern) 
		{
			Destroy(c.gameObject);
		}
		
		//List Pattern
		foreach(var c in patternList)
		{
			Destroy(c.gameObject);
		}
		patternList.Clear();
		
		collectibleGen.spawnableCollectibles.Clear();
		collectibleGen.possibleTypes.Clear();
		
		//pattern.Clear ();
		foundPattern.Clear();
	}
	
	public void RevealPattern () 
	{
		StartCoroutine("RevealPattern",((float)length) * 0.65f);
	}
	
	// Called when the player clicks the correct collectible
	public void foundCollectible () 
	{
		
		Collectible c;
		if(!isReverse)
		{
			c = patternList[0];
			patternList.RemoveAt(0);
		}
		else
		{
			c = patternList[patternList.Count-1];
			patternList.RemoveAt(patternList.Count-1);
		}
		
		
		if(patternList.Count == 0)
		{
			patternManager.seqCompleted();
		}
		
		foundPattern.Enqueue(c);
	}
	
	// Creates a nonselectable collectible of the specified type and at the specified position
	private Collectible CreatePatternCollectible (int type, Vector3 pos) 
	{
		Transform t = collectibleGen.GenerateCollectible (pos.x, pos.y, type, 0);
		t.parent = this.gameObject.transform;
		t.localScale = new Vector3(1.25f,1.25f,1f);
		Collectible col = t.gameObject.GetComponent<Collectible>();
		col.selectable = false;
		col.gameObject.layer = 9;
		col.type = type;
		col.gameObject.GetComponent<Renderer>().enabled = false;
		return col;
	}
	
	// Creates a pattern of the specified lengthz
	public void GeneratePattern (int length) 
	{
		DestroyPattern ();
		Vector3 startPos = transform.position;
		for (int i = 0; i < length; i++) 
		{
			int randNum = Random.Range (0, collectibleGen.collectibles.Length);
			
			for (int j = 0; j < resistance; j++) 
			{
				if (previous == randNum) 
				{
					randNum = Random.Range (0, collectibleGen.collectibles.Length);
				}
				else 
				{
					break;
				}
			}
			previous = randNum;
			
			collectibleGen.spawnableCollectibles.Add(collectibleGen.collectibles[randNum]);
			collectibleGen.possibleTypes.Add (randNum);
			
			
			Collectible tempCollectible = CreatePatternCollectible(randNum, startPos);
			//pattern.Enqueue(tempCollectible);
			patternList.Add(tempCollectible);
			startPos = startPos + offset;
		}
		//Shift position To center the pattern sequence:
		center = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth/2, 3*Camera.main.pixelHeight/4, 0f));
		transform.position = center;
		float newx = transform.localPosition.x - (.5f*length);
		transform.localPosition = new Vector3(newx,transform.localPosition.y,transform.localPosition.z);
		
		//transform.position.x -= 1.25*length;
		RevealPattern ();
		
		
	}
	
	
}
