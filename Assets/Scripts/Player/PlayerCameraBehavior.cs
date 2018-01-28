using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraBehavior : MonoBehaviour {
    public Transform lookAt;
    public Transform followPoint;
    public PlayerMovement playerMovement;
    
    Vector3 refvel;

    bool isHyped;

    void Awake()
    {
        this.enabled = false;
        EventBus.Subscribe<RoundActuallyStartEvent>((x) => this.enabled = true);
        EventBus.Subscribe<RoundEndEvent>((x) => this.enabled = false);

        playerMovement.gameObject.Subscribe<Player.GetHypedEvent>((x) => isHyped = true);
        playerMovement.gameObject.Subscribe<Player.GetHypedEvent>((x) => isHyped = false);
    }

    void Update()
    {
        float positionDamping = 0.1f;
        float rotationDamping = Time.deltaTime;
        if (playerMovement != null && !playerMovement.IsMoving())
        {
            rotationDamping *= 10.0f;
        }
        else
        {
            rotationDamping *= 2.0f;
        }

        if (isHyped)
        {
            rotationDamping *= 5.0f;
            positionDamping /= 3.0f;
        }

        transform.position = Vector3.SmoothDamp(transform.position, GetTargetPosition(), ref refvel, 0.1f);
        transform.rotation = Quaternion.Slerp(transform.rotation, GetTargetRotation(), rotationDamping);
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
