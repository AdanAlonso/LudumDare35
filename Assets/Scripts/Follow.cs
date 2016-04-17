using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	[System.Serializable]
	public struct Bounds {
		public float minX;
		public float maxX;
		public float minY;
		public float maxY;
	}

	public Transform target;
	public bool follow = true;
	public bool followX;
	public bool followY;
	public bool bounded;
	public Bounds bounds;

	void Update(){
		Vector3 newPosition = transform.position;
		if (follow) {
			newPosition = target.position;
			if (!followX) {
				newPosition.x = 0;
			}
			if (!followY) {
				newPosition.y = 0;
			}
			newPosition.z = transform.position.z;
		}
		if (bounded) {
			newPosition.x = Mathf.Clamp (newPosition.x, bounds.minX, bounds.maxX);
			newPosition.y = Mathf.Clamp (newPosition.y, bounds.minY, bounds.maxY);
		}
		transform.position = newPosition;
	}
}
