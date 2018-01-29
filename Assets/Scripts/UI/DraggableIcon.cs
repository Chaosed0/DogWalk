using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggableIcon : MonoBehaviour {

    Image img;
    Color originalColor;
    ConnectionUI currentConnectionUI;
    bool isMarker = false;
    float timeToShrink = .25f;

    public GameObject itemPrefab;

    public struct TrapAlreadyExistsHereEvent { }

    [SubscribeGlobal]
    public void HandleActiveSegmentHoveredEvent(ActiveSegmentHoveredEvent e)
    {
        if (!isMarker && CanPlaceAtConnection(e.connectionUI))
        {
            currentConnectionUI = e.connectionUI;
            img.color = Color.yellow;
        }
    }

    [SubscribeGlobal]
    public void HandleNoSegmentHoveredEvent (NoSegmentsHoveredEvent e)
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

    bool CanPlaceAtConnection(ConnectionUI connectionUI) {
        return connectionUI &&
            !connectionUI.hasTrap &&
            connectionUI.tendril.isTraversable &&
            MoneyManager.Instance.CanAfford(itemPrefab.GetComponent<PurchasableObject>().cost);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !isMarker)
        {
            if (currentConnectionUI && currentConnectionUI.hasTrap)
            {
                EventBus.PublishEvent(new TrapAlreadyExistsHereEvent());
            }

            // if (currentConnectionUI && currentConnectionUI.activeConnection)
            if (CanPlaceAtConnection(currentConnectionUI))
            {
                currentConnectionUI.hasTrap = true;
                Vector3 trapPosition = currentConnectionUI.GetTrapPosition();
                GameObject trap = Instantiate(itemPrefab, trapPosition, GetItemQuaternion());
                trap.GetComponent<Trap>().SetTendril(currentConnectionUI.tendril);
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
