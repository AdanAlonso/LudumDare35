using UnityEngine;
using System.Collections;

public class AudioStates : MonoBehaviour {

	public static AudioStates instance;
	public Player.States state;

	public float fadeLength = 1f;
	AudioSource[] bgmSources;
	public AudioClip[] bgms;
	AudioSource sfxSource;

	void Awake() {
		if (instance == null)
			instance = this;
		if (instance != this)
			Destroy (gameObject);

		bgmSources = new AudioSource[bgms.Length];

		for (int i = 0; i < bgmSources.Length; ++i) {
			bgmSources [i] = gameObject.AddComponent<AudioSource> ();
			bgmSources [i].volume = 0;
			bgmSources [i].loop = true;
			bgmSources [i].clip = bgms [i];
			bgmSources [i].Play ();
		}
		sfxSource = gameObject.AddComponent<AudioSource> ();
	}

	void Start () {
		state = Player.States.Solid;
		StartCoroutine (FSM());
	}

	IEnumerator FSM() {
		while (true) {
			yield return StartCoroutine(state.ToString());
		}
	}

	public IEnumerator ChangeState(Player.States newState) {
		AudioSource fromSource = bgmSources [(int)state];
		AudioSource toSource = bgmSources [(int)newState];
		float counter = 0f;
		while (counter < fadeLength) {
			if (state != newState)
				fromSource.volume = Mathf.Lerp (1, 0, counter / fadeLength);
			toSource.volume = Mathf.Lerp (0, 1, counter / fadeLength);
			yield return 0;
			counter += Time.deltaTime;
		}
		state = newState;
		yield return 0;
	}

	IEnumerator Solid() {
		while (state == Player.States.Solid) {
			yield return 0;
		}
	}

	IEnumerator Liquid() {
		while (state == Player.States.Liquid) {
			yield return 0;
		}
	}

	IEnumerator Gas() {
		while (state == Player.States.Gas) {
			yield return 0;
		}
	}

	public void Play(AudioClip clip) {
		sfxSource.PlayOneShot(clip);
	}

	public void RandomPlay(AudioClip clip) {
		sfxSource.pitch = Random.Range (0.98f, 1.02f);
		sfxSource.PlayOneShot(clip);
	}
}
