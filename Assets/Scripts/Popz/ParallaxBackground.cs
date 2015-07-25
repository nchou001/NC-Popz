using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ParallaxBackground : MonoBehaviour {

	public List<Transform> images; // Images in sequence that make up the background
	private List<Transform> background;

	public float speed = 0.5f; // Note: Bigger speed means slower looking background
	private float offset;

	private Transform cam;
	private Vector3 previousCamPos;

	void Awake () {
		cam = Camera.main.transform;
		//Screen.SetResolution (2048, 1536, false);
		//Screen.SetResolution (1440, 900, false);
	}

	// Use this for initialization
	void Start () {
		offset = images [0].gameObject.GetComponent<SpriteRenderer> ().bounds.size.x - 0.1f;

		offset = 10.19f;

		//Debug.Log (offset);
		ArrangeImages ();

		previousCamPos = cam.position;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Resolution: " + Camera.main.pixelWidth);

		PollForOffscreenImage ();
		ParallaxMove ();
		previousCamPos = cam.position;
	}



	void ArrangeImages () {
		foreach (Transform img in images) {
			RepositionBottomLeft(img);
		}

		for (int i = 1; i < images.Count; ++i) {
			Transform curr = images[i];
			Transform prev = images[i - 1];

			curr.position = prev.position + new Vector3(offset, 0, 0);
			FloorImage(curr);
		}
	}

	void FloorImage (Transform img) {
		float height = img.gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
		Vector3 floor = img.position;
		floor.y = this.transform.position.y + (height / 2.0f);
		img.position = floor;
	}

	void RepositionBottomLeft (Transform img) {
		img.position = this.transform.position;
		Vector3 size = img.gameObject.GetComponent<SpriteRenderer> ().bounds.size;
		size.z = 0;
		Vector3 centerOffset = size / 2.0f;
		img.Translate (centerOffset);
	}

	void RepeatImage (Transform img) {
		Transform end = images [images.Count - 1];
		images.Remove (img);
		img.position = end.position + new Vector3 (offset, 0, 0);
		FloorImage (img);
		img.gameObject.GetComponent<Background> ().isOffscreen = false;
		images.Add (img);
	}

	void PollForOffscreenImage () {
		List<Transform> toAdd = new List<Transform> ();
		foreach (Transform img in images) {
			if (img.gameObject.GetComponent<Background>().isOffscreen) {
				toAdd.Add (img);
			}
		}

		foreach (Transform img in toAdd) {
			RepeatImage(img);
		}
	}

	void ParallaxMove () {
		Vector3 move = cam.position - previousCamPos;
		move.y = 0;
		move.z = 0;
		move.Normalize ();
		move *= speed * Time.deltaTime;

		foreach (Transform image in images) {
			image.transform.Translate(move);
		}
	}
}
