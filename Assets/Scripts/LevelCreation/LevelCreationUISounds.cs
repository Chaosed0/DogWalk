using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreationUISounds : MonoBehaviour {

    public AudioSource addAxonSource;
    public AudioSource cutAxonSource;
    public AudioSource actionNotAllowedSource;

    private void Awake()
    {
        EventBus.Subscribe<ConnectionUI.AxonAddEvent>((x) => OnAddAxon());
        EventBus.Subscribe<ConnectionUI.AxonCutEvent>((x) => OnCutAxon());
        EventBus.Subscribe<ConnectionUI.ActionNotAllowedEvent>((x) => OnActionNotAllowed());
    }

    void OnAddAxon()
    {
        if (addAxonSource != null && addAxonSource.clip != null)
        {
            addAxonSource.PlayOneShot(addAxonSource.clip);
        }
    }

    void OnCutAxon()
    {
        if (cutAxonSource != null && cutAxonSource.clip != null)
        {
            cutAxonSource.PlayOneShot(cutAxonSource.clip);
        }
    }

    void OnActionNotAllowed()
    {
        if (actionNotAllowedSource != null && cutAxonSource.clip != null)
        {
            actionNotAllowedSource.PlayOneShot(actionNotAllowedSource.clip);
        }
    }
}
