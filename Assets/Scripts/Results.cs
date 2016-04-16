using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Results : MonoBehaviour {

	public void Continue() {
		SceneManager.LoadScene("MainMenu");
	}
}
