using UnityEngine;
using System.Collections;

public class Modifier : MonoBehaviour {

	public GameObject plyr;
	public PlayerController plyrCtrl;
	
	public CameraRotationController camRotCtrl;
	
	private ParticleSystem ps;
	private float initER, initSS;
	
	private Transform modCube;
	
	private bool collided;
	
	private float collisionAnimLength = 1.0f;
	private float collisionAnimTimer;
	
	private bool endRotationAnim;

	void Start () {
		plyr = GameObject.FindGameObjectWithTag("Player");
		plyrCtrl = plyr.GetComponent<PlayerController>();
		
		camRotCtrl = GameObject.FindGameObjectWithTag("SpinController").GetComponent<CameraRotationController>();
		
		ps = this.gameObject.GetComponent<ParticleSystem>();
		
		modCube = this.transform.parent.FindChild("ModifierCube");
		
		collided = false;
		collisionAnimTimer = 0.0f;
		
		endRotationAnim = false;
	}
	
	void Update() {
		
		if( collided ) {
			collisionAnimTimer += Time.deltaTime;
			if( collisionAnimTimer >= collisionAnimLength ) {
				endRotationAnim = true;
				ps.emissionRate = initER;
				ps.startSpeed = initSS;
				collided = false;
				collisionAnimTimer = 0.0f;
			}
			else {
				modCube.transform.Rotate(new Vector3(-120.0f * Time.deltaTime, -120.0f * Time.deltaTime, -120.0f * Time.deltaTime));
				ps.emissionRate = initER*3;
				ps.startSpeed = initSS*3;
			}
		}
		
		if( endRotationAnim ) {
			if( modCube.transform.rotation.eulerAngles.magnitude < 0.1f ) {
				modCube.transform.rotation = Quaternion.identity;
				endRotationAnim = false;
			}
			else {
				modCube.transform.rotation = Quaternion.Lerp(modCube.transform.rotation, Quaternion.identity, 0.05f);
			}
		}
		
	}

	public void triggered() {
		initER = ps.emissionRate;
		initSS = ps.startSpeed;
		collided = true;
	}
}
