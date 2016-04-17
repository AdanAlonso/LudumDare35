using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Goal : MonoBehaviour {

	public UnityEvent goal;
	public AudioClip victoryClip;

	void OnTriggerEnter2D(Collider2D other) {
		AudioStates.instance.Play (victoryClip);
		if (other.CompareTag ("Player") && goal != null)
			goal.Invoke ();
	}

}
