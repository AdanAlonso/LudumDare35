using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour {

	public string level = "Level";

	public void Play() {
		SceneManager.LoadScene(level);
	}

	public void Quit() {
		Application.Quit();
	}

}
