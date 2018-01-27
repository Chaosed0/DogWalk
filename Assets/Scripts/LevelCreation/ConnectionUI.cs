using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConnectionUI : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
    , IPointerClickHandler
{
    float fadeRate = 1.25f;
    Image img;

    public Connection connection; // TODO: enable/disable connections based on click actions
    public bool activeConnection = true;
    public bool fadeStarted = false;
    public PathTendril tendril;
    public PathGraph graph;

    void Awake()
    {
        img = GetComponent<Image>();
        img.color = Color.green;
        graph = GameObject.Find("Graph").GetComponent<PathGraph>();
    }

    void Start()
    {
        tendril = connection.GetTendril();
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        if (!fadeStarted)
        {
            img.color = Color.blue;
            EventBus.PublishEvent(new ActiveSegmentHoveredEvent(this));
        }
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        if (!fadeStarted)
        {
            img.color = Color.green;
            EventBus.PublishEvent(new NoSegmentsHoveredEvent(this));
        }
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        if (!fadeStarted)
        {
            activeConnection = !activeConnection;
            connection.ToggleConnection();
        }
    }

    public Vector3 GetTrapPosition()
    {
        float t = connection.GetInterpolationAmount();
        PathEdge edge = graph.tendrilToEdge[tendril];
        PathNeuronNode start = connection.node1.GetComponent<PathNeuronNode>();

        return edge.GetPointAlongPath(start, t);
    }

    [SubscribeGlobal]
    public void HandleLevelCreated(RoundStartEvent e)
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        fadeStarted = true;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * fadeRate;
            Color col = img.color;
            col.a = 1f - t;
            img.color = col;

            yield return null;
        }

        Destroy(gameObject);
    }
}
