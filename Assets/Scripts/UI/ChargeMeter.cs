using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class ChargeMeter: MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerMovement playerMovement;

    public Image sliderFill;
    public Color initialColor;
    public Color finalColor;

    Slider slider;
    CanvasGroup canvasGroup;

    void Awake()
    {
        slider = GetComponent<Slider>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;

        playerInput.gameObject.Subscribe<PlayerInput.StartedChargingEvent>((x) => canvasGroup.alpha = 1.0f);
        playerInput.gameObject.Subscribe<PlayerInput.StoppedChargingEvent>((x) => canvasGroup.alpha = 0.0f);
        playerInput.gameObject.Subscribe<PlayerInput.ChargeChangedEvent>(OnChargeChanged);
    }

    void OnChargeChanged(PlayerInput.ChargeChangedEvent e)
    {
        // hack, it seems laggy
        slider.value = e.charge;
        sliderFill.color = Color.Lerp(initialColor, finalColor, e.charge);

        if (e.charge >= playerMovement.burstChargeMin && e.charge <= playerMovement.burstChargeMax)
        {
            sliderFill.color = new Color(Random.Range(initialColor.r, finalColor.r),
                                        Random.Range(initialColor.g, finalColor.g),
                                        Random.Range(initialColor.b, finalColor.b));
        }
    }
}
