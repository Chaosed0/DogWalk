using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsMenu : MonoBehaviour {

    bool restartClicked = false;
    public Image fadePanel;
    public float fadeRate;

    public void Restart()
    {
        if (!restartClicked)
        {
            restartClicked = true;
            StartCoroutine(FadeToLevel());
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    // Copy and paste, bitches!
    IEnumerator FadeToLevel ()
    {

        SceneManager.LoadSceneAsync("EdsAndrewScene");

        while (fadePanel.color.a < 1)
        {
            Color col = fadePanel.color;
            col.a += fadeRate * Time.deltaTime;
            fadePanel.color = col;

            yield return null;
        }
    }
}
