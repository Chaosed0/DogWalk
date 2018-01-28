using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    bool clicked = false;
    bool sceneLoaded = false;
    public Image image;
    public float fadeRate;

	void Update()
    {
        if (!clicked && Input.GetMouseButtonDown(0))
        {
            clicked = true;
            StartCoroutine(FadeToLevel());
        }
    }

    IEnumerator FadeToLevel ()
    {

        SceneManager.LoadSceneAsync("EdsAndrewScene");

        while (image.color.a < 1)
        {
            Color col = image.color;
            col.a += fadeRate * Time.deltaTime;
            image.color = col;

            yield return null;
        }
    }
}
