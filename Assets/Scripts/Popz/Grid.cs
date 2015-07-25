using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	
	public int numCellsY = 8; // Number of cells in the screen
	public int numCellsX; // Depends on screen size 

	public float cellSizeX, cellSizeY;
	public float width, height;

	private bool[,] grid; // grid[x,y] is true is the grid cell (x,y) contains an object

	void Awake () {	
		Vector3 topRight = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
		width = topRight.x - bottomLeft.x;
		height = topRight.y - bottomLeft.y;
		cellSizeY = height / ((float)numCellsY);
		cellSizeX = 1.6f;

		//TODO: Fix these to be dynamic. These numbers are based off current background sprites of 2048x1536 resolution
		cellSizeX = 2.45f;
		cellSizeY = 1.8f;

		numCellsX = Mathf.CeilToInt (width / cellSizeX);
		grid = new bool[numCellsX * 2, numCellsY];
	}

	// Calculates cell sizes and sets up position
	void Start () {
	}

	public void ClearGrid () {
		for (int x = 0; x < numCellsX; x++) {
			for (int y = 0; y < numCellsY; y++) {
				grid[x,y] = false;
			}
		}
		for (int x = numCellsX + 1; x < grid.GetLength(0); x++) {
			for (int y = 0; y < numCellsY; y++) {
				grid[x - numCellsX - 1, y] = grid[x,y];
				grid[x,y] = false;
			}
		}
	}

	// Called when an object is placed in grid cell (x,y)
	public void MarkGrid (int x, int y) {
		if (x > numCellsX || y > numCellsY || x < 0 || y < 0) {
			//Debug.Log ("out of bounds grid");
		}
		grid[x, y] = true;
	}

	// Returns the world position of the specified grid cell
	public Vector3 GridToWorld (int x, int y) {
		if (x > numCellsX || y > numCellsY || x < 0 || y < 0) {
			//Debug.Log ("out of bounds grid");
		}
		Vector3 pos = new Vector3(0f,0f,0f);
		pos.x = (((float) x) + 0.5f) * cellSizeX;
		pos.y = (((float) y) + 0.5f) * cellSizeY;
		return pos;
	}

	// Is true if grid cell (x,y) contains an object 
	public bool containsObject (int x, int y) {
		if (x > grid.GetLength(0) || y > grid.GetLength(1) || x < 0 || y < 0) {
			//Debug.Log ("out of bounds grid");
			return false;
		}
		else {
			return grid[x,y];
		}
	}

}
