using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

class Player: MonoBehaviour
{
    public PathNeuronNode startNode;
    public PathGraph pathGraph;

    PathNeuronNode currentNode;
    List<PathEdge> availablePaths;
    int pathIndex = -1;

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

        SetCurrentPathIndex(newIndex);

        //facing = pathGraph.GetNext(startNode, availablePaths[pathIndex]);
    }

    public void TraversePath()
    {
        PathEdge pathToFollow = availablePaths[pathIndex];
        pathIndex = -1;

        pathToFollow.tendril.SetSelected(false);
        SetCurrentNode(pathGraph.GetOtherNode(currentNode, pathToFollow));
    }

    void SetCurrentNode(PathNeuronNode node)
    {
        transform.position = node.transform.position;

        currentNode = node;
        availablePaths = pathGraph.GetAvailablePathsForNode(currentNode);
        SetCurrentPathIndex(0);
        //facing = pathGraph.GetNext(currentNode, availablePaths[pathIndex]);
    }

    void SetCurrentPathIndex(int pathIndex)
    {
        if (this.pathIndex == pathIndex)
            return;

        if (this.pathIndex >= 0) {
            availablePaths[this.pathIndex].tendril.SetSelected(false);
        }

        this.pathIndex = pathIndex;
        availablePaths[this.pathIndex].tendril.SetSelected(true);
    }
}
