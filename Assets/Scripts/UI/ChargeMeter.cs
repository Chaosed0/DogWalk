using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class ChargeMeter: MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerMovement playerMovement;

    Slider slider;
    CanvasGroup canvasGroup;

    void Awake()
    {
        slider = GetComponent<Slider>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;

        playerInput.gameObject.Subscribe<PlayerInput.StartedChargingEvent>((x) => canvasGroup.alpha = 1.0f);
        playerInput.gameObject.Subscribe<PlayerInput.StoppedChargingEvent>((x) => canvasGroup.alpha = 0.0f);
        playerInput.gameObject.Subscribe<PlayerInput.ChargeChangedEvent>((x) => slider.value = x.charge);
    }
}
