using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtoninGame : MonoBehaviour {

	void Update () {
		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKey (KeyCode.Escape))
				TitleMenu ();
		}
	}

	public void TitleMenu(){
		SceneManager.LoadScene ("main");
	}

	public void ShutGame(){
		Application.Quit();
	}

}