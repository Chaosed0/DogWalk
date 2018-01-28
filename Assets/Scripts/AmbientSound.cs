using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour {

    public AudioSource citySource;
    public AudioSource birdsSource;

    [SubscribeGlobal]
    public void HandleDogSequenceStart(StartDogSequenceEvent e)
    {
        if (citySource != null && citySource.clip != null)
        {
            citySource.Play();
        }

        if (birdsSource != null && birdsSource.clip != null)
        {
            birdsSource.Play();
        }
    }

    [SubscribeGlobal]
    public void HandleDogSequenceStop(EndDogSequenceEvent e)
    {
        if (citySource != null && citySource.clip != null)
        {
            citySource.Stop();
        }

        if (birdsSource != null && birdsSource.clip != null)
        {
            birdsSource.Stop();
        }
    }
}
