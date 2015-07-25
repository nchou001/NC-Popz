using UnityEngine;
using System.Collections;

public class Teleportable : MonoBehaviour {

	public bool canTeleport;
	public PortalController destPortal;

	// Use this for initialization
	void Start () {
		canTeleport = true;
	}

}
