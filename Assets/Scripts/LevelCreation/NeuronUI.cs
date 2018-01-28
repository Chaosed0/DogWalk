using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NeuronUI : MonoBehaviour
{
    [Subscribe]
    public void HandleFadeEnd(LevelConstructionElement.LevelFadeEnded e)
    {
        Destroy(this.gameObject);
    }
}

