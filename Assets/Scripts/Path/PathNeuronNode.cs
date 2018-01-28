using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathNeuronNode : MonoBehaviour {
    Canvas levelCreationCanvas;
    public Image nodeImage;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        DoGizmos();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        DoGizmos();
    }

    void DoGizmos()
    {
        Gizmos.DrawIcon(transform.position, "spawn-node.png", true);
    }

    void Start()
    {
        if (GameManager.Instance.currentLevel == 0)
        {
            InitializeUINode();
        }
    }

    [SubscribeGlobal]
    public void HandleLevelCreationStart (LevelCreationActuallyStartEvent e)
    {
        StartCoroutine(Util.DeferForOneFrame(InitializeUINode));
    }

    private void OnDestroy ()
    {
        if (nodeImage)
        {
            Destroy(nodeImage.gameObject);
        }
    }

    void InitializeUINode()
    {
        levelCreationCanvas = GameObject.Find("LevelCreationUI").GetComponent<Canvas>();

        if (!nodeImage)
        {
            nodeImage = Instantiate(Resources.Load("NeuronImage") as GameObject).GetComponent<Image>();
        }

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 posCanvas = new Vector2((pos.x - .5f) * levelCreationCanvas.pixelRect.width,
            (pos.y - .5f) * levelCreationCanvas.pixelRect.height);

        nodeImage.rectTransform.SetParent(GameObject.Find("NeuronContainer").transform);
        nodeImage.rectTransform.anchoredPosition = posCanvas;

        ConfigureNode(FindObjectOfType<PathGraph>());
    }

    [SubscribeGlobal]
    void HandleGraphConfiguredEvent(GraphConfiguredEvent e)
    {
        if (nodeImage == null)
        {
            return;
        }
        ConfigureNode(e.graph);
    }

    void ConfigureNode(PathGraph graph)
    {
        NodeUI nodeUi = nodeImage.GetComponent<NodeUI>();
        if (graph.startNode == this)
        {
            nodeUi.SetStartNode();
            nodeUi.ClearFinishNode();
        }
        else if (graph.finishNode == this)
        {
            nodeUi.SetFinishNode();
            nodeUi.ClearStartNode();
        }
        else
        {
            nodeUi.ClearStartNode();
            nodeUi.ClearFinishNode();
        }
    }
}
