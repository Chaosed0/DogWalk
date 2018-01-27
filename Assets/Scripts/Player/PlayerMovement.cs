using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement: MonoBehaviour
{
    new Rigidbody rigidbody;

    public float minMoveSpeed = 3.0f;
    public float maxMoveSpeed = 10.0f;
    public float burstMoveSpeed = 20.0f;

    public float burstChargeMin = 0.93f;
    public float burstChargeMax = 1.0f;

    bool isMoving = false;
    int currentSegment = -1;
    List<Vector3> currentPath;
    Vector3 currentFacing;
    float currentSegmentDistance = 0.0f;

    float currentMoveSpeed = 10.0f;

    public class StoppedMovingEvent { }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void SetCharge(float charge)
    {
        currentMoveSpeed = Mathf.Lerp(minMoveSpeed, maxMoveSpeed, charge);
        if (charge >= burstChargeMin && charge <= burstChargeMax)
        {
            currentMoveSpeed = burstMoveSpeed;
        }
    }

    public void StartFollowingPath(List<Vector3> path)
    {
        currentSegment = 0;
        currentPath = path;
        currentSegmentDistance = 0.0f;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        currentSegmentDistance += currentMoveSpeed * Time.deltaTime;
        Vector3 nextPosition = GetNextPosition();
        SetPosition(nextPosition);
        SetFacing(currentFacing);

        if (nextPosition == currentPath[currentPath.Count - 1])
        {
            isMoving = false;
            this.gameObject.PublishEvent(new StoppedMovingEvent());
        }
    }

    public void SetPosition(Vector3 position)
    {
        rigidbody.MovePosition(position);
    }

    public void SetFacing(Vector3 facing)
    {
        rigidbody.MoveRotation(Quaternion.LookRotation(facing));
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    Vector3 GetNextPosition()
    {
        if (currentSegment >= currentPath.Count - 1)
        {
            return currentPath[currentPath.Count - 1];
        }

        Vector3 direction = currentPath[currentSegment+1] - currentPath[currentSegment];
        float segmentDistance = direction.magnitude;
        direction = direction.normalized;
        this.currentFacing = direction;

        if (currentSegmentDistance > segmentDistance)
        {
            currentSegmentDistance -= segmentDistance;
            currentSegment++;
            return GetNextPosition();
        }

        return currentPath[currentSegment] + direction * currentSegmentDistance;
    }

    Vector3 GetCurrentFacing()
    {
        return currentPath[currentSegment+1] - currentPath[currentSegment];
    }
}
