using UnityEngine;
using System.Collections;

public class PortalManager : MonoBehaviour {

	public Object portal;
	public PortalController destPortal;

	public PortalController portalA;
	public PortalController portalB;
	private bool createPortal;
	public bool canCreate = true;

	// Use this for initialization
	void Start () {
		createPortal = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && canCreate) {
			createPortal = true;
		}
	}

	void FixedUpdate () {
		if (!createPortal) { return; }
		if (portalB != null) {
			Destroy (portalB.gameObject.transform.root.gameObject, 0.0f);
		}
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		portalB = portalA;
		GameObject obj = (GameObject) Instantiate(portal, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
		portalA = obj.GetComponentInChildren<PortalController>();
		createPortal = false;

	}

}
