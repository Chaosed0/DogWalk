using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectionUI : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
{
    public void OnPointerEnter (PointerEventData eventData)
    {
        Debug.Log("entered");
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        Debug.Log("exited");
    }

}
