using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContainerButtons : MonoBehaviour {

	private int selection;
//	public Texture btnTexture;
//	private string findName;
	public SpawnCreatures levelStart;

	private GameObject[] creatures;

	private int toolbarWidth = 500;
	private int toolbarHeight = 50;
	
//	private string[] toolbarStrings;
	private List<string> selectSet;

	// Use this for initialization
	void Start () {
		selection = -1;
//		toolbarStrings = new string[] {""};
		selectSet = new List<string>();
		buildToolBar();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
//		selection = GUI.Toolbar(new Rect(Screen.width/2 - toolbarWidth/2, 
//		                                Screen.height - toolbarHeight, toolbarWidth, toolbarHeight), 
//		                        		selection, selectSet.ToArray());

		if (GUI.Button(new Rect(10, Screen.height - 60, 50, 50), "Light")) {
			if (creatures == null)
				creatures = GameObject.FindGameObjectsWithTag("creature");
			
			foreach (GameObject creature in creatures) {
				creature.GetComponent<Creature>().reveal();
			}
		}
	}

	public int getSelection () {
		return selection;
	}
/*
	private bool isName(string name)
	{
		return (name==findName);
	}
*/
	private void buildToolBar () {
		int level = levelStart.diffLevel;
		if (level >= 0) {
			selectSet.Add("Blue");
		}
		if (level >= 1) {
			selectSet.Add("Red");
		}
		if (level >= 2) {
			selectSet.Add("Yellow");
		}
		if (level >= 3) {
			selectSet.Add("Green");
		}
		if (level >= 4) {
			selectSet.Add("Magenta");
		}
	}
	/*
	public int addSelectID (string buttonName) {
		int id;
		if (selectSet.Contains(buttonName)) {
			findName = buttonName;
			id = selectSet.FindIndex(isName);
		}
		else {
			selectSet.Add(buttonName);
			//toolbarStrings = selectSet.ToArray();
			id = selectSet.Count;
		}
		return id;
	}
	*/

}
