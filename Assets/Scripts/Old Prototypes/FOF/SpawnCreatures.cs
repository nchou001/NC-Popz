using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnCreatures : MonoBehaviour {

	public Creature refCreature;
	public Creature refDistractor;

//	public Material[] materials;
//	private List<GameObject> creatureSet;
	private int creatureCount;

	public WayPoint topBound;
	public WayPoint bottomBound;
	public WayPoint leftBound;
	public WayPoint rightBound;

	public int diffLevel;

	// Use this for initialization
	void Start () {
		diffLevel = LevelManager.level;
		creatureCount = 1;
		spawn (diffLevel);
//		creatureSet = new List<GameObject>();
//		creatureSet.Add(refCreature.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		checkCollectSuccess();
	}

	public void spawn (int level) {
		float xMin = leftBound.transform.position.x;
		float xMax = rightBound.transform.position.x;
		float yMax = topBound.transform.position.y;
		float yMin = bottomBound.transform.position.y;
		
//		Vector3 target = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0);
		Vector3 target = new Vector3(5,11,0);

		Creature clone;
		Creature distractor;
		for (int i = 0; i < level; i++) {
			clone = Instantiate(refCreature, target, Quaternion.identity) as Creature;
			chooseColor(i, clone);
//			creatureSet.Add(clone.gameObject);
			creatureCount++;
//			Debug.Log ("Added Creature: " + creatureCount);
			if (i > 0 && i % 2 == 0) {
				Debug.Log ("Added Distractor: " + creatureCount);
				distractor = Instantiate(refDistractor, target, Quaternion.identity) as Creature;
			}
		}
//		Debug.Log ("CreatureCount: " + creatureCount);
	}

	private void chooseColor (int selection, Creature clone) {
		string cc;
		switch (selection)
		{
			case 0:
				clone.GetComponent<Renderer>().material.color = Color.red;
				cc = "r";
				break;
			case 1:
				clone.GetComponent<Renderer>().material.color = Color.yellow;
				cc = "y";
				break;
			case 2:
				clone.GetComponent<Renderer>().material.color = Color.green;
				cc = "g";
				break;
			case 3:
				clone.GetComponent<Renderer>().material.color = Color.magenta;
				cc = "m";
				break;
			case 4:
				clone.GetComponent<Renderer>().material.color = Color.blue;
				cc = "b";
				break;
			default:
				int j = Random.Range(1, 5);
				switch (j)
				{
					case 1:
						clone.GetComponent<Renderer>().material.color = Color.red;
						cc = "r";
						break;
					case 2:
						clone.GetComponent<Renderer>().material.color = Color.blue;
						cc = "b";
						break;
					case 3:
						clone.GetComponent<Renderer>().material.color = Color.green;
						cc = "g";
						break;
					case 4:
						clone.GetComponent<Renderer>().material.color = Color.magenta;
						cc = "m";
						break;
					case 5:
						clone.GetComponent<Renderer>().material.color = Color.yellow;
						cc = "y";
						break;
					default:
						clone.GetComponent<Renderer>().material.color = Color.red;
						cc = "r";
						break;
				}
				break;
		}
//		clone.GetComponent<GoodCreatureSelect>().colorCode = cc;
		clone.GetComponent<GoodCreatureSelect>().setSelectID(cc);
	}

	public void removeCreature () {
		creatureCount--;
	}

	private void checkCollectSuccess () {
		if (creatureCount <= 0) {
//			diffLevel++;
			LevelManager.level++;
			Application.LoadLevel(0);
		}
	}
}
