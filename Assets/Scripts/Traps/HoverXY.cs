using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverXY : MonoBehaviour {

    float defaultX;
    float defaultY;
    float xOffset;
    float yOffset;

    public float xAmplitude;
    public float yAmplitude;
    public float xRate;
    public float yRate;

    private void Start()
    {
        defaultX = transform.localPosition.x;
        defaultY = transform.localPosition.y;
        xOffset = Random.Range(0f, Mathf.PI);
        yOffset = Random.Range(0f, Mathf.PI);
    }

    void Update()
    {
        Vector3 pos = transform.localPosition;
        float xPos = defaultY + xAmplitude * Mathf.Sin((Time.time + xOffset) * xRate);
        float yPos = defaultY + yAmplitude * Mathf.Sin((Time.time + yOffset) * yRate);
        transform.localPosition = new Vector3(xPos, yPos, pos.z);
    }
}
