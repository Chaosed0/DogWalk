using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neurotransmitters : MonoBehaviour {

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
        defaultX = transform.position.x;
        defaultY = transform.position.y;
        xOffset = Random.Range(0f, Mathf.PI);
        yOffset = Random.Range(0f, Mathf.PI);
    }

    void Update()
    {
        Vector3 pos = transform.position;
        float xPos = defaultY + xAmplitude * Mathf.Sin((Time.time + xOffset) * xRate);
        float yPos = defaultY + yAmplitude * Mathf.Sin((Time.time + yOffset) * yRate);
        transform.position = new Vector3(xPos, yPos, pos.z);
    }
}
