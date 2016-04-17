using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {

	public Transform door;
	public Transform switchTarget;
	public Transform doorTarget;
	public float switchTime = 0.5f;
	public float delay = 0.5f;
	public float doorTime = 0.5f;
	public AudioClip switchPress;
	public AudioClip doorOpen;

	void OnTriggerEnter2D(Collider2D other) {
		Player p = other.GetComponentInParent<Player>();
		if (p != null && p.state == Player.States.Solid) {
			StartCoroutine(PressSwitch ());
		}
	}

	IEnumerator PressSwitch() {
		AudioStates.instance.Play (switchPress);
		float counter = 0;
		Vector3 origin = transform.position;
		while (counter < switchTime) {
			yield return 0;
			transform.position = Vector3.Lerp (origin, switchTarget.position, counter/switchTime);
			counter += Time.deltaTime;
		}
		yield return new WaitForSeconds (delay);
		AudioStates.instance.Play (doorOpen);
		StartCoroutine (OpenDoor());
	}

	IEnumerator OpenDoor() {
		float counter = 0;
		Vector3 origin = door.position;
		while (counter < doorTime) {
			yield return 0;
			door.position = Vector3.Lerp (origin, doorTarget.position, counter/switchTime);
			counter += Time.deltaTime;
		}
		yield return 0;
	}
}
