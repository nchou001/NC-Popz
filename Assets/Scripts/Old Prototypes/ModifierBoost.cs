using UnityEngine;
using System.Collections;

public class ModifierBoost : Modifier {

	private float boostVel = 10.0f;
	
	void OnTriggerEnter(Collider col) {
		
		if( col.gameObject.tag == "Player" ) {

			triggered();

			plyr.GetComponent<PlayerController>().boostMod = true;

			switch(camRotCtrl.view) {
				case CameraRotationController.VIEW.FRONT:
				case CameraRotationController.VIEW.BACK:
					plyr.GetComponent<Rigidbody>().velocity = new Vector3(boostVel, 0, 0);
					break;
				case CameraRotationController.VIEW.RIGHT:
				case CameraRotationController.VIEW.LEFT:
					plyr.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -boostVel);
					break;
			}
		}
	}
}
