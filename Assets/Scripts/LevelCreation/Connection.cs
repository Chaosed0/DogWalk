using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PathTendril))]
public class Connection : MonoBehaviour {
    GameObject node1;
    GameObject node2;

    PathTendril tendril;
    PathGraph graph;
    Canvas levelCreationCanvas;
    Image testImage;

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
        testImage = Instantiate(Resources.Load("ConnectionImage") as GameObject).GetComponent<Image>();
        testImage.rectTransform.SetParent(levelCreationCanvas.transform);
        testImage.gameObject.GetComponent<ConnectionUI>().connection = this;

        Vector3 pos1 = Camera.main.WorldToViewportPoint(node1.transform.position);
        Vector3 pos2 = Camera.main.WorldToViewportPoint(node2.transform.position);

        float canvasDistX = (pos2.x - pos1.x) * levelCreationCanvas.pixelRect.width;
        float canvasDistY = (pos2.y - pos1.y) * levelCreationCanvas.pixelRect.height;

        float distance = Mathf.Sqrt(Mathf.Pow(canvasDistX, 2) + Mathf.Pow(canvasDistY, 2));
        float rads = Mathf.Atan2(canvasDistY, canvasDistX);

        testImage.rectTransform.anchoredPosition = new Vector2((pos1.x - .5f) * levelCreationCanvas.pixelRect.width + distance * Mathf.Cos(rads) / 2,
            (pos1.y - .5f) * levelCreationCanvas.pixelRect.height + distance * Mathf.Sin(rads) / 2);
        testImage.rectTransform.sizeDelta = new Vector2(distance, testImage.rectTransform.sizeDelta.y);
        testImage.rectTransform.rotation = Quaternion.Euler(0, 0, rads * Mathf.Rad2Deg);
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
        return tendril.isTraversable;
    }

    public PathTendril GetTendril()
    {
        return tendril;
    }
}
