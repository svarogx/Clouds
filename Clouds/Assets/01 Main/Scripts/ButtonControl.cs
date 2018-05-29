using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour {

	void Update () {
		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKey(KeyCode.Escape))
				ShutGame ();
		}
	}

	public void NewGame(){
		PlayerPrefs.SetInt ("Bonus", 0);
		PlayerPrefs.SetInt("gamepoints", 0);
		PlayerPrefs.SetFloat ("history", 0.0f);
		PlayerPrefs.SetInt ("easy", 0);
		PlayerPrefs.SetInt ("medium", 0);
		PlayerPrefs.SetInt ("hard", 0);
		PlayerPrefs.SetInt ("stat", 0);
		PlayerPrefs.Save ();
		SceneManager.LoadScene ("prototype");
	}

	public void ContinueGame(){
		PlayerPrefs.SetInt ("Bonus", 0);
		PlayerPrefs.SetInt ("stat", 0);
		PlayerPrefs.Save ();
		SceneManager.LoadScene ("prototype");
	}

	public void ShutGame(){
		Application.Quit();
	}

}
