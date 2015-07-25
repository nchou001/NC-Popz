using UnityEngine;
using System.Collections;

public class TerrainChunk : MonoBehaviour {

	private TerrainGenerator terrainGen;
	

	void Start () {
		terrainGen = GameObject.FindGameObjectWithTag ("TerrainGen").GetComponent<TerrainGenerator> ();
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.name.Equals("TerrainGenerator")) {
			terrainGen.GenerateTerrainRelevantTo(transform.position);
		}
		
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.name.Equals("TerrainGenerator")) {
			for (int i = transform.childCount - 1; i >= 0; i--){
				GameObject.Destroy(transform.GetChild(i).gameObject);
			}
			GameObject.Destroy(gameObject);
		}
	}
}
