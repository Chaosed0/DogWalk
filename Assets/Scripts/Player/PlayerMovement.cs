using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PlayerMovement: MonoBehaviour
{
    Rigidbody rigidbody;

    bool isMoving = false;
    int currentSegment = -1;
    List<Vector3> currentPath;
    float currentSegmentDistance = 0.0f;

    float moveSpeed = 10.0f;

    public class StoppedMovingEvent { }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
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

        currentSegmentDistance += moveSpeed * Time.deltaTime;
        Vector3 nextPosition = GetNextPosition();
        rigidbody.MovePosition(nextPosition);

        if (nextPosition == currentPath[currentPath.Count - 1])
        {
            isMoving = false;
            this.gameObject.PublishEvent(new StoppedMovingEvent());
        }
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
