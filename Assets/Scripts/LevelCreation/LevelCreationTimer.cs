using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCreationTimer : MonoBehaviour
{
    public float allowedLevelSetupTime;
    Text timerText;

	void Awake ()
    {
        timerText = GetComponent<Text>();
        StartCoroutine(StartCountdown());
	}
	
	IEnumerator StartCountdown()
    {
        float remainingLevelSetupTime = allowedLevelSetupTime + .5f; // to account for start delays
        while (remainingLevelSetupTime > 0)
        {
            remainingLevelSetupTime -= Time.deltaTime;
            timerText.text = "" + Mathf.Clamp(Mathf.Ceil(remainingLevelSetupTime), 1, allowedLevelSetupTime);
            yield return null;
        }

        timerText.text = "GO";
        Camera.main.GetComponent<CameraBehavior>().LerpToMatchStart();
    }
}
