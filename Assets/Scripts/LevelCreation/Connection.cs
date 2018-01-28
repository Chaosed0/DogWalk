using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PathTendril))]
public class Connection : MonoBehaviour {
    public GameObject node1;
    public GameObject node2;

    PathTendril tendril;
    PathGraph graph;
    Canvas levelCreationCanvas;
    Image tendrilImage;
    Vector3 pos1Canvas;
    Vector3 pos2Canvas;
    float length;

    void Awake()
    {
        tendril = GetComponent<PathTendril>();
        graph = GameObject.FindObjectOfType<PathGraph>();
    }

    void Start()
    {
        PathEdge edge = graph.tendrilToEdge[tendril];
        node1 = edge.node1.gameObject;
        node2 = edge.node2.gameObject;

        InitializeConnection();
    }

    void InitializeConnection()
    {
        levelCreationCanvas = GameObject.Find("LevelCreationUI").GetComponent<Canvas>();
        tendrilImage = Instantiate(Resources.Load("ConnectionImage") as GameObject).GetComponent<Image>();
        tendrilImage.rectTransform.SetParent(levelCreationCanvas.transform);
        tendrilImage.gameObject.GetComponent<ConnectionUI>().connection = this;

        Vector3 pos1 = Camera.main.WorldToViewportPoint(node1.transform.position);
        Vector3 pos2 = Camera.main.WorldToViewportPoint(node2.transform.position);

        pos1Canvas = new Vector2(pos1.x * levelCreationCanvas.pixelRect.width, pos1.y * levelCreationCanvas.pixelRect.height);

        float canvasDistX = (pos2.x - pos1.x) * levelCreationCanvas.pixelRect.width;
        float canvasDistY = (pos2.y - pos1.y) * levelCreationCanvas.pixelRect.height;

        length = Mathf.Sqrt(Mathf.Pow(canvasDistX, 2) + Mathf.Pow(canvasDistY, 2));
        float rads = Mathf.Atan2(canvasDistY, canvasDistX);

        tendrilImage.rectTransform.anchoredPosition = new Vector2((pos1.x - .5f) * levelCreationCanvas.pixelRect.width + length * Mathf.Cos(rads) / 2,
            (pos1.y - .5f) * levelCreationCanvas.pixelRect.height + length * Mathf.Sin(rads) / 2);
        tendrilImage.rectTransform.sizeDelta = new Vector2(length, tendrilImage.rectTransform.sizeDelta.y);
        tendrilImage.rectTransform.rotation = Quaternion.Euler(0, 0, rads * Mathf.Rad2Deg);
    }

    [SubscribeGlobal]
    public void HandleLevelCreationStart (LevelCreationStartEvent e)
    {
        StartCoroutine(Util.DeferForOneFrame(InitializeConnection));
    }

    public float GetInterpolationAmount()
    {
        Vector2 viewportMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        viewportMousePos.x *= levelCreationCanvas.pixelRect.width;
        viewportMousePos.y *= levelCreationCanvas.pixelRect.height;

        return Vector2.Distance(viewportMousePos, pos1Canvas) / length;
    }

    public bool ToggleConnection()
    {
        Debug.Log("tendril.isTraversable: " + tendril.isTraversable);
        bool canFinishIfToggled = graph.CanReachFinishIf(tendril, !tendril.isTraversable);
        Debug.Log("canFinishIfToggled: " + canFinishIfToggled);
        if (canFinishIfToggled)
        {
            tendril.TogglePath();
        }
        Debug.Log("tendril.isTraversable: " + tendril.isTraversable);
        return canFinishIfToggled;
    }

    public PathTendril GetTendril()
    {
        return tendril;
    }
}
