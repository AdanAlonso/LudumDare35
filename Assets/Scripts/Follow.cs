using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public Transform target;
	public bool followX;
	public bool followY;

	void Update(){
		Vector3 newPosition = target.position;
		if (!followX) {
			newPosition.x = 0;
		}
		if (!followY) {
			newPosition.y = 0;
		}
		newPosition.z = transform.position.z;
		transform.position = newPosition;
	}
}
