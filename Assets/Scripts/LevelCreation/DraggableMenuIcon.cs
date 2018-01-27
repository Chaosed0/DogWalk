﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableMenuIcon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    public GameObject itemPrefab;

    Image imageIcon;
    Vector3 localPointerPosition;
    Canvas levelCreationCanvas;
    RectTransform levelCreationRectTransform;

    void Awake()
    {
        levelCreationCanvas = GameObject.Find("LevelCreationUI").GetComponent<Canvas>();
        levelCreationRectTransform = levelCreationCanvas.GetComponent<RectTransform>();
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
        imageIcon = (Instantiate(itemPrefab, localPointerPosition, Quaternion.identity,
            levelCreationCanvas.transform)).GetComponent<Image>();
        imageIcon.transform.position = localPointerPosition;
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
    }

    public void OnPointerExit (PointerEventData eventData)
    {
    }

    public void OnPointerUp (PointerEventData eventData)
    {
        Destroy(imageIcon.gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}