using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public static int level = 1;
	private bool runTimer;
	private float levelTimeLimit = 30f;
//	private bool penalty;
	private int life;

	private float endTicker;
	private float endWaitTime = 6f;

	// Use this for initialization
	void Start () {
		runTimer = true;
//		penalty = false;
		life = 3;
		endTicker = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (runTimer) {
			levelTimeLimit -= Time.deltaTime;
			checkState();
		}
		else if (!runTimer && endTicker >= endWaitTime) {
			Application.LoadLevel(0);
		}
		else {
			endTicker += Time.deltaTime;
		}
	}

	void OnGUI () {
		GUI.Box(new Rect(Screen.width - 70, Screen.height - 70, 65, 20), "Time: " + levelTimeLimit.ToString("0"));
		GUI.Box(new Rect(Screen.width - 60, Screen.height - 45, 50, 20), "Life: " + life.ToString("0"));
	}

	void checkState () {
		if (levelTimeLimit <= 1 || life < 1)
			runTimer = false;
	}

	public void setPenalty () {
		life--;
	}

	public bool getState () {
		return runTimer;
	}
}
