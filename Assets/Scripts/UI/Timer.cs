using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float allowedLevelSetupTime;
    public float allowedLevelSetupTime_second;
    public float raceTime;

    int times = 0;

    Text timerText;
    bool stopEarly = false;

    Coroutine raceCountdown;

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
        raceCountdown = StartCoroutine(StartRaceCountdownCoroutine());
    }

    [SubscribeGlobal]
    void HandlePlayerFinishedMap(PlayerFinishedMapEvent e)
    {
        stopEarly = true;
    }

    public void FinishLevelSetupEarly()
    {
        stopEarly = true;
    }
	
	IEnumerator StartLevelSetupCountdownCoroutine()
    {
        float setupTime;
        float remainingLevelSetupTime; // to account for start delays
        if (times <= 1)
        {
            setupTime = allowedLevelSetupTime;
        }
        else
        {
            setupTime = allowedLevelSetupTime_second;
        }
        remainingLevelSetupTime = setupTime + 0.5f;

        while (remainingLevelSetupTime > 0)
        {
            if (stopEarly) break;

            remainingLevelSetupTime -= Time.deltaTime;
            timerText.text = "" + Mathf.Clamp(Mathf.Ceil(remainingLevelSetupTime), 1, setupTime);
            yield return null;
        }

        timerText.text = "GO";
        EventBus.PublishEvent(new RoundStartEvent());
        stopEarly = false;
        ++times;
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
