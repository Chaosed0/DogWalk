using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableMenuIcon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    public GameObject itemDraggablePrefab;
    public GameObject itemPrefab;
    public CanvasGroup tooltip;

    Image imageIcon;
    Vector3 localPointerPosition;
    Canvas levelCreationCanvas;
    RectTransform levelCreationRectTransform;
    Image img;

    void Awake()
    {
        img = GetComponent<Image>();
        levelCreationCanvas = GameObject.Find("LevelToolUI").GetComponent<Canvas>();
        levelCreationRectTransform = levelCreationCanvas.GetComponent<RectTransform>();
        tooltip.alpha = 0.0f;
    }

    public void OnDrag (PointerEventData eventData)
    {
        GetPointerPosition(eventData);
        imageIcon.transform.position = localPointerPosition;
    }

    void GetPointerPosition(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(levelCreationRectTransform,
            eventData.position, eventData.pressEventCamera, out localPointerPosition);
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        GetPointerPosition(eventData);
        imageIcon = (Instantiate(itemDraggablePrefab, localPointerPosition, Quaternion.identity,
            levelCreationCanvas.transform)).GetComponent<Image>();
        imageIcon.GetComponent<DraggableIcon>().itemPrefab = itemPrefab;
        imageIcon.sprite = img.sprite;
        imageIcon.preserveAspect = true;
        imageIcon.transform.position = localPointerPosition;
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        img.color = Color.white * 0.7f;
        tooltip.alpha = 1.0f;
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        img.color = Color.white;
        tooltip.alpha = 0.0f;
    }

    public void OnPointerUp (PointerEventData eventData)
    {
    }
}
