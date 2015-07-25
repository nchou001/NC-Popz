using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

//	// Use this for initialization
//	void Start () {
//		Screen.SetResolution (1024, 768, true);
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
//
//	// Produce new rectangles to position menu objects from top to bottom
//	struct GUIRectFactory {
//		int y_index;
//		float x_pos;
//		float y_pos;
//		float width;
//		float height;
//
//		// Desired start position of GUI objects x, y (top left), width, height of objects
//		public GUIRectFactory (float x, float y, float width, float height) {
//			this.y_index = -1;
//			this.x_pos = x;
//			this.y_pos = y;
//			this.width = width;
//			this.height = height;
//		}
//
//		public Rect Generate () {
//			++y_index;
//			return new Rect (x_pos, y_pos + (height * y_index), width, height);
//		}
//	};
//
////	void OnGUI() {
////		float screenScale = Screen.width / 480.0f;
////		Matrix4x4 scaledMatrix = Matrix4x4.Scale(new Vector3(screenScale,screenScale,screenScale));
////		GUI.matrix = scaledMatrix;
////
////		GUI.skin.toggle.fixedWidth = 200;
////
////		var patternLabel = "Enable Pattern Game";
////		var nbackLabel = "Enable Nback Game";
////		var multiObjLabel = "Enable Multi Object Game";
////		var platformsLabel = "Enable Platform Nbacks Game";
////
////		float width = 200;
////		float height = 30;
////		float startY = height;
////		float startX = 10;
////
////		GUIRectFactory factory = new GUIRectFactory (startX, startY, width, height);
////
////		Settings.togglePattern = GUI.Toggle (factory.Generate (), 
////		                                     Settings.togglePattern, 
////		                                     patternLabel);
////
////		Settings.toggleNback = GUI.Toggle (factory.Generate (), 
////		                                   Settings.toggleNback, 
////		                                   nbackLabel);
////
////		Settings.toggleMultiObj = GUI.Toggle (factory.Generate(), 
////		                                      Settings.toggleMultiObj, 
////		                                      multiObjLabel);
////
////		Settings.togglePlatformsNback = GUI.Toggle (factory.Generate (),
////		                                            Settings.togglePlatformsNback, 
////		                                            platformsLabel);
////
////		int textToInt = 0;
////		int difficulty = Settings.nbackNavigationDifficulty;
////		string text = GUI.TextField (factory.Generate (), difficulty.ToString ());
////		if (int.TryParse (text, out textToInt)) {
////			difficulty = textToInt;
////		} else if (text == "") {
////			difficulty = 0;
////		}
////
////		Settings.nbackNavigationDifficulty = difficulty;
////
////		if (GUI.Button (factory.Generate (), "Play Game")) {
////			Settings.isSet = true;
////			Application.LoadLevel("RunningAnimTest");
////		}
////	}
}