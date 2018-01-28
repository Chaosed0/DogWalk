using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

    public AudioSource downTempoMusic;
    public AudioSource upTempoMusic;

    private void Awake()
    {
        if (downTempoMusic != null && downTempoMusic.clip != null)
        {
            downTempoMusic.Play();
        }
    }

    [SubscribeGlobal]
    public void HandleRoundStart(RoundStartEvent e)
    {
        if (downTempoMusic != null && downTempoMusic.clip != null)
        {
            // Debug.Log("Stopping Downtempo");
            downTempoMusic.Stop();
        }

        if (upTempoMusic != null && upTempoMusic.clip != null)
        {
            if (!upTempoMusic.isPlaying)
            {
                // Debug.Log("Starting Uptempo");
                upTempoMusic.Play();
            }
        }
    }

    [SubscribeGlobal]
    public void HandleLevelCreationStart(LevelCreationStartEvent e)
    {
        if (upTempoMusic != null && upTempoMusic.clip != null)
        {
            // Debug.Log("Stopping Uptempo");
            upTempoMusic.Stop();
        }

        if (downTempoMusic != null && downTempoMusic.clip != null)
        {
            if (!downTempoMusic.isPlaying)
            {
                // Debug.Log("Starting Downtempo");
                downTempoMusic.Play();
            }
        }
    }

    [SubscribeGlobal]
    public void HandleDogSequenceStart(StartDogSequenceEvent e)
    {
        if (upTempoMusic != null && upTempoMusic.clip != null)
        {
            upTempoMusic.Stop();
        }

        if (downTempoMusic != null && downTempoMusic.clip != null)
        {
            upTempoMusic.Stop();
        }
    }
}
