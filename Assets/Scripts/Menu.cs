using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour {

	public string level = "Level";
	public string credits = "Credits";

	public void Play() {
		SceneManager.LoadScene(level);
	}

	public void Credits() {
		SceneManager.LoadScene(credits);
	}

	public void Quit() {
		Application.Quit();
	}

}
