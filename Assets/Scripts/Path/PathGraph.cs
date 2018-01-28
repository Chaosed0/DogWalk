using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class PathEdge
{
    public PathNeuronNode node1;
    public PathNeuronNode node2;
    public PathTendril tendril;

    public List<Vector3> GetPath(PathNeuronNode start)
    {
        List<Vector3> path = new List<Vector3>();

        path.Add(node1.transform.position);
        foreach (PathTendrilNode node in tendril.pathTendrilNodes)
        {
            path.Add(node.transform.position);
        }
        path.Add(node2.transform.position);

        if (node1 != start)
        {
            path.Reverse();
        }

        path = SplineUtil.GenerateSpline(path, 5);
        // Not sure why, but the spline doesn't include the final point
        path.Add((start == node1 ? node2: node1).transform.position);

        return path;
    }

    float GetPathLength (List<Vector3> path)
    {
        float length = 0;

        for (int i = 0; i < path.Count - 1; i++)
        {
            length += Vector3.Distance(path[i], path[i + 1]);
        }

        return length;
    }

    public Vector3 GetPointAlongPath (PathNeuronNode start, float t)
    {
        List<Vector3> path = GetPath(start);
        float length = GetPathLength(path);
        float distanceTraveled = 0;
        float distanceAlongCurrentPath = 0;
        float accumulatedT = 0;

        int i = 0;
        while (i < path.Count - 1 && accumulatedT < t)
        {
            distanceAlongCurrentPath = Mathf.Min(Vector3.Distance(path[i], path[i + 1]), (t - accumulatedT) * length);
            distanceTraveled += distanceAlongCurrentPath;
            accumulatedT = distanceTraveled / length;
            i++;
        }

        if (i != 0)
        {
            return path[i - 1] + distanceAlongCurrentPath * (path[i] - path[i - 1]).normalized;
        }
        else
        {
            Debug.LogError("You fucked up mang.");
            return new Vector3();
        }
    }
}

public class PathGraph : MonoBehaviour
{
    public List<PathEdge> edges = new List<PathEdge>();

    public Dictionary<PathNeuronNode, List<PathEdge>> nodeToEdge = new Dictionary<PathNeuronNode, List<PathEdge>>();
    public Dictionary<PathTendril, PathEdge> tendrilToEdge = new Dictionary<PathTendril, PathEdge>();

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        foreach (PathEdge edge in edges)
        {
            List<PathEdge> n1Edges = GetOrInsertEdgesForNode(edge.node1);
            List<PathEdge> n2Edges = GetOrInsertEdgesForNode(edge.node2);
            n1Edges.Add(edge);
            n2Edges.Add(edge);

            tendrilToEdge[edge.tendril] = edge;
        }

