using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(EventSubscriber))]
public class LevelConstructionElement : MonoBehaviour
{
    public float fadeRate = 1.25f;
    CanvasGroup canvasGroup;

    public struct LevelFadeStarted { }
    public struct LevelFadeEnded { }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    [SubscribeGlobal]
    public void HandleLevelCreated(RoundStartEvent e)
    {
        this.gameObject.PublishEvent(new LevelFadeStarted());
        StartCoroutine(FadeInOrOut(false));
    }

    [SubscribeGlobal]
    public void HandleLevelCreationStarted(LevelCreationStartEvent e)
    {
        this.gameObject.PublishEvent(new LevelFadeStarted());
        StartCoroutine(FadeInOrOut(true));
    }

    IEnumerator FadeInOrOut(bool fadeIn)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * fadeRate;
            canvasGroup.alpha = (fadeIn ? t : 1f - t);

            yield return null;
        }

        this.gameObject.PublishEvent(new LevelFadeEnded());
    }
}
