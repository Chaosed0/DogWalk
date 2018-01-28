using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSound : MonoBehaviour {

    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        this.gameObject.Subscribe<PlayerMovement.StartedMovingEvent>((x) => OnStartedMoving());
        this.gameObject.Subscribe<PlayerMovement.StoppedMovingEvent>((x) => OnStoppedMoving());
    }

    void OnStartedMoving()
    {
        source.Play();
    }

    void OnStoppedMoving()
    {
        source.Stop();
    }
}
