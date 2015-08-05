using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpikeSpawner : MonoBehaviour {

	public Transform obstacle;
	public Transform pillar;
	public static float timer = 0;
	private int lastGridOffset = 5;
	private float spawnTimer = 10;
	private int count = 115;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		spawnTimer -= Time.deltaTime;

		if (timer >= 30 && timer < 60 && spawnTimer <= 0) {
			int rand = Random.Range(0, 2);
			int y = rand == 0 ? -7 : 5;
			Transform i = GenerateSpikes (count, y);
			count += 30;
			spawnTimer = 5;
		}

		else if (timer >= 60 && spawnTimer <= 0) {
			int rand = Random.Range(0, 2);
			int y = rand == 0 ? -7 : 5;
			int y2 = Random.Range (-10, -4);
			int choose = Random.Range (0,3);
			if(choose == 0){
			    Transform i = GenerateSpikes ( count, y);
			} else if (choose == 1){
				Transform i = GeneratePillar (count, y2);
			} else if (choose == 2){
				Transform i = GeneratePillar (count, y2);
				Transform j = GenerateSpikes (count, 5);
			}
			count += 30;
			spawnTimer = 5;
		}
	}

	Transform GenerateSpikes(float x, float y){
		Vector3 spawnPos = new Vector3 (x, y, 0);
		Transform obs = GameObject.Instantiate (obstacle, spawnPos, Quaternion.identity) as Transform;
		return obs;
	}	

	Transform GeneratePillar(float x, float y){
		Vector3 spawnPos = new Vector3 (x, y, 0);
		Transform pil = GameObject.Instantiate (pillar, spawnPos, Quaternion.identity) as Transform;
		return pil;
	}
}
