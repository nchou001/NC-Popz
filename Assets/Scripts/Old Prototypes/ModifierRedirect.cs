using UnityEngine;
using System.Collections;

public class ModifierRedirect : Modifier {

	void OnTriggerEnter(Collider col) {
		
		if( col.gameObject.tag == "Player" ) {

			triggered();

			if( plyrCtrl.isMovingRight ) {
				plyrCtrl.isMovingRight = false;
				plyrCtrl.isMovingLeft = true;
			}
			else {
				plyrCtrl.isMovingLeft = false;
				plyrCtrl.isMovingRight = true;
			}
		}
	}
}
