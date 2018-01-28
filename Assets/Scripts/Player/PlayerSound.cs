using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSound : MonoBehaviour {

    public AudioSource axonRidingSource;
    public AudioSource chargeUpSource;
    public AudioSource axonSwitchingSource;
    public AudioSource adrenalineSource;
    public AudioSource chargeBonusSource;
    public AudioSource axonDegradedSource;

    private void Awake()
    {
        // Axon Riding & Axon Degraded events
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

        // Charge Bonus events
        this.gameObject.Subscribe<PlayerMovement.ChargeBonusEvent>((x) => OnChargeBonus());
    }

    // Axon Riding & Axon Degraded Sounds
    void OnStartedMoving()
    {
        if (GetComponent<PlayerMovement>().isOnSlowPath)
        {
            if (axonRidingSource != null && axonRidingSource.clip != null)
            {
                axonRidingSource.Stop();
            }

            if (axonDegradedSource != null && axonDegradedSource.clip != null)
            {
                axonDegradedSource.Play();
            }
        }
        else
        {
            if (axonDegradedSource != null && axonDegradedSource.clip != null)
            {
                axonDegradedSource.Stop();
            }

            if (axonRidingSource != null && axonRidingSource.clip != null)
            {
                axonRidingSource.Play();
            }
        }
    }

    void OnStoppedMoving()
    {
        if (axonRidingSource != null && axonRidingSource.clip != null)
        {
            axonRidingSource.Stop();
        }

        if (axonDegradedSource != null && axonDegradedSource.clip != null)
        {
            axonDegradedSource.Stop();
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

    // Charge Bonus Sounds
    void OnChargeBonus()
    {
        if (chargeBonusSource != null && chargeBonusSource.clip != null)
        {
            chargeBonusSource.PlayOneShot(chargeBonusSource.clip);
        }
    }
}
