using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {

    public Vector3 spinAxis = Vector3.up;
    public float spinSpeed = 10.0f;
    
    void Update () {
        transform.rotation *= Quaternion.AngleAxis(spinSpeed, spinAxis);
	}
}
