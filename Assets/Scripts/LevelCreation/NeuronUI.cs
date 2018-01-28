using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NeuronUI : MonoBehaviour
{
    [Subscribe]
    public void HandleFadeEnd (LevelConstructionElement.LevelFadeEnded e)
    {
        GetComponent<CanvasGroup>().interactable = false;
    }

    [SubscribeGlobal]
    public void HandleLevelCreationStartEvent (LevelCreationStartEvent e)
    {
        GetComponent<CanvasGroup>().interactable = true;
    }
}

