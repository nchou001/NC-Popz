using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

	public float speed = 1.7f;

	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(speed * Time.deltaTime, 0f, 0f));
	}
}
