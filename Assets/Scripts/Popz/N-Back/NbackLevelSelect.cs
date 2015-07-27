using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NbackLevelSelect : MonoBehaviour {



	void OnGUI()
	{
		if (Time.timeScale == 0f) {
			if (GUI.Button (new Rect (0, 50, 50, 50), "Level1")) {
				Application.LoadLevel (Application.loadedLevel);
				NbackGenerator.setLevel(1);
			}else if(GUI.Button (new Rect(0, 100, 50, 50), "Level2")){
				Application.LoadLevel (Application.loadedLevel);
				NbackGenerator.setLevel(2);
			}else if(GUI.Button (new Rect(0, 150, 50, 50), "Level3")){
				Application.LoadLevel (Application.loadedLevel);
				NbackGenerator.setLevel(3);
			}else if(GUI.Button (new Rect(0, 200, 50, 50), "Level4")){
				Application.LoadLevel (Application.loadedLevel);
				NbackGenerator.setLevel(4);
			}else if(GUI.Button (new Rect(0, 250, 50, 50), "Level5")){
				Application.LoadLevel (Application.loadedLevel);
				NbackGenerator.setLevel(5);
			}
		}
	}
}
