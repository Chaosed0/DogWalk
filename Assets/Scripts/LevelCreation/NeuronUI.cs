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

    [SubscribeGlobal]
    public void HandleRoundStartEvent(RoundActuallyStartEvent e)
    {
        NodeUI node = GetComponent<NodeUI>();
        node.ClearStartNode();
        node.ClearFinishNode();
    }
}

