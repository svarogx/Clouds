using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;

public class AdsControl : MonoBehaviour {
	
	public Button AdButton;
	public BonusControl bonusControl;

	void Update(){
#if UNITY_ADS
        if (Advertisement.IsReady ("rewardedVideo") && !bonusControl.BonusStat ())
			AdButton.interactable = true;
		else
			AdButton.interactable = false;
#endif
	}

	public void ShowRewardedAd(){
#if UNITY_ADS
        if (Advertisement.IsReady("rewardedVideo")){
			Time.timeScale = 0.0f;
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
#endif 
	}
#if UNITY_ADS
   private void HandleShowResult(ShowResult result){
		Time.timeScale = 1.0f;
		switch (result){
		case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");
			if (!bonusControl.BonusStat ())
				bonusControl.EnableBonus ();
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}
#endif
}


