using UnityEngine;
using System.Collections;

public class BonusControl : MonoBehaviour {

	private bool isBonus;
	private Animator bonusAnim;

	void Awake(){
		bonusAnim = GetComponent<Animator> ();
	}

	void Start(){
		isBonus = false;
		if (PlayerPrefs.HasKey ("Bonus"))
		if (PlayerPrefs.GetInt ("Bonus") == 1) {
			isBonus = true;
			EnableBonus ();
		}
	}

	public bool BonusStat(){
		return isBonus;
	}

	public void EnableBonus(){
		bonusAnim.SetBool ("isBonus", true);
	}

	public void ActiveBonus(){
		isBonus = true;
		PlayerPrefs.SetInt ("Bonus", 1);
		PlayerPrefs.Save ();
	}

	public void EmptyBonus(){
		isBonus = false;
		bonusAnim.SetBool ("isBonus", false);
		PlayerPrefs.SetInt ("Bonus", 0);
		PlayerPrefs.Save ();
	}
}
