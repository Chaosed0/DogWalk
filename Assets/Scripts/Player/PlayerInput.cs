using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public float normalChargeTime = 1.0f;
    public float hypedChargeTime = 0.3f;
    public float confusedAutoPickTime = 1.0f;
    public float confusedAndHypedAutoPickTime = 0.5f;

    float chargeTime = 1.0f;
    float confusedTimer = 0.0f;
    float autoPickTime = 0.0f;

    Player player;
    PlayerMovement playerMovement;

    bool isConfused = false;
    bool isCharging = false;
    float currentCharge = 0.0f;

    public class StartedChargingEvent { }
    public class ChargeChangedEvent {
        public float charge;
        public ChargeChangedEvent(float charge) { this.charge = charge; }
    }
    public class StoppedChargingEvent { }

    void Awake()
    {
        autoPickTime = confusedAutoPickTime;
        player = GetComponent<Player>();
        playerMovement = GetComponent<PlayerMovement>();
        this.enabled = false;
        chargeTime = normalChargeTime;

        EventBus.Subscribe<RoundActuallyStartEvent>((x) => this.enabled = true);
        EventBus.Subscribe<RoundEndEvent>((x) => this.enabled = false);
    }

    void Update()
    {
        if (player.CanTraversePath())
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentCharge = 0.0f;
                isCharging = true;
                this.gameObject.PublishEvent(new StartedChargingEvent());
            }

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                isCharging = false;
                this.gameObject.PublishEvent(new StoppedChargingEvent());
                playerMovement.SetCharge(currentCharge);
                player.TraversePath();
            }
        }

        if (!playerMovement.IsMoving())
        {
            if (isConfused)
            {
                confusedTimer += Time.deltaTime;
                if (confusedTimer >= autoPickTime)
                {
                    player.SelectNextPath(true);
                    confusedTimer -= autoPickTime;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    player.SelectNextPath(false);
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    player.SelectNextPath(true);
                }
            }
        }

        if (isCharging)
        {
            currentCharge += Time.deltaTime / chargeTime;
            if (currentCharge >= 1.0f)
            {
                currentCharge -= 1.0f;
            }
            this.gameObject.PublishEvent(new ChargeChangedEvent(currentCharge));
        }
    }

    [Subscribe]
    void GetHyped(Player.GetHypedEvent e)
    {
        chargeTime = hypedChargeTime;
        autoPickTime = confusedAndHypedAutoPickTime;
    }

    [Subscribe]
    void StopHyped(Player.StopHypedEvent e)
    {
        chargeTime = normalChargeTime;
        autoPickTime = confusedAutoPickTime;
    }

    [Subscribe]
    void GetConfused(Player.GetConfusedEvent e)
    {
        isConfused = true;
    }

    [Subscribe]
    void StopConfused(Player.StopConfusedEvent e)
    {
        isConfused = false;
    }
}

