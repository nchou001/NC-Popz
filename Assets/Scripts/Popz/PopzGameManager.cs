using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameModes {
	Pattern,
	Nback,
	Multiobj
};

public class PopzGameManager : MonoBehaviour {
	// Toggles for game modes
	public bool enablePattern;
	public bool enableNback;
	public bool enableMultiObject;
	private List<GameModes> gameModes;

	// Game Managers for each game type
	private PatternLevelManager patternGame;
	private MultiObjGameManager multiObjGame;
	private NbackGameManager nbackGame;

	private Player player;


	void Awake () {
		// Use settings if it was set and loaded from Menu
		if (Settings.isSet) {
			enablePattern = Settings.togglePattern;
			enableNback = Settings.toggleNback;
			enableMultiObject = Settings.toggleMultiObj;
		}

		// Set game modes
		gameModes = new List<GameModes> ();
		if (enablePattern)
			gameModes.Add (GameModes.Pattern);
		if (enableNback)
			gameModes.Add (GameModes.Nback);
		if (enableMultiObject)
			gameModes.Add (GameModes.Multiobj);
	}

	// Use this for initialization
	void Start () {
		// Use settings if it was set and loaded from Menu
		if (Settings.isSet) {
			enablePattern = Settings.togglePattern;
			enableNback = Settings.toggleNback;
			enableMultiObject = Settings.toggleMultiObj;
		}
		
		// Set game modes
		gameModes = new List<GameModes> ();
		if (enablePattern)
			gameModes.Add (GameModes.Pattern);
		if (enableNback)
			gameModes.Add (GameModes.Nback);
		if (enableMultiObject)
			gameModes.Add (GameModes.Multiobj);


		patternGame = FindObjectOfType (typeof(PatternLevelManager)) as PatternLevelManager;
		multiObjGame = FindObjectOfType (typeof(MultiObjGameManager)) as MultiObjGameManager;
		nbackGame = FindObjectOfType (typeof(NbackGameManager)) as NbackGameManager;
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();

		// Start each game if enabled
		// Note: Disable/Enable of Pattern and Nback currently done
		// by disabling their generators in TerrainGenerator.cs, should be here

		if(!enableNback)
		{
			StartCoroutine("GameTimeWait",4f);	
		}
		else
		{
			player.canJump = true;
			player.canRun = true;
			player.canDoubleJump = true;
			
			player.GetComponent<Animator>().SetInteger("PlayerState",1);
			
			if(enablePattern)
			{
				patternGame.StartPatternGame();
			}
			if (enableMultiObject) {
				multiObjGame.startLevel();
			}
		}
		//StartCoroutine("GameTimeWait",4f);	



	}
	
	IEnumerator GameTimeWait(float displayTime)
	{
		yield return new WaitForSeconds(displayTime); 
		player.canJump = true;
		player.canRun = true;
		player.canDoubleJump = true;

		player.GetComponent<Animator>().SetInteger("PlayerState",1);

		/*if (enableMultiObject) {
			multiObjGame.startLevel();
		}*/
		if(enablePattern)
		{
			patternGame.StartPatternGame();
		}
		if (enableMultiObject) {
			multiObjGame.startLevel();
		}

	}

	public List<GameModes> Modes() {
		return gameModes;
	}

//	void OnGUI() {
//		/*
//		float screenScale = Screen.width / 480.0f;
//		Matrix4x4 scaledMatrix = Matrix4x4.Scale(new Vector3(screenScale,screenScale,screenScale));
//		GUI.matrix = scaledMatrix;
//		*/
//		GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = 20;
//
//		float width = 80;
//		float height = 90;
//		var text = "Menu";
//		if (GUI.Button (new Rect (Screen.width - width, Screen.height - height, width, height), text)) {
//			Application.LoadLevel("PopzMenu");
//		}
//	}
}
