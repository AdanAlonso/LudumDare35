using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour {

	public Animator animator;

	public void Play() {
		SceneManager.LoadScene("Level");
	}

	public void Change() {
		animator.SetTrigger ("Change");
	}

	public void Quit() {
		Application.Quit();
	}

}
