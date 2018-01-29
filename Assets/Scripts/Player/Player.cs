using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player: MonoBehaviour
{
    public PathGraph pathGraph;
    public float hypedTime = 10.0f;
    public float confusedTime = 10.0f;

    PlayerMovement playerMovement;

    PathNeuronNode currentNode;
    List<PathEdge> availablePaths;
    int pathIndex = -1;

    // For carrying over state between StartTraverse and StoppedMoving
    PathNeuronNode nextNode;

    Coroutine hypedCoroutine;
    Coroutine confusedCoroutine;

    public struct GetHypedEvent { }
    public struct StopHypedEvent { }

    public struct GetConfusedEvent { }
    public struct StopConfusedEvent { }

    public struct SelectPathEvent { }

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        playerMovement.gameObject.Subscribe<PlayerMovement.StoppedMovingEvent>((x) => OnStoppedMoving());
        playerMovement.gameObject.Subscribe<PlayerMovement.ReversedMovementEvent>((x) => OnReversedMovement());
    }

    [SubscribeGlobal]
    public void HandleRoundStart(RoundStartEvent e)
    {
        playerMovement.SetPosition(pathGraph.startNode.transform.position);
        SetCurrentNode(pathGraph.startNode);
        SetCurrentPathIndex(0);
    }

    [SubscribeGlobal]
    public void HandleRoundEnd(RoundEndEvent e)
    {
        if (hypedCoroutine != null)
        {
            StopCoroutine(hypedCoroutine);
            this.gameObject.PublishEvent(new StopHypedEvent());
        }

        if (confusedCoroutine != null)
        {
            StopCoroutine(confusedCoroutine);
            this.gameObject.PublishEvent(new StopConfusedEvent());
        }
    }

    [SubscribeGlobal]
    public void HandleGraphConfiguredEvent (GraphConfiguredEvent e)
    {
        pathGraph = e.graph;
    }

    public void SelectNextPath(bool clockwise)
    {
        int newIndex = 0;
        if (pathIndex < 0)
        {
            newIndex = 0;
        }
        else if (clockwise)
        {
            newIndex = (pathIndex+1)%availablePaths.Count;
        }
        else
        {
            newIndex = (pathIndex-1 + availablePaths.Count)%availablePaths.Count;
        }

        DeselectCurrentPath();
        this.pathIndex = -1;
        SetCurrentPathIndex(newIndex);

        this.gameObject.PublishEvent(new SelectPathEvent());
    }

    public int GetPathClosestToFacing()
    {
        int closestAngleIndex = 0;
        float closestAngle = float.MaxValue;
        for (int i = 0; i < availablePaths.Count; i++)
        {
            Vector3 next = pathGraph.GetOtherNode(currentNode, availablePaths[i]).transform.position;
            Vector3 rel = next - currentNode.transform.position;
            float nodeAngle = Vector3.Angle(rel, transform.forward);
            if (nodeAngle < closestAngle)
            {
                closestAngleIndex = i;
                closestAngle = nodeAngle;
            }
        }

        return closestAngleIndex;
    }

    public bool CanTraversePath()
    {
        return pathIndex >= 0 && !playerMovement.IsMoving();
    }

    public void TraversePath()
    {
        PathEdge pathToFollow = availablePaths[pathIndex];

        DeselectCurrentPath();
        playerMovement.StartFollowingPath(pathToFollow.GetPath(currentNode), pathToFollow.tendril.isDegraded);
        PathEdge previousPath = availablePaths[pathIndex];
        nextNode = pathGraph.GetOtherNode(currentNode, previousPath);
    }

    public void OnReversedMovement()
    {
        nextNode = currentNode;
    }

    public void OnStoppedMoving()
    {
        SetCurrentNode(nextNode);
        this.pathIndex = -1;
        SetCurrentPathIndex(GetPathClosestToFacing());

        if (currentNode == pathGraph.finishNode)
        {
            EventBus.PublishEvent(new PlayerFinishedMapEvent());
        }
    }

    void SetCurrentNode(PathNeuronNode node)
    {
        currentNode = node;
        availablePaths = pathGraph.GetAvailablePathsForNode(currentNode);
    }

    void DeselectCurrentPath()
    {
        if (pathIndex >= 0) {
            availablePaths[pathIndex].tendril.SetSelected(false);
        }
    }

    void SetCurrentPathIndex(int pathIndex)
    {
        if (this.pathIndex == pathIndex)
            return;

        this.pathIndex = pathIndex;
        availablePaths[this.pathIndex].tendril.SetSelected(true);

        Vector3 facing = pathGraph.GetOtherNode(currentNode, availablePaths[pathIndex]).transform.position - currentNode.transform.position;
        playerMovement.SetFacing(facing);
    }

    public void GetHyped() {
        this.gameObject.PublishEvent(new GetHypedEvent());
        if (hypedCoroutine != null) {
            StopCoroutine(hypedCoroutine);
        }
        hypedCoroutine = StartCoroutine(HypedTimer());
    }

    public IEnumerator HypedTimer()
    {
        yield return new WaitForSeconds(hypedTime);
        this.gameObject.PublishEvent(new StopHypedEvent());
    }

    public void GetConfused() {
        this.gameObject.PublishEvent(new GetConfusedEvent());
        if (confusedCoroutine != null) {
            StopCoroutine(confusedCoroutine);
        }
        confusedCoroutine = StartCoroutine(ConfusedTimer());
    }

    public IEnumerator ConfusedTimer()
    {
        yield return new WaitForSeconds(confusedTime);
        this.gameObject.PublishEvent(new StopConfusedEvent());
    }
}
