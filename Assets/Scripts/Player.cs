using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	[System.Serializable]
	public enum States {
		Solid,
		Liquid,
		Gas,
		Win,
		Dead
	}
	public ParticleSystem deathParticles;
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
		public float maxVelocity;
		public GameObject gameObject;
		public Animator animator;
	};

	public AudioClip jumpClip;
	public AudioClip deathClip;
	public KeyCode jumpButton;
	public float jumpPower;
	public StateSettings[] settings;

	bool grounded;
	Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		StartCoroutine (FSM());
	}

	IEnumerator FSM() {
		ChangeState (state);
		while (true) {
			yield return StartCoroutine(state.ToString());
		}
	}

	void ChangeState(States newState) {
		state = newState;
		if ((int) newState < 3) StartCoroutine(AudioStates.instance.ChangeState (newState));
	}

	public void SetWin() {
		ChangeState (States.Win);
	}

	public void Kill() {
		deathParticles.Play ();
		ChangeState(States.Dead);
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
			settings [(int)States.Solid].animator.SetFloat ("speed", rb.velocity.magnitude);
			float h = Input.GetAxis ("Horizontal");
			if (h == -1)
				transform.rotation = Quaternion.Euler (0, 180, 0);	
			else if (h == 1)
				transform.rotation = Quaternion.Euler (0, 0, 0);				
			Vector3 force = Vector3.right * h * settings [(int)States.Solid].force;
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, settings [(int)States.Solid].maxVelocity);
			rb.AddForce(force);

			if (grounded && Input.GetKeyDown (jumpButton)) {
				AudioStates.instance.RandomPlay (jumpClip);
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
			settings [(int)States.Liquid].animator.SetFloat ("speed", rb.velocity.magnitude);
			float h = Input.GetAxis ("Horizontal");
			if (h == -1)
				transform.rotation = Quaternion.Euler (0, 180, 0);	
			else if (h == 1)
				transform.rotation = Quaternion.Euler (0, 0, 0);
			Vector3 force = Vector3.right * h * settings [(int)States.Liquid].force;
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, settings [(int)States.Liquid].maxVelocity);
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
			if (h == -1)
				transform.rotation = Quaternion.Euler (0, 180, 0);	
			else if (h == 1)
				transform.rotation = Quaternion.Euler (0, 0, 0);
			Vector3 force = Vector3.right * h * settings [(int)States.Gas].force;
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, settings [(int)States.Gas].maxVelocity);
			rb.AddForce(force);

			if (Input.GetKeyDown (settings[(int)States.Solid].button)) {
				ChangeState (States.Solid);
			}
			if (Input.GetKeyDown (settings[(int)States.Liquid].button)) {
				ChangeState (States.Liquid);
			}
		}
	}

	IEnumerator Win() {
		while (state == States.Win) {
			yield return 0;
		}
	}

	IEnumerator Dead() {
		Camera.main.GetComponent<Follow> ().follow = false;
		AudioStates.instance.RandomPlay (deathClip);
		while (state == States.Dead) {
			yield return 0;
		}
	}

	void OnCollisionStay2D(Collision2D other) {
		foreach (ContactPoint2D contact in other.contacts) {
			if (contact.point.y < transform.position.y) {
				grounded = true;
				break;
			}
		}
	}

	void OnCollisionExit2D(Collision2D other) {
		foreach (ContactPoint2D contact in other.contacts) {
			if (contact.point.y < transform.position.y) {
				grounded = false;
				break;
			}
		}
	}
}
