﻿using System;
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
    public static int tendrilFee = 5;
    public Connection connection;
    public bool fadeStarted = false;
    public PathTendril tendril;
    public Texture2D defaultCursor;
    public Texture2D addAxonCursor;
    public Texture2D removeAxonCursor;
    public Sprite dottedLineImage;
    public Sprite tendrilImage;

    public bool hasTrap;

    Color hoverColor = new Color(1, .42f, .88f);
    Vector2 cursorSize;

    public struct AxonAddEvent { }
    public struct AxonCutEvent
    {
        public ConnectionUI connectionUI;

        public AxonCutEvent(ConnectionUI connectionUI)
        {
            this.connectionUI = connectionUI;
        }
    }
    public struct ActionNotAllowedEvent { }

    void Awake()
    {
        img = transform.GetChild(0).GetComponent<Image>();

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
        ConfigureImage();
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        if (!Cursor.visible)
        {
            return;
        }

        img.color = hoverColor;
        if (!fadeStarted)
        {
            // if (activeConnection)
            if (tendril.isTraversable)
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
        if (tendril.isTraversable)
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
        if (!Cursor.visible)
        {
            return;
        }
        img.color = Color.white;
        SetCursor(defaultCursor);
        if (!fadeStarted)
        {
            EventBus.PublishEvent(new NoSegmentsHoveredEvent(this));
        }
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        if (!Cursor.visible)
        {
            return;
        }

        if (!fadeStarted)
        {
            if (!tendril.isTraversable)
            {
                if (MoneyManager.Instance.CanAfford(tendrilFee))
                {
                    MoneyManager.Instance.RemoveMoney(tendrilFee);
                    SetCursor(removeAxonCursor);
                    connection.ToggleConnection();
                    EventBus.PublishEvent(new AxonAddEvent());
                }
            }
            else if (connection.ToggleConnection())
            {
                MoneyManager.Instance.AddMoney(tendrilFee);
                SetCursor(addAxonCursor);
                EventBus.PublishEvent(new AxonCutEvent(this));
            }
            else
            {
                EventBus.PublishEvent(new ActionNotAllowedEvent());
            }

            ConfigureImage();
        }
    }

    public Vector3 GetTrapPosition()
    {
        float t = connection.GetInterpolationAmount();
        PathGraph graph = FindObjectOfType<PathGraph>();
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
        GetComponent<CanvasGroup>().interactable = false;
        fadeStarted = false;
    }

    [SubscribeGlobal]
    public void HandleLevelCreationStartEvent(LevelCreationStartEvent e)
    {
        GetComponent<CanvasGroup>().interactable = true;
        hasTrap = false;
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

