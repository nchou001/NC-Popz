using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsMenu : MonoBehaviour {

	public Button playButton;
	
	// Use this for initialization
	void Start () {
		playButton = playButton.GetComponent<Button> ();
	}
	
	public void Play()
	{
		Settings.isSet = true;
		Application.LoadLevel ("RunningAnimTest");
	}
}
