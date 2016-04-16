using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Goal : MonoBehaviour {

	public UnityEvent goal;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player") && goal != null)
			goal.Invoke ();
	}

}
