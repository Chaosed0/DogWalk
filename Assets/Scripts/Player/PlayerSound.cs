using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSound : MonoBehaviour {

    public AudioSource axonRidingSource;
    public AudioSource chargeUpSource;
    public AudioSource axonSwitchingSource;

    private void Awake()
    {
        // Axon Riding Sounds
        this.gameObject.Subscribe<PlayerMovement.StartedMovingEvent>((x) => OnStartedMoving());
        this.gameObject.Subscribe<PlayerMovement.StoppedMovingEvent>((x) => OnStoppedMoving());

        // Charging Sounds
        this.gameObject.Subscribe<PlayerInput.StartedChargingEvent>((x) => OnStartedCharging());
        this.gameObject.Subscribe<PlayerInput.ChargeRecycledEvent>((x) => OnChargeRecycled());
        this.gameObject.Subscribe<PlayerInput.StoppedChargingEvent>((x) => OnStoppedCharging());

        // Axon Switching Sounds (Select Path)
        this.gameObject.Subscribe<Player.SelectPathEvent>((x) => OnSelectPath());
    }

    // Axon Riding Sounds
    void OnStartedMoving()
    {
        axonRidingSource.Play();
    }

    void OnStoppedMoving()
    {
        axonRidingSource.Stop();
    }

    // Charging Sounds
    void OnStartedCharging()
    {
        chargeUpSource.PlayOneShot(chargeUpSource.clip);
    }

    void OnChargeRecycled()
    {
        chargeUpSource.Stop();
        chargeUpSource.PlayOneShot(chargeUpSource.clip);
    }

    void OnStoppedCharging()
    {
        chargeUpSource.Stop();
    }

    void OnSelectPath()
    {
        axonSwitchingSource.PlayOneShot(axonSwitchingSource.clip);
    }
}
