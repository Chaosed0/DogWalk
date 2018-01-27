using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggableIcon : MonoBehaviour {

    Image img;
    Color originalColor;

    [SubscribeGlobal]
    public void HandleActiveSegmentHoveredEvent(ActiveSegmentHoveredEvent e)
    {
        img.color = Color.yellow;
    }

    [SubscribeGlobal]
    public void HandleActiveSegmentHoveredEvent (NoSegmentsHoveredEvent e)
    {
        img.color = originalColor;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("placin dat trap doe");
        }
    }

    void Awake()
    {
        img = GetComponent<Image>();
        originalColor = img.color;
    }
}
