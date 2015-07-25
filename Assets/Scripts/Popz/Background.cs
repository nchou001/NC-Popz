using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
	
	public bool isOffscreen = false;

	void OnBecameInvisible () {
		isOffscreen = true;
	}
}
