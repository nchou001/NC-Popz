
using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour {
	private CollectibleGenerator collectibleGen;
	
	public Platform[] platforms; // Possible platforms that will be generated
	public float spawnChance = 0.7f; // The chance a platform will be generated in a grid cell
	public int maxCellLength = 1;
	
	void Awake() {
		collectibleGen = GameObject.FindGameObjectWithTag ("CollectibleGen").GetComponent<CollectibleGenerator> ();
		
		for (int i = 0; i < platforms.Length; i++) {
			if (platforms[i].numCells > maxCellLength) {
				maxCellLength = platforms[i].numCells;
			}
		}
	}
	
	
	
	// Generates platforms in cells of given grid
	public void GeneratePlatforms (Grid grid, TerrainChunk tc) {
		for (int x = 0; x < grid.numCellsX; x++) {
			for (int y = 2; y < grid.numCellsY; y+=2) {
				
				//Cloud Spawning By Column
				
				int platformType = 0;//Random.Range (0, platforms.Length);
				
				//GeneratePlatform (x, y, platformType, grid, tc);
				
				//Cloud Format:
				
				if(x == 0)
				{
					if(y == 4)
					{
						//If x == 0 spawn platform on y=4 
						GeneratePlatform (x, y, 0, grid, tc);
					}
					
				}
				else if(x ==1)
				{
					if(y !=6)
					{
						//if x == 1 spawn platforms on y=4 and y=2
						GeneratePlatform (x, y, 0, grid, tc);
					}
					
					
				}
				else if(x ==2)
				{
					if(y != 2)
					{
						//if x == 2 spawn platforms on y=4 and y =6
						GeneratePlatform (x, y, 0, grid, tc);
					}
					
				}
				else if(x == grid.numCellsX-4)
				{
					if( y != 2)
					{
						//if x ==grid.numCells-3 spawn on y= 4 and y=6
						GeneratePlatform (x, y, 0, grid, tc);
					}
					
					
				}
				else if(x == grid.numCellsX-3)
				{
					if(y != 6)
					{
						//if x == grid.numCells-2 spawn on y=2 and y=4
						GeneratePlatform (x, y, 0, grid, tc);
					}
					
					
				}
				else if(x == grid.numCellsX-2)
				{
					if( y == 4)
					{
						//if x == grid.numCells-1 spawn on y=4 
						GeneratePlatform (x, y, 0, grid, tc);
					}
					
					
				}
				else if (x == grid.numCellsX-1)
				{
					//Do not do anything
				}
				else
				{
					GeneratePlatform (x, y, platformType, grid, tc);
					
				}
				
			}
		}
	}
	// Generates platform of the specified type at the grid location (x,y)
	public Transform GeneratePlatform (int x, int y, int type, Grid grid, TerrainChunk tc) {
		for (int i = 0; i < platforms[type].numCells; i++) {
			if (grid.containsObject(x + i, y)) {
				return null;
			}
		}
		Vector3 offset =  new Vector3 (((float)(platforms[type].numCells - 1)) * 0.5f * grid.cellSizeX, 0f, 0f);
		Vector3 spawnPos = grid.GridToWorld (x, y) + offset + tc.transform.position;
		for (int i = 0; i < platforms[type].numCells; i++) {
			grid.MarkGrid (x + i, y);
		}
		Transform t = GeneratePlatform (spawnPos.x, spawnPos.y, type);
		t.parent = tc.gameObject.transform;
		return t;
	}
	
	// Generates platform of the specified type at world coordinates (x,y)
	public Transform GeneratePlatform (float x, float y, int type) {
		Vector3 spawnPos = new Vector3 (x, y, 0);
		Platform p = GameObject.Instantiate (platforms [type], spawnPos, Quaternion.identity) as Platform;
		return p.gameObject.transform;
	}
	
	//No Terrrain Chunk
	
	// Generates platforms in cells of given grid
	public void GeneratePlatforms (Grid grid,GameObject cloud) {
		int sizeOfPossibleCollectibles = collectibleGen.possibleTypes.Count;
		
		//for (int x = 0; x < grid.numCellsX; x++) {
		for (int x = 0; x < sizeOfPossibleCollectibles; x++) {
			for (int y = 2; y < grid.numCellsY; y+=2) {
				
				//Cloud Spawning By Column

				int platformType = 	collectibleGen.spawnableCollectibles.Count-1;
				//0;// Random.Range (0, platforms.Length);
				
				if(y== 4)
				{
					GeneratePlatform (x, y, platformType, grid,cloud);
					
				}
				
				
				/*if(x == 0)
				{
					if(y == 4)
					{
						//If x == 0 spawn platform on y=4 
						GeneratePlatform (x, y, 0, grid, cloud);
					}
					
				}
				else if(x ==1)
				{
					if(y !=6)
					{
						//if x == 1 spawn platforms on y=4 and y=2
						GeneratePlatform (x, y, 0, grid, cloud);
					}
					
					
				}
				else if(x ==2)
				{
					if(y != 2)
					{
						//if x == 2 spawn platforms on y=4 and y =6
						GeneratePlatform (x, y, 0, grid,cloud);
					}
					
				}
				else if(x == grid.numCellsX-4)
				{
					if( y != 2)
					{
						//if x ==grid.numCells-3 spawn on y= 4 and y=6
						GeneratePlatform (x, y, 0, grid, cloud);
					}
					
					
				}
				else if(x == grid.numCellsX-3)
				{
					if(y != 6)
					{
						//if x == grid.numCells-2 spawn on y=2 and y=4
						GeneratePlatform (x, y, 0, grid,cloud);
					}
					
					
				}
				else if(x == grid.numCellsX-2)
				{
					if( y == 4)
					{
						//if x == grid.numCells-1 spawn on y=4 
						GeneratePlatform (x, y, 0, grid,cloud);
					}
					
					
				}
				else if (x == grid.numCellsX-1)
				{
					//Do not do anything
				}
				else
				{
					GeneratePlatform (x, y, platformType, grid,cloud);
					
				}*/
				
			}
		}
	}
	public void GeneratePlatform (int x, int y, int type, Grid grid, GameObject cloud) {
		for (int i = 0; i < platforms[type].numCells; i++) 
		{
			if (grid.containsObject(x + i, y)) 
			{
				//return null;
			}
		}
		Vector3 midRight = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight/2, 0f));
		Vector3 offset =  new Vector3 (((float)(platforms[type].numCells - 1)) * 0.5f * grid.cellSizeX, 0f, 0f);
		Vector3 spawnPos = grid.GridToWorld (x, y) + offset;
		//spawnPos.x = midRight.x;
		//spawnPos.y -= 1f;
		for (int i = 0; i < platforms[type].numCells; i++) {
			grid.MarkGrid (x + i, y);
		}
		
		Transform temp = GeneratePlatform (spawnPos.x, spawnPos.y, type);
		temp.parent = cloud.transform;
		
	}
	//End No Chunk
	
	
}

