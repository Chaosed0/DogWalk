using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConnectionUI : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
    , IPointerClickHandler
{
    float fadeRate = 1.25f;
    Image img;

    public Connection connection; // TODO: enable/disable connections based on click actions
    public bool activeConnection = true;
    public bool fadeStarted = false;

    void Awake()
    {
        img = GetComponent<Image>();
        img.color = Color.green;
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        if (!fadeStarted)
        {
            img.color = Color.blue;
        }
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        if (!fadeStarted)
        {
            img.color = Color.green;
        }
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        if (!fadeStarted)
        {
            activeConnection = !activeConnection;
            Debug.Log("CLICKED!");
            // activate/deactivate the connection
        }
    }

    [SubscribeGlobal]
    public void HandleLevelCreated(RoundStartEvent e)
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        fadeStarted = true;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * fadeRate;
            Color col = img.color;
            col.a = 1f - t;
            img.color = col;

            yield return null;
        }

        Destroy(gameObject);
    }
}
