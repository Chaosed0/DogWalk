using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSoundOneShot : MonoBehaviour {

    public AudioSource trapSource;

    private void Awake()
    {
        this.gameObject.Subscribe<Trap.PlayerRanIntoTrapEvent>((x) => OnRanIntoTrap());
    }

    void OnRanIntoTrap()
    {
        if (trapSource != null && trapSource.clip != null)
        {
            trapSource.PlayOneShot(trapSource.clip);
        }
    }
}
