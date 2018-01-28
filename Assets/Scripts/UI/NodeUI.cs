﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeUI : MonoBehaviour
{

    public float movementAmount;
    public float secondsPerCycle;

    Vector2 initialPos;
    float roundStart;
    float randomDirX;
    float randomDirY;
    float randomAmountX;
    float randomAmountY;

    void Start ()
    {
        initialPos = transform.position;
        roundStart = Time.time + Random.Range(0, Mathf.PI * 2);
        randomDirX = Random.Range(0, 1) == 0 ? 1 : -1;
        randomDirY = Random.Range(0, 1) == 0 ? 1 : -1;
        randomAmountX = Random.Range(Mathf.PI / 2, Mathf.PI);
        randomAmountY = Random.Range(Mathf.PI / 2, Mathf.PI);
    }

    void Update ()
    {
        float amtX = (movementAmount * Mathf.Sin(randomAmountX * ((Time.time - roundStart) / secondsPerCycle)));
        float amtY = (movementAmount * Mathf.Sin(randomAmountY * ((Time.time - roundStart) / secondsPerCycle)));
        transform.position = new Vector2(initialPos.x + amtX * randomDirX, initialPos.y + amtY * randomDirY);
    }

    public void UpdateInitialPos (float newX)
    {
        initialPos = new Vector2(newX, initialPos.y);
    }
}