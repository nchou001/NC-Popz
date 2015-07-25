using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {

	public float speed = 0.02f;
	private float position = 0f;

	// Update is called once per frame
	void Update () {
		position += speed * Time.deltaTime;
		if (position < 0f) {
			position += 1f;
		}
		else if (position > 1f) {
			position -= 1f;
		}
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2 (position, 0);
	}
}
