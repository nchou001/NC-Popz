using UnityEngine;
using System.Collections;

public class PortalController : MonoBehaviour {

	private PortalManager pm;

	// Use this for initialization
	void Start () {
		pm = GameObject.FindGameObjectWithTag ("Portals").GetComponent<PortalManager>();
	}

	void OnTriggerStay (Collider other) {
		Teleportable tel = other.gameObject.GetComponent<Teleportable> ();
		if (tel == null || !tel.canTeleport) { return; }
		if (pm.portalA == null || pm.portalB == null) { return; }

		if (this.GetInstanceID () == pm.portalA.GetInstanceID ()) {
			Teleport(other.gameObject, pm.portalB.transform.position);
			tel.destPortal = pm.portalB;
		}
		else if (this.GetInstanceID () == pm.portalB.GetInstanceID ()) {
			Teleport(other.gameObject, pm.portalA.transform.position);
			tel.destPortal = pm.portalA;
		}	    
		other.GetComponent<Teleportable>().canTeleport = false;
	}

	void OnTriggerExit (Collider other) {
		Teleportable tel = other.gameObject.GetComponent<Teleportable> ();
		if (tel == null || tel.destPortal == null) { return; }
		if (this.GetInstanceID () == tel.destPortal.GetInstanceID ()) {
			tel.canTeleport = true;
		}
	}

	private void Teleport (GameObject obj, Vector3 pos) {
		Rigidbody rb = obj.GetComponent<Rigidbody> ();
		rb.isKinematic = true;
		obj.transform.position = pos;
		rb.isKinematic = false;
	}
}

