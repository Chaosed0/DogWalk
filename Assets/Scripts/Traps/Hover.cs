using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour {

    float defaultY;
    float offset;

    public float amplitude;
    public float rate;

    Vector3 originalPos;

    private void Start()
    {
        defaultY = transform.position.y;
        offset = Random.Range(0f, Mathf.PI);
        originalPos = transform.position;
    }
    
    void Update () {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, defaultY + amplitude * Mathf.Sin((Time.time + offset) * rate), pos.z);
	}
}
