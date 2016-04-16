using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Credits : MonoBehaviour {

	public string menu = "MainMenu";

	public void Menu() {
		SceneManager.LoadScene (menu);
	}
}
