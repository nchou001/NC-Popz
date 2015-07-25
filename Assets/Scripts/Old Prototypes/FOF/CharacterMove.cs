using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour {

	public Transform followingCamera;
	public WayPoint endMarker;
	public WayPoint charEndMark;
	private Vector3 charTarget;
	private Vector3 camTarget;
	public float speed;
	private float startTime;
	private float charJourneyLength;
	private float camJourneyLength;
	public float smooth;

	void Start() {
		speed = 0.1F;
		smooth = 5.0F;
		startTime = Time.time;
		charTarget = charEndMark.transform.position;
		charTarget.x = transform.position.x;
		charTarget.z = 0;
		camTarget = endMarker.transform.position;
		charJourneyLength = Vector3.Distance(transform.position, charTarget);
		camJourneyLength = Vector3.Distance(transform.position, camTarget);
	}

	void Update() {
		float distCovered = (Time.time - startTime) * speed;
		float charStep = distCovered / charJourneyLength;
		float camStep = distCovered / camJourneyLength;
		transform.position = Vector3.Lerp(transform.position, charTarget, charStep);
		followingCamera.position = Vector3.Lerp(followingCamera.position, camTarget, camStep);
	}
}
