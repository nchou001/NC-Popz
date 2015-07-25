using UnityEngine;
using System.Collections;

public class FixedHeight : MonoBehaviour {

	public float height;

	// Use this for initialization
	void Awake () {
		height = transform.position.y;
		transform.position = new Vector3(transform.position.x, height, transform.position.z);
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x, height, transform.position.z);
	}

	public void FixPosition () {
		transform.position = new Vector3(transform.position.x, height, transform.position.z);
	}
}
