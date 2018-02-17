using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayerIndicator : MonoBehaviour {

    public float scaleSpeeed = 10f;

    public Color[] defaultColors;
    public Color[] highlightColors;

    private int fontSizeMin = 80;
    private int fontSizeMax = 160;

    [Range(80, 160)]
    private float currentFontSize;

    private int currentPlayer = 0;

    Text text;

    void Awake()
    {
        text = GetComponent<Text>();
        HighlightText();
    }

    private void Update()
    {
        if (currentFontSize > fontSizeMin)
        {
            currentFontSize = currentFontSize - Mathf.Min(scaleSpeeed * Time.deltaTime, currentFontSize - fontSizeMin);
            text.fontSize = (int) currentFontSize;
            text.color = Color.Lerp(defaultColors[currentPlayer], highlightColors[currentPlayer], (currentFontSize - fontSizeMin) / (fontSizeMax - fontSizeMin));
        }
    }

    [SubscribeGlobal]
    public void HandleTogglePlayerEvent(ToggleCurrentPlayerEvent e)
    {
        text.text = "PLAYER " + e.currentPlayer;
        currentPlayer = e.currentPlayer - 1;
        HighlightText();
    }

    private void HighlightText()
    {
        text.fontSize = fontSizeMax;
        currentFontSize = fontSizeMax;
        text.color = highlightColors[currentPlayer];
    }
}
