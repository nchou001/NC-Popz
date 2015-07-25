using UnityEngine;
using System.Collections;

public class ConstantRotate : MonoBehaviour {
	
	void Update () {
		this.transform.Rotate (new Vector3 (-30.0f * Time.deltaTime, -30.0f * Time.deltaTime, -30.0f * Time.deltaTime));
	}
}
