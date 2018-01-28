using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerChargeHandler: MonoBehaviour
{
    PlayerInput playerInput;

    public float minScale;
    public float maxScale;
    public Transform player;
    public ParticleSystem particleSystem;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.gameObject.Subscribe<PlayerInput.StartedChargingEvent>((x) => OnStartedCharging());
        playerInput.gameObject.Subscribe<PlayerInput.StoppedChargingEvent>((x) => OnStoppedCharging());
        playerInput.gameObject.Subscribe<PlayerInput.ChargeChangedEvent>(OnChargeChanged);
    }

    void OnStartedCharging() {
        if (particleSystem != null)
            particleSystem.Play();
    }

    void OnStoppedCharging() {
        if (particleSystem != null)
            particleSystem.Stop();
        StartCoroutine(Deflate());
    }

    Vector3 refvel;
    void OnChargeChanged(PlayerInput.ChargeChangedEvent e)
    {
        Vector3 target = Vector3.Lerp(new Vector3(minScale, minScale, minScale), new Vector3(maxScale, maxScale, maxScale), e.charge);
        player.localScale = target;
    }

    IEnumerator Deflate()
    {
        Vector3 scale = player.localScale;
        float time = 1.0f;
        while (time >= 0.0f)
        {
            player.localScale = Vector3.Lerp(new Vector3(minScale, minScale, minScale), scale, time);
            time -= Time.deltaTime;
            yield return null;
        }
    }
}
