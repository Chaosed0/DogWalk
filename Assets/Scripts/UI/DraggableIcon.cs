using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggableIcon : MonoBehaviour {

    Image img;
    Color originalColor;
    ConnectionUI currentConnectionUI;
    bool isMarker = false;
    float timeToShrink = .35f;

    public GameObject itemPrefab;

    [SubscribeGlobal]
    public void HandleActiveSegmentHoveredEvent(ActiveSegmentHoveredEvent e)
    {
        if (!isMarker)
        {
            currentConnectionUI = e.connectionUI;
            img.color = Color.yellow;
        }
    }

    [SubscribeGlobal]
    public void HandleActiveSegmentHoveredEvent (NoSegmentsHoveredEvent e)
    {
        if (!isMarker)
        {
            if (currentConnectionUI == e.connectionUI)
            {
                currentConnectionUI = null;
            }

            img.color = originalColor;
        }
    }

    [SubscribeGlobal]
    public void HandleRoundEndEvent (RoundEndEvent e)
    {
        Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !isMarker)
        {
            // if (currentConnectionUI && currentConnectionUI.activeConnection)
            if (currentConnectionUI && currentConnectionUI.tendril.isTraversable)
            {
                Vector3 trapPosition = currentConnectionUI.GetTrapPosition();
                Instantiate(itemPrefab, trapPosition, GetItemQuaternion());
                MoneyManager.Instance.RemoveMoney(itemPrefab.GetComponent<PurchasableObject>().cost);
                isMarker = true;
                StartCoroutine(ShrinkIcon());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator ShrinkIcon()
    {
        float t = 1;
        float perFrame = .5f / timeToShrink;
        while (t > .35f)
        {
            t = Mathf.Max(t - perFrame * Time.deltaTime, .35f);
            img.transform.localScale = new Vector2(t, t);
            yield return null;
        }
    }

    void Awake()
    {
        img = GetComponent<Image>();
        originalColor = img.color;
    }

    Quaternion GetItemQuaternion()
    {
        Vector3 node1Pos = currentConnectionUI.connection.node1.transform.position;
        Vector3 node2Pos = currentConnectionUI.connection.node2.transform.position;
        return Quaternion.LookRotation(node1Pos - node2Pos);
    }
}
