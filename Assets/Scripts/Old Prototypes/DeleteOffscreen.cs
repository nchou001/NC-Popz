using UnityEngine;
using System.Collections;

public class DeleteOffscreen : MonoBehaviour {

	void OnBecameInvisible () {
		Destroy (gameObject);
	}
}
