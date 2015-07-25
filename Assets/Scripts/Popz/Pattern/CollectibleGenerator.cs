/*
 * 
 * OLD
 * 
 * 
 * using UnityEngine;
using System.Collections;

public class CollectibleGenerator : MonoBehaviour {

	public Transform[] collectibles; // Possible collectibles that will be generated
	public float spawnChance = 0.9f; // The chance a collectible will be generated in a grid cell
	private Pattern pattern;

	void Start () {
		pattern = GameObject.FindGameObjectWithTag ("Pattern").GetComponent<Pattern> ();

		Grid grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		for (int i = 0; i < collectibles.Length; i++) {
			BoxCollider boxCol = collectibles[i].GetComponent<BoxCollider> ();
			Vector3 scale = collectibles[i].localScale;
			boxCol.size = new Vector3(grid.cellSizeX/scale.x, grid.cellSizeY/scale.y, 1f);
			boxCol.center = new Vector3(0f, 0f, 0f);
		}
	}

	// Generates collectibles in cells of the given grid
	public void GenerateCollectibles (Grid grid, TerrainChunk tc) {
		for (int x = 0; x < grid.numCellsX; x++) {
			for (int y = 3; y < grid.numCellsY; y+=2) {
				if (Random.value > spawnChance || !grid.containsObject(x,y-1)) { //Spawn Chance or there is a platform underneath
					continue;
				}
				//if(!grid.containsObject(x,y-1)) // Make sure that there is platform underneath.
				//{
				//	continue;
				}
				int collectibleType = Random.Range (0, collectibles.Length);
				GenerateCollectible (x, y, collectibleType, grid, tc);
			}
		}
	}

	// Generates collectible of the specified type at the grid location (x,y)
	public Transform GenerateCollectible (int x, int y, int type, Grid grid, TerrainChunk tc) {
		if (grid.containsObject(x, y)) {
			return null;
		}
		Vector3 spawnPos = grid.GridToWorld (x,y) + tc.transform.position;
		Transform t = GenerateCollectible (spawnPos.x, spawnPos.y, type);
		t.parent = tc.gameObject.transform;
		grid.MarkGrid (x, y);
		return t;
	}

	// Generates collectible of the specified type at world coordinates (x,y)
	public Transform GenerateCollectible (float x, float y, int type) {
		Vector3 spawnPos = new Vector3 (x,y,0); 

		Debug.Log (type);
		Transform t = GameObject.Instantiate (collectibles [type], spawnPos, Quaternion.identity) as Transform;
		//Transform t = GameObject.Instantiate (pattern.patternList[type], spawnPos, Quaternion.identity) as Transform;

		Collectible col = t.gameObject.GetComponent<Collectible>();
		return t;
	}

	//No TerrainChunk
	public void GenerateCollectibles (Grid grid, GameObject cloud) 
	{

		for (int x = 0; x < grid.numCellsX; x++) 
		{
			for (int y = 3; y < grid.numCellsY; y+=2) 
			{
				if (Random.value > spawnChance || !grid.containsObject(x,y-1)) 
				{ //Spawn Chance or there is a platform underneath
					continue;
				}
				//if(!grid.containsObject(x,y-1)) // Make sure that there is platform underneath.
				//{
				//	continue;
				//}
				int collectibleType = Random.Range (0, collectibles.Length);
				//int collectibleType = Random.Range (0, pattern.patternList.Count);

				GenerateCollectible (x, y, collectibleType, grid, cloud);
			}
		}
	}

	public void GenerateCollectible (int x, int y, int type, Grid grid, GameObject cloud) 
	{
		if (grid.containsObject(x, y)) 
		{
			//return null;
		}
		Vector3 spawnPos = grid.GridToWorld (x,y) + cloud.transform.position;
		Transform t = GenerateCollectible (spawnPos.x, spawnPos.y, type);
		t.parent = cloud.gameObject.transform;
		grid.MarkGrid (x, y);
		//return t;
	}

}*****/

	//NEW

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CollectibleGenerator : MonoBehaviour {
	
	public Transform[] collectibles; // Possible collectibles that will be generated
	public List<Transform> spawnableCollectibles; //Collectables that will be spawned
	public List<int> possibleTypes; //Collectables that will be spawned
	
	
	public float spawnChance = 0.9f; // The chance a collectible will be generated in a grid cell
	private Pattern pattern;
	
	void Start () {
		possibleTypes = new List<int>();
		pattern = GameObject.FindGameObjectWithTag ("Pattern").GetComponent<Pattern> ();
		
		Grid grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		for (int i = 0; i < collectibles.Length; i++) {
			BoxCollider boxCol = collectibles[i].GetComponent<BoxCollider> ();
			Vector3 scale = collectibles[i].localScale;
			boxCol.size = new Vector3(grid.cellSizeX/scale.x, grid.cellSizeY/scale.y, 1f);
			boxCol.center = new Vector3(0f, 0f, 0f);
		}
	}
	
	// Generates collectibles in cells of the given grid
	public void GenerateCollectibles (Grid grid, TerrainChunk tc) {
		for (int x = 0; x < grid.numCellsX; x++) {
			for (int y = 3; y < grid.numCellsY; y+=2) {
				if (Random.value > spawnChance || !grid.containsObject(x,y-1)) { //Spawn Chance or there is a platform underneath
					continue;
				}
				/*if(!grid.containsObject(x,y-1)) // Make sure that there is platform underneath.
				{
					continue;
				}*/
				int collectibleType = Random.Range (0, collectibles.Length);
				/*This is where we actually generate the plants!*/
				GenerateCollectible (x, y, collectibleType, grid, tc);
			}
		}
	}
	
	// Generates collectible of the specified type at the grid location (x,y)
	public Transform GenerateCollectible (int x, int y, int type, Grid grid, TerrainChunk tc) {
		if (grid.containsObject(x, y)) {
			return null;
		}
		Vector3 spawnPos = grid.GridToWorld (x,y) + tc.transform.position;
		Transform t = GenerateCollectible (spawnPos.x, spawnPos.y, type,1);
		t.parent = tc.gameObject.transform;
		grid.MarkGrid (x, y);
		return t;
	}
	
	// Generates collectible of the specified type at world coordinates (x,y)
	public Transform GenerateCollectible (float x, float y, int type, int called) {
		Vector3 spawnPos = new Vector3 (x,y,0); 
		
		//Debug.Log (type);
		Transform t = null;
		
		if(called == 0) // PAttern Called it
		{
			t = GameObject.Instantiate (collectibles [type], spawnPos, Quaternion.identity) as Transform;
			
		}
		else if (called == 1)
		{
			t = GameObject.Instantiate (collectibles [type], spawnPos, Quaternion.identity) as Transform;
		}
		//Transform t = GameObject.Instantiate (collectibles [type], spawnPos, Quaternion.identity) as Transform;
		
		Collectible col = t.gameObject.GetComponent<Collectible>();
		return t;
	}
	
	//No TerrainChunk
	public void GenerateCollectibles (Grid grid, GameObject cloud) 
	{
		int amount = possibleTypes.Count;
		
		//List of x locations
		List<Vector2> grid_locations = new List<Vector2>(); 
		for (int x = 0; x < grid.numCellsX; x++) 
		{
			for (int y = 3; y < grid.numCellsY; y+=2) 
			{
				if (!grid.containsObject(x,y-1)) 
				{ //Spawn Chance or there is a platform underneath
					continue;
				}
				grid_locations.Add( new Vector2(x,y));
				
			}
			
		}
		
		int rand_temp = 0;// = new Vector2(0,0);
		int randType = 0;
		int collectibleType;
		
		for(int i = amount; i != 0;i--)
		{
			rand_temp = Random.Range (0, grid_locations.Count);
			
			//Make List! Not Just Random!!
			randType = Random.Range (0, possibleTypes.Count);
			//Debug.Log ("RandType: "+randType);
			
			collectibleType = possibleTypes[randType];
			
			
			
			GenerateCollectible ((int)grid_locations[rand_temp].x, (int)grid_locations[rand_temp].y, collectibleType, grid, cloud);
			grid_locations.RemoveAt(rand_temp);
			possibleTypes.RemoveAt(randType);
			
			
			
			
			
		}
		
		
		/*for (int x = 0; x < grid.numCellsX; x++) 
		{
			for (int y = 3; y < grid.numCellsY; y+=2) 
			{
				//if (Random.value > spawnChance || !grid.containsObject(x,y-1)) 
				if (!grid.containsObject(x,y-1)) 
				{ //Spawn Chance or there is a platform underneath
					continue;
				}
				//if(!grid.containsObject(x,y-1)) // Make sure that there is platform underneath.
				//{
				//	continue;
				//}
				//int collectibleType = Random.Range (0, collectibles.Length);

				if(amount != 0)
				{
					int collectibleType = Random.Range (0, spawnableCollectibles.Count);
					//This is where we actually generate the plants!
					GenerateCollectible (x, y, collectibleType, grid, cloud);
					amount--;
				}
			}
		}
		*/
	}
	
	public void GenerateCollectible (int x, int y, int type, Grid grid, GameObject cloud) 
	{
		if (grid.containsObject(x, y)) 
		{
			
		}
		Vector3 spawnPos = grid.GridToWorld (x,y) + cloud.transform.position;
		Transform t = GenerateCollectible (spawnPos.x, spawnPos.y, type,1);
		t.parent = cloud.gameObject.transform;
		grid.MarkGrid (x, y);
	}
	
	
	
	
}
