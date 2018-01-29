using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class PathCutError : MonoBehaviour
{
    Text text;
    CanvasGroup canvasGroup;

    Coroutine currentCoroutine;

    void Awake()
    {
        text = GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
    }

    [SubscribeGlobal]
    void OnActionNotAllowed(ConnectionUI.ActionNotAllowedEvent e)
    {
        text.text = "ERROR: Breaks path to finish";
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(FlashMessage());
    }

    [SubscribeGlobal]
    void OnTrapAlreadyExists(DraggableIcon.TrapAlreadyExistsHereEvent e)
    {
        text.text = "ERROR: Only one trap per axon";
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(FlashMessage());
    }

    IEnumerator FlashMessage()
    {
        canvasGroup.alpha = 1.0f;
        yield return new WaitForSeconds(0.5f);
        float timer = 1.0f;
        while (timer >= 0.0f)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, timer);
            timer -= Time.deltaTime;
            yield return null;
        }
    }
}
