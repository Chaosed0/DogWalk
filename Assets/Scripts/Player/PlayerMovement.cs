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

    public float hypedSpeedFactor = 2.0f;

    public LayerMask trapLayer;

    bool isMoving = false;
    bool moveForward = true;
    int currentSegment = -1;
    List<Vector3> currentPath;
    Vector3 currentFacing;
    float currentSegmentDistance = 0.0f;

    float currentMoveSpeed = 10.0f;
    float currentSpeedFactor = 1.0f;

    public class StoppedMovingEvent { }
    public class ReversedMovementEvent { }

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
        currentMoveSpeed *= currentSpeedFactor;
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
        
        currentSegmentDistance += moveForward ? currentMoveSpeed * Time.deltaTime : -currentMoveSpeed * Time.deltaTime;
        Vector3 nextPosition = moveForward ? GetNextPosition() : GetPreviousPosition();
        SetPosition(nextPosition);
        SetFacing(currentFacing);
        if (moveForward && nextPosition == currentPath[currentPath.Count - 1] || !moveForward && nextPosition == currentPath[0])
        {
            this.gameObject.PublishEvent(new StoppedMovingEvent());
            moveForward = true;
            isMoving = false;
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

    Vector3 GetPreviousPosition()
    {
        if (currentSegment <= 0)
        {
            return currentPath[0];
        }

        Vector3 direction = currentPath[currentSegment + 1] - currentPath[currentSegment];
        float segmentDistance = direction.magnitude;
        direction = direction.normalized;
        this.currentFacing = direction;
        
        if (currentSegmentDistance < 0)
        {
            currentSegmentDistance += segmentDistance;
            currentSegment--;
            return GetPreviousPosition();
        }

        return currentPath[currentSegment] + direction * currentSegmentDistance;
    }

    Vector3 GetCurrentFacing()
    {
        return currentPath[currentSegment+1] - currentPath[currentSegment];
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer) == trapLayer.value)
        {
            Debug.Log("Hit Trap!");
            Trap trap = other.GetComponentInParent<Trap>();
            trap.ApplyAffectsToPlayer(this.gameObject);
        }
    }

    public void ReverseMovement()
    {
        this.gameObject.PublishEvent(new PlayerMovement.ReversedMovementEvent());
        moveForward = false;
    }

    [Subscribe]
    void GetHyped(Player.GetHypedEvent e)
    {
        currentSpeedFactor = hypedSpeedFactor;
    }

    [Subscribe]
    void StopHyped(Player.StopHypedEvent e)
    {
        currentSpeedFactor = 1.0f;
    }
}
