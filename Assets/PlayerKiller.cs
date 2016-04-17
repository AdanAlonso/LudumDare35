using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerKiller : MonoBehaviour {

	public float reloadTime = 3f;

	void OnTriggerEnter2D(Collider2D other) {
		Player p = other.GetComponentInParent<Player> ();
		if (p == null)
			return;
		p.Kill ();
		StartCoroutine (ReloadLevel());
	}

	IEnumerator ReloadLevel() {
		yield return new WaitForSeconds (3f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

}
