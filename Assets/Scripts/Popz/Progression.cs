using UnityEngine;
using System.Collections;

public class Progression : MonoBehaviour {
	private int currNumMistakes; // Current number of mistakes (non consecutive)
	public int maxNumMistakes = 3; // Number of mistakes player can make before difficulty is decremented


	private int currNumSuccess; // Current number of consecutive successes
	public int maxNumSuccess = 5; // Number of consecutive successes player must complete before difficulty is incremented

	private int difficulty = 2; // Current difficulty. 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MarkFail () {
		++currNumMistakes;
		if (currNumMistakes >= maxNumMistakes) {
			currNumMistakes = 0;
			--difficulty;
		}
		currNumSuccess = 0;
	}

	public void MarkSuccess () {
		++currNumSuccess;
		if (currNumSuccess >= maxNumSuccess) {
			currNumSuccess = 0;
			++difficulty;
		}
	}

	public int Difficulty () {
		return difficulty;
	}
}
