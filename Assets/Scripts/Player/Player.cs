using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player: MonoBehaviour
{
    public PathNeuronNode startNode;
    public PathGraph pathGraph;

    PlayerMovement playerMovement;

    PathNeuronNode currentNode;
    List<PathEdge> availablePaths;
    int pathIndex = -1;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.gameObject.Subscribe<PlayerMovement.StoppedMovingEvent>((x) => OnStoppedMoving());
    }

    void Start()
    {
        SetCurrentNode(startNode);
        SetCurrentPathIndex(0);
        playerMovement.SetPosition(startNode.transform.position);
    }

    public void SelectNextPath(bool clockwise)
    {
        int newIndex = 0;
        if (pathIndex < 0)
        {
            newIndex = SelectNextPathAccordingToFacing(clockwise);
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
        SetCurrentPathIndex(newIndex);
    }

    public int SelectNextPathAccordingToFacing(bool clockwise)
    {
        float angle = PathGraph.GetClockwiseAngle(transform.forward.z, transform.forward.x);
        int firstLesserPath = 0;
        for (int i = 0; i < availablePaths.Count; i++)
        {
            Vector3 next = pathGraph.GetOtherNode(currentNode, availablePaths[i]).transform.position;
            Vector3 rel = next - currentNode.transform.position;
            float nodeAngle = PathGraph.GetClockwiseAngle(rel.z, rel.x);
            if (nodeAngle < angle)
            {
                firstLesserPath = i;
                break;
            }
        }

        if (clockwise)
        {
            return firstLesserPath;
        }
        else
        {
            return (firstLesserPath-1 + availablePaths.Count)%availablePaths.Count;
        }
    }

    public bool CanTraversePath()
    {
        return pathIndex >= 0 && !playerMovement.IsMoving();
    }

    public void TraversePath()
    {
        PathEdge pathToFollow = availablePaths[pathIndex];

        DeselectCurrentPath();
        playerMovement.StartFollowingPath(pathToFollow.GetPath(currentNode));
    }

    public void OnStoppedMoving()
    {
        PathEdge previousPath = availablePaths[pathIndex];
        SetCurrentNode(pathGraph.GetOtherNode(currentNode, previousPath));
        pathIndex = -1;
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
}
