using UnityEngine;
using System.Collections;

public class ModifierRotate : Modifier {

	void OnTriggerEnter(Collider col) {
		
		if( col.gameObject.tag == "Player" ) {
			
			triggered();

			camRotCtrl.performRotateRight();
		}
	}
}
