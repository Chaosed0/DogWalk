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
        //facing = pathGraph.GetNext(startNode, availablePaths[pathIndex]);
    }

    public void SelectNextPath(bool increment)
    {
        int newIndex = 0;
        if (increment)
        {
            newIndex = (pathIndex+1)%availablePaths.Count;
        }
        else
        {
            newIndex = (pathIndex-1 + availablePaths.Count)%availablePaths.Count;
        }

        DeselectCurrentPath();
        SetCurrentPathIndex(newIndex);

        //facing = pathGraph.GetNext(startNode, availablePaths[pathIndex]);
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
    }

    void SetCurrentNode(PathNeuronNode node)
    {
        transform.position = node.transform.position;

        currentNode = node;
        availablePaths = pathGraph.GetAvailablePathsForNode(currentNode);
        SetCurrentPathIndex(0);
    }

    void DeselectCurrentPath()
    {
        if (this.pathIndex >= 0) {
            availablePaths[this.pathIndex].tendril.SetSelected(false);
        }
    }

    void SetCurrentPathIndex(int pathIndex)
    {
        if (this.pathIndex == pathIndex)
            return;

        this.pathIndex = pathIndex;
        availablePaths[this.pathIndex].tendril.SetSelected(true);

        Vector3 facing = pathGraph.GetOtherNode(currentNode, availablePaths[pathIndex]).transform.position - currentNode.transform.position;
        GetComponent<Rigidbody>().MoveRotation(Quaternion.LookRotation(facing));
    }
}
