using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraBehavior : MonoBehaviour {
    public Transform lookAt;
    public Transform followPoint;
    
    Vector3 refvel;

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, GetTargetPosition(), ref refvel, 0.1f);
        transform.rotation = Quaternion.Slerp(transform.rotation, GetTargetRotation(), Time.deltaTime * 10.0f);
    }

    public Vector3 GetTargetPosition()
    {
        return followPoint.transform.position;
    }

    public Quaternion GetTargetRotation()
    {
        Vector3 direction = lookAt.transform.position - followPoint.transform.position;
        return Quaternion.LookRotation(direction.normalized);
    }
}
