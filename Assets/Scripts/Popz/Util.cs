using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public struct FieldInfo {
	public float width;
	public float height;

	public float lowerX;
	public float lowerY;
	public float upperX;
	public float upperY;
}

public class Util : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	static public Color randomColor () {
		List<Color> bins = new List<Color> ();
		bins.Add (Color.red);
		bins.Add (Color.blue);
		bins.Add (Color.green);
		bins.Add (Color.magenta);
		bins.Add (Color.yellow);

		return bins[Random.Range (0, 5)];
	}

	static public Color randomColorFromSet (List<Color> colorSet) {
		return colorSet [Random.Range (0, colorSet.Count)];
	}

	static public List<Color> genColorSet (int num) {
		List<Color> bins = new List<Color> ();
		bins.Add (Color.red);
		bins.Add (Color.blue);
		bins.Add (Color.green);
		bins.Add (Color.magenta);
		bins.Add (Color.yellow);
		
		return bins.OrderBy (item => Random.value).Take (num).ToList ();
	}

	static public FieldInfo getFieldInfo (GameObject field) {
		/*
		FieldInfo ret;


		var fieldRenderer = field.GetComponentInChildren<Renderer> ();
		ret.width = fieldRenderer.bounds.size.x;
		ret.height = fieldRenderer.bounds.size.y;

		ret.lowerX = field.transform.position.x;
		ret.lowerY = field.transform.position.y;
		ret.upperX = ret.lowerX + ret.width;
		ret.upperY = ret.lowerY + ret.height;
		*/

		FieldInfo ret;

		var bottomLeftCorner = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0));
		var upperRightCorner = Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 1, 0));

		ret.lowerX = bottomLeftCorner.x;
		ret.lowerY = bottomLeftCorner.y;
		ret.upperX = upperRightCorner.x;
		ret.upperY = upperRightCorner.y;

		ret.width = ret.upperX - ret.lowerX;
		ret.height = ret.upperY - ret.lowerY;

		return ret;
	}

	static public float getDistance2D (GameObject x, GameObject y) {
		var x2D = new Vector2 (x.transform.position.x, x.transform.position.y);
		var y2D = new Vector2 (y.transform.position.x, y.transform.position.y);
		return (x2D - y2D).magnitude;
	}

	static public bool checkNbackMatch (List<Mineral> sequence, int n) {
		if (n >= sequence.Count) {
			return false;
		}
		if (sequence[sequence.Count - n - 1] == sequence[sequence.Count - 1]) {
			return true;
		}
		return false;
	}

	static public int findNbackMatch (List<Mineral> sequence, int n){
		if (n >= sequence.Count) {
			return Random.Range (0, 5);
		}
		NbackObjControl temp;
        if (sequence [sequence.Count - n] == Mineral.Yellow) {
			return 0;
		} else if (sequence [sequence.Count - n] == Mineral.Green) {
			return 1;
		} else if (sequence [sequence.Count - n] == Mineral.Red) {
			return 2;
		} else if (sequence [sequence.Count - n] == Mineral.Blue) {
			return 3;
		} else if (sequence [sequence.Count - n] == Mineral.White) {
			return 4;
		} else {
			return 0;
		}
	}
}
