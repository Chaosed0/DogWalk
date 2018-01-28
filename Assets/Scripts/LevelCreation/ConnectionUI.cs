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
    Image img;

    public Connection connection; // TODO: enable/disable connections based on click actions
    public bool activeConnection = true;
    public bool fadeStarted = false;
    public PathTendril tendril;
    public PathGraph graph;
    public Texture2D defaultCursor;
    public Texture2D addAxonCursor;
    public Texture2D removeAxonCursor;

    Color hoverColor = new Color(1, .42f, .88f);
    Vector2 cursorSize;

    void Awake()
    {
        img = transform.GetChild(0).GetComponent<Image>();
        graph = GameObject.Find("Graph").GetComponent<PathGraph>();

        // inefficient as fuck but whatever
        defaultCursor = Resources.Load<Texture2D>("cursor_regular");
        addAxonCursor = Resources.Load<Texture2D>("cursor_add_syringe");
        removeAxonCursor = Resources.Load<Texture2D>("cursor_remove_scissors");

        cursorSize = new Vector2(defaultCursor.width, defaultCursor.height);
        img.color = Color.white;
        SetCursor(defaultCursor);
    }

    void Start()
    {
        tendril = connection.GetTendril();
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        img.color = hoverColor;
        if (!fadeStarted)
        {
            if (activeConnection)
            {
                SetCursor(addAxonCursor);
            }
            else
            {
                SetCursor(removeAxonCursor);
            }

            EventBus.PublishEvent(new ActiveSegmentHoveredEvent(this));
        }
    }

    void SetCursor(Texture2D image)
    {
        Cursor.SetCursor(image, cursorSize * .2f, CursorMode.ForceSoftware);
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        img.color = Color.white;
        SetCursor(defaultCursor);
        if (!fadeStarted)
        {
            //img.color = Color.green;
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

    [Subscribe]
    public void HandleFadeStart(LevelConstructionElement.LevelFadeStarted e)
    {
        fadeStarted = true;
    }

    [Subscribe]
    public void HandleFadeEnd(LevelConstructionElement.LevelFadeEnded e)
    {
        Destroy(this.gameObject);
    }
}

