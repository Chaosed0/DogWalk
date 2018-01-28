using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float allowedLevelSetupTime;
    public float raceTime;

    Text timerText;
    bool stopEarly = false;

	void Awake ()
    {
        timerText = GetComponent<Text>();
        StartLevelSetupCountdown();

        EventBus.Subscribe<RoundActuallyStartEvent>((x) => StartRaceCountdown());
	}

    [SubscribeGlobal]
    void HandleLevelCreationStartEvent(LevelCreationActuallyStartEvent e)
    {
        StartLevelSetupCountdown();
    }

    public void StartLevelSetupCountdown()
    {
        StartCoroutine(StartLevelSetupCountdownCoroutine());
    }

    public void StartRaceCountdown()
    {
        StartCoroutine(StartRaceCountdownCoroutine());
    }

    public void FinishLevelSetupEarly()
    {
        stopEarly = true;
    }
	
	IEnumerator StartLevelSetupCountdownCoroutine()
    {
        float remainingLevelSetupTime = allowedLevelSetupTime + .5f; // to account for start delays
        while (remainingLevelSetupTime > 0)
        {
            if (stopEarly) break;

            remainingLevelSetupTime -= Time.deltaTime;
            timerText.text = "" + Mathf.Clamp(Mathf.Ceil(remainingLevelSetupTime), 1, allowedLevelSetupTime);
            yield return null;
        }

        timerText.text = "GO";
        EventBus.PublishEvent(new RoundStartEvent());
        stopEarly = false;
    }

    IEnumerator StartRaceCountdownCoroutine()
    {
        float remainingRaceTime = raceTime + .5f;
        while (remainingRaceTime > 0)
        {
            if (stopEarly) break;

            remainingRaceTime -= Time.deltaTime;
            timerText.text = "" + Mathf.Clamp(Mathf.Ceil(remainingRaceTime), 1, raceTime);
            yield return null;
        }

        timerText.text = "OK";
        EventBus.PublishEvent(new RoundEndEvent(remainingRaceTime));
        stopEarly = false;
    }
}
