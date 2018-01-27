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

    void Awake()
    {
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
            // Sort by angle relative to north, i.e. what's clockwise
            pair.Value.Sort((PathEdge edge1, PathEdge edge2) => {
                Vector3 next1 = GetOtherNode(pair.Key, edge1).transform.position;
                Vector3 next2 = GetOtherNode(pair.Key, edge2).transform.position;
                Vector3 rel1 = next1 - pair.Key.transform.position;
                Vector3 rel2 = next2 - pair.Key.transform.position;
                float ang1 = GetClockwiseAngle(rel1.z, rel1.x);
                float ang2 = GetClockwiseAngle(rel2.z, rel2.x);
                return ang2.CompareTo(ang1);
            });
        }
    }

    float GetClockwiseAngle(float z, float x)
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
