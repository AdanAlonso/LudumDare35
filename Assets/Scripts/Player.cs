using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public KeyCode leftButton = KeyCode.LeftArrow;
	public KeyCode rightButton = KeyCode.RightArrow;
	public float speed = 10f;
	public KeyCode jumpButton = KeyCode.Space;
	public float jumpPower = 5f;

	Rigidbody2D rb;
	bool jumping = false;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}

	void Update () {
		float h = Input.GetAxis ("Horizontal");
		transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.right * h, Time.deltaTime * speed);

		if (!jumping && Input.GetKeyDown (jumpButton)) {
			jumping = true;
			rb.AddForce(Vector3.up * jumpPower);
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Ground") && other.transform.position.y < transform.position.y) {
			jumping = false;
		}
	}
}
