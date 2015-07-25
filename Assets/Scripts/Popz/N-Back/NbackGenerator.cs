using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NbackGenerator : MonoBehaviour {
	
	public List<Transform> nbackObjects;
	private int nLevel = 1;
	public int rate = 5;
	
	public bool platforms = true;
	public int navigationDifficulty = 0;
	
	private int lastGridOffset;
	private List<Mineral> sequence = new List<Mineral>();
	private bool nbackFirstGround = false;
	public static int correctInaRow = 0;
	public static int wrongInaRow = 0;
	private float timer = 0;
	private int previous = 0;
	public GameObject nLevelText;
	public Transform obstacle;

	void Awake () {
		if (Settings.isSet) {
			platforms = Settings.togglePlatformsNback;
			navigationDifficulty = Settings.nbackNavigationDifficulty;
		}
	}
	
	// Use this for initialization
	void Start () {
		lastGridOffset = rate;

	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		nLevelText.GetComponent<Text>().text = "nLevel: "+nLevel.ToString();
		// If 3 correct in a row, increase nlevel
		if (correctInaRow == 3) {
			nLevel++;
			correctInaRow = 0;
		}
		
		// If 2 wrong in a row, decrease nLevel
		else if(wrongInaRow == 2 & nLevel > 1){
			nLevel--;
			wrongInaRow = 0;
		}
	}
	
	// Entry for prototype platforms generation
	public void GenerateNbackInGridPlatforms(Grid grid, TerrainChunk tc, GroundGenerator ggen) {

		int x = lastGridOffset;
		int topY = grid.numCellsY - 2;
		int midY = topY / 2;
		for ( ; x < grid.numCellsX; x += rate) {
			int roll = Random.Range (0, 4);
			int placement = roll == 0 ? 1 : (roll == 1 ? midY + 1: topY + 1);
			//Debug.Log("x: "+x.ToString()+" roll: "+roll.ToString()+" placement: "+placement.ToString());
			Transform h = GenerateNbackObjectInGrid(x, placement, grid, tc);
		}
		lastGridOffset = x - grid.numCellsX;
		
		for (int y = 0; y <= grid.numCellsY; ++y) {
			ggen.GenerateGround(y, topY, grid, tc);
			ggen.GenerateGround(y, midY, grid, tc);
			ggen.GenerateGround(y, 0, grid, tc);
		}
		
		return;
	}
	
	int generateCount = 0;
	public void GenerateNbackInGrid(Grid grid, TerrainChunk tc, GroundGenerator ggen) {
		//Called from Terrainen
		int difficulty = navigationDifficulty;
		int scale = 10;
		
		// Generate beginning so character doesnt fall
		if (generateCount == 0) {
			for (int i = 0; i < 5; ++i) {
				//ggen.GenerateGround (i, 0, grid, tc);
				//ggen.GenerateGround (i, 7, grid, tc);
			}
		}
		++generateCount;
		
		//Entry for prototype platforms generation
		/*if (platforms) {
			GenerateNbackInGridPlatforms(grid, tc, ggen);
			return;
		}*/
		
		// Generate Nback collectibles
		int x = lastGridOffset;
		int y = 3;
		for ( ; x < grid.numCellsX; x += rate) {
			int rand = Random.Range(0, 2);
			//int y = rand == 0 ? 1 : 6;
			if(timer >= 0f && timer < 40f){
				y = 4;
			} else if (timer >= 40f && timer < 80f) {
				y = 5;
			} else if (timer >= 80f && timer < 120f) {
				y = 6;
			} else if (timer >= 120f && timer < 240f) {
				y = 1;
			} else if (timer >= 240f && timer < 280) {
				y = rand == 0 ? 1 : 5;
			} else if ((timer >= 280f && timer < 320f) || (timer >= 360f && timer < 480f)) {
				y = rand == 0 ? 1 : 6;
			} else if ((timer >= 320f && timer < 360f) || timer >= 480f) {
				y = rand == 0 ? 1 : 4;
			}
			int emptyspace = 0;
			if(timer >= 120 && timer < 125){
				emptyspace = 1;
			}else if(timer >= 240 && timer < 245){
				emptyspace = 1;
			}else if(timer >= 360 && timer < 365){
				emptyspace = 1;
			}else if(timer >= 480 && timer < 485){
				emptyspace = 1;
			}else{
				emptyspace = 0;
			}

			if(emptyspace == 0){
			Transform h = GenerateNbackObjectInGrid(x, y, grid, tc);
			}
		}

		//GenerateGround
		ggen.GenerateGrounds (grid, tc, 0, false);

		//Generate Ceiling!
		//if ((timer >= 0.1f && timer <= 30f) || (timer >= 210f && timer <= 240f)) {
		//	ggen.GenerateGrounds (grid, tc, 4, true);
		if ((timer >= 0 && timer < 40) || (timer >= 200f && timer < 240f) || (timer >= 320f && timer < 360f) || timer >= 480f) {
			ggen.GenerateGrounds (grid, tc, 5, true);
		} else if ((timer >= 40f && timer < 80f) || (timer >= 160f && timer < 200f) || (timer >= 240f && timer < 280f)) {
			ggen.GenerateGrounds (grid, tc, 6, true);
		} else if ((timer >= 80f && timer < 160f) || (timer >= 280f && timer < 320f) || (timer >= 360f && timer < 480f)) {
			ggen.GenerateGrounds (grid, tc, 7, true);
		}
		lastGridOffset = x - grid.numCellsX;
	}
	
	Transform GenerateNbackObjectInGrid(int x, int y, Grid grid, TerrainChunk tc) {
		if (grid.containsObject(x, y)) {
			return null;
		}
		Vector3 spawnPos = grid.GridToWorld (x,y) + tc.transform.position; 
		Transform t = GenerateNbackObject (spawnPos.x, spawnPos.y);
		if (timer >= 420f && timer <= 480f) {
			Transform i = GenerateObstacles (spawnPos.x+5, -7f);
		} else if (timer > 480 && timer < 540f) {
			Transform i = GenerateObstacles (spawnPos.x+5, 5f);
		} else if (timer >= 540f && timer <= 600f) {
			Transform i = GenerateObstacles (spawnPos.x+5, -1f);
		}
		t.parent = tc.gameObject.transform;
		grid.MarkGrid (x, y);
		return t;
	}
	
	Transform GenerateNbackObject(float x, float y) {
		Vector3 spawnPos = new Vector3 (x, y, 0); 
		
		// Generate Random Nback Object
		//int rand = Random.Range (0, nbackObjects.Count);
		int forceCorrect = Random.Range (0, 10);
		int correctNback = Util.findNbackMatch (sequence, nLevel);

		int randNback = Random.Range (0, nbackObjects.Count);
		if (forceCorrect >= 3) {
			while (randNback == previous || randNback == correctNback) {
				randNback = Random.Range (0, nbackObjects.Count);
			}
		} else {
			randNback = correctNback;
			//Debug.Log ("Forcing to correct nback "+correctNback.ToString ());
		}

		previous = randNback;
		Transform t = GameObject.Instantiate (nbackObjects [randNback], spawnPos, Quaternion.identity) as Transform;
		NbackObjControl ctrl = t.gameObject.GetComponent<NbackObjControl> ();


		// Register in sequence
		sequence.Add (ctrl.type);
		if (Util.checkNbackMatch (sequence, nLevel)) {
			ctrl.MarkCorrect ();
		}
		
		return t;
	}

	Transform GenerateObstacles(float x, float y){
		Vector3 spawnPos = new Vector3 (x, y, 0);
		Transform obs = GameObject.Instantiate (obstacle, spawnPos, Quaternion.identity) as Transform;
		return obs;
	}
}

