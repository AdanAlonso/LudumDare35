using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	[System.Serializable]
	public enum States {
		Solid,
		Liquid,
		Gas
	}
	public States state = States.Solid;
	[System.Serializable]
	public struct StateSettings
	{
		public string name;
		public KeyCode button;
		public float force;
		public float mass;
		public float linearDrag;
		public float angularDrag;
		public float gravityScale;
		public GameObject gameObject;
	};

	public float maxVelocity;
	public KeyCode jumpButton;
	public float jumpPower;
	public StateSettings[] settings;

	bool jumping = false;
	Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		ChangeState (state);
		StartCoroutine (FSM());
	}

	IEnumerator FSM() {
		while (true) {
			yield return StartCoroutine(state.ToString());
		}
	}

	void ChangeState(States newState) {
		state = newState;
	}

	void UpdateSettings() {
		rb.mass = settings [(int)state].mass;
		rb.drag = settings [(int)state].linearDrag;
		rb.angularDrag = settings [(int)state].angularDrag;
		rb.gravityScale = settings [(int)state].gravityScale;

		settings [(int)States.Solid].gameObject.SetActive(state == States.Solid);
		settings [(int)States.Liquid].gameObject.SetActive(state == States.Liquid);
		settings [(int)States.Gas].gameObject.SetActive(state == States.Gas);

		settings [(int)States.Solid].gameObject.GetComponent<Collider2D>().enabled = state == States.Solid;
		settings [(int)States.Liquid].gameObject.GetComponent<Collider2D>().enabled = state == States.Liquid;
		settings [(int)States.Gas].gameObject.GetComponent<Collider2D>().enabled = state == States.Gas;
	}

	IEnumerator Solid() {
		UpdateSettings();
		while (state == States.Solid) {
			yield return 0;
			float h = Input.GetAxis ("Horizontal");
			Vector3 force = Vector3.ClampMagnitude(Vector3.right * h * settings [(int)state].force, maxVelocity);
			rb.AddForce(force);

			if (!jumping && Input.GetKeyDown (jumpButton)) {
				jumping = true;
				rb.AddForce(Vector3.up * jumpPower);
			}
			if (Input.GetKeyDown (settings[(int)States.Liquid].button)) {
				ChangeState (States.Liquid);
			}
			if (Input.GetKeyDown (settings[(int)States.Gas].button)) {
				ChangeState (States.Gas);
			}
		}
	}

	IEnumerator Liquid() {
		UpdateSettings();
		while (state == States.Liquid) {
			yield return 0;
			float h = Input.GetAxis ("Horizontal");
			Vector3 force = Vector3.ClampMagnitude(Vector3.right * h * settings [(int)state].force, maxVelocity);
			rb.AddForce(force);

			if (Input.GetKeyDown (settings[(int)States.Solid].button)) {
				ChangeState (States.Solid);
			}
			if (Input.GetKeyDown (settings[(int)States.Gas].button)) {
				ChangeState (States.Gas);
			}
		}
	}

	IEnumerator Gas() {
		UpdateSettings();
		while (state == States.Gas) {
			yield return 0;
			float h = Input.GetAxis ("Horizontal");
			Vector3 force = Vector3.up * settings [(int)state].force + Vector3.ClampMagnitude(Vector3.right * h * settings [(int)state].force, maxVelocity);
			rb.AddForce(force);

			if (Input.GetKeyDown (settings[(int)States.Solid].button)) {
				ChangeState (States.Solid);
			}
			if (Input.GetKeyDown (settings[(int)States.Liquid].button)) {
				ChangeState (States.Liquid);
			}
		}
	}

	void OnCollisionStay2D(Collision2D other) {
		if (other.gameObject.CompareTag("Ground") && other.transform.position.y < transform.position.y) {
			jumping = false;
		}
	}
}
