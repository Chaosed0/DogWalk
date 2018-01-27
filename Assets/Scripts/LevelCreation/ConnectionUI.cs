using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConnectionUI : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
{
    float fadeRate = 1.25f;
    Image img;

    void Awake()
    {
        img = GetComponent<Image>();
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        Debug.Log("entered");
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        Debug.Log("exited");
    }

    [SubscribeGlobal]
    public void HandleMatchStart(MatchStartEvent e)
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
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
