
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Settings : MonoBehaviour {
	static public bool togglePattern = false;
	static public bool toggleNback = false;
	static public bool toggleMultiObj = false;
	static public bool togglePlatformsNback = false;
	static public bool isSet = false;
	static public int nbackNavigationDifficulty = 3;
	
	public Toggle Pattern;
	public Toggle Nback;
	public Toggle MultiObj;
	public Toggle PlatformsNback;
	public Slider diff;
	
	void Awake () {
		DontDestroyOnLoad (this);
	}
	
	// Use this for initialization
	void Start () {
		togglePattern = false;
		toggleNback = false;
		toggleMultiObj = false;
		togglePlatformsNback = false;
		nbackNavigationDifficulty = 3;
		Pattern = Pattern.GetComponent<Toggle> ();
		Nback = Nback.GetComponent<Toggle> ();
		MultiObj = MultiObj.GetComponent<Toggle> ();
		PlatformsNback = PlatformsNback.GetComponent<Toggle> ();
		diff = diff.GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void PatternTog()
	{
		if (Pattern.isOn) {
			togglePattern = true;
		} else {
			togglePattern = false;
		}
	}
	public void NbackTog()
	{
		if (Nback.isOn) {
			toggleNback = true;
		} else {
			toggleNback = false;
		}
	}
	public void MultiObjTog()
	{
		if (MultiObj.isOn) {
			toggleMultiObj = true;
		} else {
			toggleMultiObj = false;
		}
	}
	public void PlatformsTog()
	{
		if (PlatformsNback.isOn) {
			togglePlatformsNback = true;
		} else {
			togglePlatformsNback = false;
		}
	}
	public void SetDifficulty()
	{
		nbackNavigationDifficulty = (int)diff.value;
	}
}

