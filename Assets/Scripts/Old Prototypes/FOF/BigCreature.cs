using UnityEngine;
using System.Collections;

public class BigCreature : MonoBehaviour {

	public LevelManager manager;
	public WayPoint endMarker;
	private Vector3 target;
	private float speed;
	private float startTime;
	private float journeyLength;
	private float smooth;

	// Use this for initialization
	void Start () {
		speed = 0.05F;
		smooth = 5.0F;
		startTime = Time.time;
		target = endMarker.transform.position;
//		charTarget.x = transform.position.x;
//		charTarget.z = 0;
//		camTarget = endMarker.transform.position;
//		charJourneyLength = Vector3.Distance(transform.position, charTarget);
		journeyLength = Vector3.Distance(transform.position, target);
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!manager.getState()) {
			float distCovered = (Time.time - startTime) * speed;
			float charStep = distCovered / journeyLength;
//			float camStep = distCovered / camJourneyLength;
			transform.position = Vector3.Lerp(transform.position, target, charStep);
		}
	}
}
