using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraBehavior : MonoBehaviour {
    public Transform lookAt;
    public Transform followPoint;
    
    Vector3 refvel;

    void Update()
    {
        Vector3 direction = lookAt.transform.position - followPoint.transform.position;
        transform.position = Vector3.SmoothDamp(transform.position, followPoint.transform.position, ref refvel, 0.1f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), Time.deltaTime * 10.0f);
    }
}
