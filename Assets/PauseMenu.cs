using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public Button menuButton;
	public Button okButton;
	public Button pauseButton;
	public bool isPaused;
	public GameObject PauseCanvas;

	// Use this for initialization
	void Start () {
		menuButton = menuButton.GetComponent<Button> ();
		okButton = okButton.GetComponent<Button> ();
		pauseButton = pauseButton.GetComponent<Button> ();
		isPaused = false;
	}

	void Update () {
		if (isPaused) {
			PauseCanvas.SetActive (true);
			Time.timeScale = 0f;
		} else {
			PauseCanvas.SetActive (false);
			Time.timeScale = 1f;
		}
	}

	public void Pause()
	{
		isPaused = true;
	}

	public void OpenMenu()
	{
		Application.LoadLevel (0);
	}
	
	public void ResumeGame()
	{
		isPaused = false;
	}
}
