using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDisplayController: MonoBehaviour
{
    public float minScale;
    public float maxScale;
    public Transform player;
    public ParticleSystem shocks;
    public ParticleSystem charge;

    public Color hypedColor;

    ParticleSystem.MinMaxGradient shocksStartColor;

    void Awake()
    {
        shocksStartColor = shocks.main.startColor;

        this.gameObject.Subscribe<PlayerInput.StartedChargingEvent>((x) => OnStartedCharging());
        this.gameObject.Subscribe<PlayerInput.StoppedChargingEvent>((x) => OnStoppedCharging());
        this.gameObject.Subscribe<PlayerInput.ChargeChangedEvent>(OnChargeChanged);
    }

    [Subscribe]
    void GetHyped(Player.GetHypedEvent e)
    {
        var main = shocks.main;
        main.startColor = hypedColor;
    }

    [Subscribe]
    void StopHyped(Player.StopHypedEvent e)
    {
        var main = shocks.main;
        main.startColor = shocksStartColor;
    }

    void OnStartedCharging() {
        if (charge != null)
            charge.Play();
    }

    void OnStoppedCharging() {
        if (charge != null)
            charge.Stop();
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
