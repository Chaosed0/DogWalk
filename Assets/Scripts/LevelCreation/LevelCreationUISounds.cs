using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreationUISounds : MonoBehaviour {

    public AudioSource addAxonSource;
    public AudioSource cutAxonSource;

    private void Awake()
    {
        EventBus.Subscribe<ConnectionUI.AxonAddEvent>((x) => OnAddAxon());
        EventBus.Subscribe<ConnectionUI.AxonCutEvent>((x) => OnCutAxon());
    }

    void OnAddAxon()
    {
        addAxonSource.PlayOneShot(addAxonSource.clip);
    }

    void OnCutAxon()
    {
        cutAxonSource.PlayOneShot(cutAxonSource.clip);
    }
}
