using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(BoxCollider))]

public class DragControls : MonoBehaviour { 

	private Creature selectable;
	private Vector3 screenPoint; 
	private Vector3 offset; 
	private float _lockedYPosition;

	void Start() {
		selectable = this.GetComponent<Creature>();
	}
	
	void OnMouseDown() {
		//screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position); // I removed this line to prevent centring 
//		_lockedYPosition = screenPoint.y;
//		if (selectable.getSelectable()) {
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
//		Screen.showCursor = false;
//		}
	}
	
	void OnMouseDrag() 
	{ 
		if (selectable.getSelectable()) {
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
	//		curPosition.x = _lockedYPosition;
			transform.position = curPosition;
		}
	}
	
	void OnMouseUp()
	{
		Cursor.visible = true;
	}
}