        foreach (KeyValuePair<PathNeuronNode, List<PathEdge>> pair in nodeToEdge)
        {
            // Sort by angle clockwise relative to north
            pair.Value.Sort((e1, e2) => CompareAnglesOfEdges(pair.Key, e1, e2));
        }
    }

    public int CompareAnglesOfEdges(PathNeuronNode start, PathEdge edge1, PathEdge edge2)
    {
        Vector3 next1 = GetOtherNode(start, edge1).transform.position;
        Vector3 next2 = GetOtherNode(start, edge2).transform.position;
        Vector3 rel1 = next1 - start.transform.position;
        Vector3 rel2 = next2 - start.transform.position;
        float ang1 = GetClockwiseAngle(rel1.z, rel1.x);
        float ang2 = GetClockwiseAngle(rel2.z, rel2.x);
        return ang2.CompareTo(ang1);
    }

    [SubscribeGlobal]
    public void HandleRoundStart(RoundStartEvent e)
    {
        List<PathEdge> randomPath = GetRandomPath((int)(nodeToEdge.Keys.Count / 4f));
        // TODO: foreach edge on randomPath, toggle path to active
    }

    public List<PathEdge> GetRandomPath(int targetPathLength)
    {
        List<PathEdge> pathEdges = new List<PathEdge>();

        List<PathNeuronNode> allNodes = new List<PathNeuronNode>(nodeToEdge.Keys);
        PathNeuronNode startNode = allNodes[Random.Range(0, allNodes.Count)];
 
        HashSet<PathNeuronNode> closedNodeSet = new HashSet<PathNeuronNode> { startNode };
        Dictionary<PathNeuronNode, HashSet<PathEdge>> closedEdgeSet = new Dictionary<PathNeuronNode, HashSet<PathEdge>>();

        PathNeuronNode currentNode = startNode;
        while (pathEdges.Count < targetPathLength)
        {
            if (!closedEdgeSet.Keys.Contains(currentNode))
            {
                closedEdgeSet.Add(currentNode, new HashSet<PathEdge>());
            }
            List<PathEdge> availableEdges = nodeToEdge[currentNode].Where(x => !closedEdgeSet[currentNode].Contains(x)
                                                                            && !closedNodeSet.Contains(GetOtherNode(currentNode, x))).ToList();
            // Back-track if we've reached a dead end
            if (availableEdges.Count == 0)
            {
                if (currentNode == startNode)
                {
                    Debug.LogError("Random Path could not be found for start node: " + startNode.gameObject.name + " with target path length: " + targetPathLength.ToString());
                }
                PathEdge previousPath = pathEdges[pathEdges.Count - 1];
                PathNeuronNode previousNode = GetOtherNode(currentNode, previousPath);
                closedEdgeSet[previousNode].Add(previousPath);
                pathEdges.Remove(previousPath);
                currentNode = previousNode;
                continue;
            }

            PathEdge nextEdge = availableEdges[Random.Range(0, availableEdges.Count)];
            pathEdges.Add(nextEdge);
            currentNode = GetOtherNode(currentNode, nextEdge);
        }

        return pathEdges;
    }

    public bool CanReachFinishIf(PathTendril tendril, bool tentativeState)
    {
        PathEdge edgeException = tentativeState ? null : tendrilToEdge[tendril];
        bool canReachFinish = DoesPathExist(gameManager.startNode, gameManager.finishNode, edgeException, tentativeState);
        return canReachFinish;
    }

    public bool CanReachFinish()
    {
        if (gameManager.startNode == null || gameManager.finishNode == null)
        {
            Debug.LogError("Set start and finish in GameManager");
        }
        return DoesPathExist(gameManager.startNode, gameManager.finishNode);
    }

    public bool DoesPathExist(PathNeuronNode startNode, PathNeuronNode endNode, PathEdge edgeException = null, bool exceptionState = true)
    {
        bool pathExists = false;
        
        HashSet<PathNeuronNode> closedSet = new HashSet<PathNeuronNode> { startNode };

        Queue<PathNeuronNode> queue = new Queue<PathNeuronNode>();
        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            PathNeuronNode node = queue.Dequeue();
            foreach (PathEdge edge in nodeToEdge[node])
            {
                PathNeuronNode otherNode = GetOtherNode(node, edge);

                if (!closedSet.Contains(otherNode))
                {
                    if (edge == edgeException ? exceptionState : edge.tendril.isTraversable)
                    {
                        if (otherNode == endNode)
                        {
                            return true;
                        }
                        queue.Enqueue(otherNode);
                        closedSet.Add(otherNode);
                    }
                }
            }
        }

        return pathExists;
    }

    public static float GetClockwiseAngle(float z, float x)
    {
        float angle = Mathf.Atan2(z, x);
        if (angle < 0)
        {
            angle += 2*Mathf.PI;
        }
        return angle;
    }

    public PathNeuronNode GetOtherNode(PathNeuronNode node, PathEdge edge)
    {
        if (node == edge.node1)
        {
            return edge.node2;
        }
        else
        {
            return edge.node1;
        }
    }

    public Vector3 GetNext(PathNeuronNode node, PathEdge edge)
    {
        if (node == edge.node1)
        {
            if (edge.tendril != null && edge.tendril.GetFirst() != null)
            {
                return edge.tendril.GetFirst().transform.position;
            }

            return edge.node2.transform.position;
        }
        else
        {
            if (edge.tendril != null && edge.tendril.GetFirst() != null)
            {
                return edge.tendril.GetLast().transform.position;
            }

            return edge.node2.transform.position;
        }
    }

    List<PathEdge> GetOrInsertEdgesForNode(PathNeuronNode node)
    {
        List<PathEdge> neighborEdges;
        if (!nodeToEdge.TryGetValue(node, out neighborEdges))
        {
            neighborEdges = new List<PathEdge>();
            nodeToEdge[node] = neighborEdges;
        }
        return neighborEdges;
    }

    public List<PathEdge> GetAvailablePathsForNode(PathNeuronNode node)
    {
        List<PathEdge> availablePaths = nodeToEdge[node];
        return availablePaths.Where((edge) => edge.tendril.isTraversable).ToList();
    }

    void OnDrawGizmos()
    {
        foreach (PathEdge edge in edges)
        {
            List<Vector3> path = edge.GetPath(edge.node1);
            for (int i = 0; i < path.Count - 1; i++)
            {
                Gizmos.DrawLine(path[i], path[i+1]);
            }
        }
    }
}
