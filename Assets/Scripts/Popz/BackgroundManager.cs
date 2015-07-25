using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour {

	public float speed = 1f;

	private Queue<Background> backgrounds;
	private Vector3 end;
	private Player player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		backgrounds = new Queue<Background> ();
		GameObject[] bgs = GameObject.FindGameObjectsWithTag ("Background");
		Vector3 start = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth/2, Camera.main.pixelHeight/2, 0f));
		start.z = 0;

		Transform last = bgs [bgs.Length - 1].transform; 
		foreach (var b in bgs) {
			Vector3 spriteExtents = b.GetComponent<SpriteRenderer> ().sprite.bounds.extents;
			b.transform.position = start;
			start += new Vector3 (2 * spriteExtents.x * b.transform.localScale.x, 0f, 0f);
			backgrounds.Enqueue(b.GetComponent <Background> ());
		}
		end = last.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (backgrounds.Peek ().isOffscreen) {
			Background offscreen = backgrounds.Dequeue();
			Vector3 spriteExtents = offscreen.GetComponent<SpriteRenderer> ().sprite.bounds.extents;
			end += new Vector3(2 * spriteExtents.x * offscreen.transform.localScale.x, 0f, 0f);
			offscreen.transform.position = end;
			offscreen.isOffscreen = false;
			backgrounds.Enqueue(offscreen);
		}
		if (player.IsRunning) {
			Vector3 offset = new Vector3 (speed * Time.deltaTime, 0f, 0f);
			transform.Translate(offset);
			end += offset;
		}
	}

	 
}
