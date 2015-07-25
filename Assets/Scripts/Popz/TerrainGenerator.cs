using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour {
	
	public TerrainChunk terrainChunk;
	public Grid grid;
	public bool genPlants;
	public bool genPlatforms;
	private PlatformGenerator platformGen;
	private CollectibleGenerator collectibleGen;
	private GroundGenerator groundGen;
	private NbackGenerator nbackGen;
	private Player player;


	
	// Used to determine current game modes
	private PopzGameManager gameMngr;
	
	private int Tcounter = 0;
	
	// Use this for initialization
	void Start () {
		gameMngr = FindObjectOfType (typeof(PopzGameManager)) as PopzGameManager;
		
		// Set up references to object generators and grid
		grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		platformGen = GameObject.FindGameObjectWithTag ("PlatformGen").GetComponent<PlatformGenerator> ();
		collectibleGen = GameObject.FindGameObjectWithTag ("CollectibleGen").GetComponent<CollectibleGenerator> ();
		groundGen = GameObject.FindGameObjectWithTag ("GroundGen").GetComponent<GroundGenerator> ();
		nbackGen = FindObjectOfType (typeof(NbackGenerator)) as NbackGenerator;
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		
		// Position generator and box collider 
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
		Vector3 topRight = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
		bottomLeft.y = Camera.main.GetComponent<FixedHeight> ().height - (topRight.y - bottomLeft.y)/2f;
		BoxCollider2D boxCol = gameObject.GetComponent<BoxCollider2D> ();
		float boxHeight = topRight.y - bottomLeft.y;
		float boxWidth = 1f;
		boxCol.size = new Vector2 (boxWidth, boxHeight);
		boxCol.offset = new Vector2 (-boxWidth, boxHeight / 2f);
		transform.position = new Vector3(bottomLeft.x, bottomLeft.y, 0f);
		
		BoxCollider2D chunkBoxCol = terrainChunk.GetComponent<BoxCollider2D> ();
		float chunkBoxWidth = 2f * ((float) grid.numCellsX) * grid.cellSizeX;
		float chunkBoxHeight = topRight.y - bottomLeft.y;
		chunkBoxCol.size = new Vector2 (chunkBoxWidth, chunkBoxHeight);
		chunkBoxCol.offset = new Vector2 (chunkBoxWidth/2f, chunkBoxHeight/2f);
		chunkBoxCol.isTrigger = true;
		
		genPlants = false;
		genPlatforms = false;


		GenerateTerrain (transform.position);
	}
	
	public void GenerateTerrainRelevantTo (Vector3 pos) {
		Vector3 spawnPos = pos + new Vector3 (((float)grid.numCellsX) * grid.cellSizeX,0f,0f);
		GenerateTerrain (spawnPos);
	}
	
	//Should be its own Class!
	public void GenerateCloud()
	{
		//Creates Cloud GameObject
		GameObject cloud =  new GameObject();
		cloud.transform.position = new Vector3(0,0,0);
		cloud.name = "Cloud";
		
		//Create Platforms
		platformGen.GeneratePlatforms(grid,cloud);
		
		//Add Plants
		collectibleGen.GenerateCollectibles (grid, cloud);
		
		
		//Shift Cloud
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
		Vector3 topRight = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
		bottomLeft.y = Camera.main.GetComponent<FixedHeight> ().height - (topRight.y - bottomLeft.y)/2f;
		cloud.transform.position = new Vector3(topRight.x,bottomLeft.y, cloud.transform.position.z);
		cloud.AddComponent<Cloud>();
		
		Transform[] children = cloud.GetComponentsInChildren<Transform>();
		float tempx = 0f;
		for(int i = 0; i < children.Length; i++)
		{
			if(children[i].position.x > tempx)
			{
				cloud.GetComponent<Cloud>().farthestPlatform = children[i].gameObject;
				tempx = children[i].position.x;
				
			}
		}
		//Debug.Log (children.);
		//cloud.GetComponent<Cloud>().farthestPlatform = 
		
	}
	
	public void GenerateTerrain (Vector3 spawnPos) {
		grid.ClearGrid ();
		TerrainChunk tc = GameObject.Instantiate (terrainChunk, spawnPos, Quaternion.identity) as TerrainChunk;
		
		// Generate ground
		// Experimental: Generate ground separately for Nback
		// groundGen.GenerateGrounds(grid, tc);
		
		// Generate For Pattern
		if (gameMngr.Modes ().Contains (GameModes.Pattern)) {
			
			//Platforms should also be spawned in cloud format!
			/*if(genPlatforms)
			{
				platformGen.GeneratePlatforms (grid, tc);

			}
			if(genPlants)
			{
				collectibleGen.GenerateCollectibles (grid, tc);
			}*/
		}
		
		// Generate For Nback
		if (gameMngr.Modes ().Contains (GameModes.Nback)) {
			nbackGen.GenerateNbackInGrid (grid, tc, groundGen);
		} else { 
			groundGen.GenerateGrounds (grid, tc);
		}
	}
}
