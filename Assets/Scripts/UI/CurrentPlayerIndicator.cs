using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayerIndicator : MonoBehaviour {

    Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }

	[SubscribeGlobal]
    public void HandleTogglePlayerEvent(ToggleCurrentPlayerEvent e)
    {
        text.text = "Current Player: " + e.currentPlayer;
    }
}
