using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connection : MonoBehaviour {
    public GameObject node1;
    public GameObject node2;

    Canvas levelCreationCanvas;
    Image testImage;

    void Start()
    {
        InitializeConnection();
    }

    void InitializeConnection()
    {
        levelCreationCanvas = GameObject.Find("LevelCreationUI").GetComponent<Canvas>();
        testImage = Instantiate(Resources.Load("ConnectionImage") as GameObject).GetComponent<Image>();
        testImage.rectTransform.SetParent(levelCreationCanvas.transform);

        Vector3 pos1 = Camera.main.WorldToViewportPoint(node1.transform.position);
        Vector3 pos2 = Camera.main.WorldToViewportPoint(node2.transform.position);

        float canvasDistX = (pos2.x - pos1.x) * levelCreationCanvas.pixelRect.width;
        float canvasDistY = (pos2.y - pos1.y) * levelCreationCanvas.pixelRect.height;

        float distance = Mathf.Sqrt(Mathf.Pow(canvasDistX, 2) + Mathf.Pow(canvasDistY, 2));
        float rads = Mathf.Atan2(canvasDistY, canvasDistX);

        testImage.rectTransform.anchoredPosition = new Vector2((pos1.x - .5f) * levelCreationCanvas.pixelRect.width + distance * Mathf.Cos(rads) / 2,
            (pos1.y - .5f) * levelCreationCanvas.pixelRect.height + distance * Mathf.Sin(rads) / 2);
        testImage.rectTransform.sizeDelta = new Vector2(distance, testImage.rectTransform.sizeDelta.y);
        testImage.rectTransform.rotation = Quaternion.Euler(0, 0, rads * Mathf.Rad2Deg);
    }
}
