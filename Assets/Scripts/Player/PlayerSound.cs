using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSound : MonoBehaviour {

    public AudioSource axonRidingSource;
    public AudioSource chargeUpSource;
    public AudioSource axonSwitchingSource;
    public AudioSource adrenalineSource;

    private void Awake()
    {
        // Axon Riding events
        this.gameObject.Subscribe<PlayerMovement.StartedMovingEvent>((x) => OnStartedMoving());
        this.gameObject.Subscribe<PlayerMovement.StoppedMovingEvent>((x) => OnStoppedMoving());

        // Charging events
        this.gameObject.Subscribe<PlayerInput.StartedChargingEvent>((x) => OnStartedCharging());
        this.gameObject.Subscribe<PlayerInput.ChargeRecycledEvent>((x) => OnChargeRecycled());
        this.gameObject.Subscribe<PlayerInput.StoppedChargingEvent>((x) => OnStoppedCharging());

        // Axon Switching (Select Path) events
        this.gameObject.Subscribe<Player.SelectPathEvent>((x) => OnSelectPath());

        // Adrenaline events
        this.gameObject.Subscribe<Player.GetHypedEvent>((x) => OnGetHyped());
        this.gameObject.Subscribe<Player.StopHypedEvent>((x) => OnStopHyped());
    }

    // Axon Riding Sounds
    void OnStartedMoving()
    {
        if (axonRidingSource != null && axonRidingSource.clip != null)
        {
            axonRidingSource.Play();
        }
    }

    void OnStoppedMoving()
    {
        if (axonRidingSource != null && axonRidingSource.clip != null)
        {
            axonRidingSource.Stop();
        }
    }

    // Charging Sounds
    void OnStartedCharging()
    {
        if (chargeUpSource != null && chargeUpSource.clip != null)
        {
            chargeUpSource.PlayOneShot(chargeUpSource.clip);
        }
    }

    void OnChargeRecycled()
    {
        if (chargeUpSource != null && chargeUpSource.clip != null)
        {
            chargeUpSource.Stop();
            chargeUpSource.PlayOneShot(chargeUpSource.clip);
        }
    }

    void OnStoppedCharging()
    {
        if (chargeUpSource != null && chargeUpSource.clip != null)
        {
            chargeUpSource.Stop();
        }
    }

    // Axon Switching Sounds (Select Path)
    void OnSelectPath()
    {
        if (axonSwitchingSource != null && axonSwitchingSource.clip != null)
        {
            axonSwitchingSource.PlayOneShot(axonSwitchingSource.clip);
        }
    }

    // Adrenaline Sounds
    void OnGetHyped()
    {
        if (adrenalineSource != null && adrenalineSource.clip != null)
        {
            adrenalineSource.Play();
        }
    }

    void OnStopHyped()
    {
        if (adrenalineSource != null && adrenalineSource.clip != null)
        {
            adrenalineSource.Stop();
        }
    }
}
