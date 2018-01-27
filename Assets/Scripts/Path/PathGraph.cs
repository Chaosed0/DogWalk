using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        return path;
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
    
    public bool CanReachFinishIf(PathTendril tendril, bool tentativeState)
    {
        tendril.SetTraversable(tentativeState);
        bool canReachFinish = CanReachFinish();
        tendril.SetTraversable(!tentativeState);
        return canReachFinish;
    }

    public bool CanReachFinish()
    {
        if (gameManager.startNode == null || gameManager.finishNode == null)
        {
            Debug.LogError("Set start and finish in MainCamera/GameManager");
        }
        return DoesPathExist(gameManager.startNode, gameManager.finishNode);
    }

    public bool DoesPathExist(PathNeuronNode startNode, PathNeuronNode endNode)
    {
        bool pathExists = false;

        HashSet<PathNeuronNode> openSet = new HashSet<PathNeuronNode> { startNode };
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
                    if (edge.tendril.isTraversable)
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
        return nodeToEdge[node];
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
