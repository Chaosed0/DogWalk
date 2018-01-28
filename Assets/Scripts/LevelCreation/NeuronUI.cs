using UnityEngine;
public class NeuronUI : MonoBehaviour
{
    [Subscribe]
    public void HandleFadeEnd (LevelConstructionElement.LevelFadeEnded e)
    {
        GetComponent<CanvasGroup>().interactable = false;
    }

    [SubscribeGlobal]
    public void HandleLevelCreationStartEvent (LevelCreationStartEvent e)
    {
        GetComponent<CanvasGroup>().interactable = true;
    }
}

