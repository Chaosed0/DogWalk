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
    public static int tendrilFee = 10;
    public Connection connection; // TODO: enable/disable connections based on click actions
    public bool activeConnection = false;
    public bool fadeStarted = false;
    public PathTendril tendril;
    public PathGraph graph;
    public Texture2D defaultCursor;
    public Texture2D addAxonCursor;
    public Texture2D removeAxonCursor;
    public Sprite dottedLineImage;
    public Sprite tendrilImage;

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
        dottedLineImage = Resources.Load<Sprite>("dotted_line");
        tendrilImage = Resources.Load<Sprite>("short1");

        cursorSize = new Vector2(defaultCursor.width, defaultCursor.height);
        img.color = Color.white;
        SetCursor(defaultCursor);
    }

    void Start()
    {
        tendril = connection.GetTendril();
        ConfigureImage();
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        img.color = hoverColor;
        if (!fadeStarted)
        {
            if (activeConnection)
            {
                SetCursor(removeAxonCursor);
            }
            else
            {
                SetCursor(addAxonCursor);
            }

            EventBus.PublishEvent(new ActiveSegmentHoveredEvent(this));
        }
    }

    void SetCursor(Texture2D image)
    {
        Cursor.SetCursor(image, cursorSize * .2f, CursorMode.ForceSoftware);
    }

    public void ConfigureImage()
    {
        if (activeConnection)
        {
            img.sprite = tendrilImage;
            img.type = Image.Type.Simple;
        }
        else
        {
            img.sprite = dottedLineImage;
            img.type = Image.Type.Tiled;
        }
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        img.color = Color.white;
        SetCursor(defaultCursor);
        if (!fadeStarted)
        {
            EventBus.PublishEvent(new NoSegmentsHoveredEvent(this));
        }
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        if (!fadeStarted)
        {
            if (!activeConnection)
            {
                MoneyManager.Instance.AddMoney(tendrilFee);
                SetCursor(removeAxonCursor);
                activeConnection = true;
                connection.ToggleConnection();
            }
            else if (MoneyManager.Instance.CanAfford(tendrilFee) && connection.ToggleConnection())
            {
                MoneyManager.Instance.RemoveMoney(tendrilFee);
                activeConnection = false;
                SetCursor(addAxonCursor);
            }

            ConfigureImage();
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

    /*
    [SubscribeGlobal]
    public void HandleTendrilToggled(ToggleTendrilEvent e)
    {
        if (e.tendril == this.tendril)
        {
            Debug.Log("HandleTendrilToggled");
            activeConnection = e.tendril.isTraversable;
        }
    }
    */
}

