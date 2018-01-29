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
    public class ChargeRecycledEvent { }

    bool doUpdate = false;

    void Awake()
    {
        autoPickTime = confusedAutoPickTime;
        player = GetComponent<Player>();
        playerMovement = GetComponent<PlayerMovement>();
        chargeTime = normalChargeTime;

        EventBus.Subscribe<RoundActuallyStartEvent>((x) => doUpdate = true);
        EventBus.Subscribe<RoundEndEvent>((x) => doUpdate = false);
    }

    void Update()
    {
        if (!doUpdate)
            return;

        if (player.CanTraversePath())
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                currentCharge = 0.0f;
                isCharging = true;
                this.gameObject.PublishEvent(new StartedChargingEvent());
            }

            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
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
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    player.SelectNextPath(false);
                }

                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    player.SelectNextPath(true);
                }
            }
        }

        if (isCharging)
        {
            float previousChage = currentCharge;
            currentCharge += Time.deltaTime / chargeTime;
            if (currentCharge >= 1.0f)
            {
                currentCharge -= 1.0f;
            }
            if (currentCharge < previousChage)
            {
                this.gameObject.PublishEvent(new ChargeRecycledEvent());
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

