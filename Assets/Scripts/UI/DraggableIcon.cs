using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggableIcon : MonoBehaviour {

    Image img;
    Color originalColor;
    ConnectionUI currentConnectionUI;

    [SubscribeGlobal]
    public void HandleActiveSegmentHoveredEvent(ActiveSegmentHoveredEvent e)
    {
        currentConnectionUI = e.connectionUI;
        img.color = Color.yellow;
    }

    [SubscribeGlobal]
    public void HandleActiveSegmentHoveredEvent (NoSegmentsHoveredEvent e)
    {
        if (currentConnectionUI == e.connectionUI)
        {
            currentConnectionUI = null;
        }

        img.color = originalColor;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (currentConnectionUI)
            {
                Vector3 trapPosition = currentConnectionUI.GetTrapPosition();
                Instantiate(Resources.Load<GameObject>("TestTrap"), trapPosition, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

    void Awake()
    {
        img = GetComponent<Image>();
        originalColor = img.color;
    }
}
