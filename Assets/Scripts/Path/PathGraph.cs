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

        return path;
    }
}

public class PathGraph : MonoBehaviour
{
    public List<PathEdge> edges = new List<PathEdge>();

    public Dictionary<PathNeuronNode, List<PathEdge>> nodeToEdge = new Dictionary<PathNeuronNode, List<PathEdge>>();

    void Awake()
    {
        foreach (PathEdge edge in edges)
        {
            List<PathEdge> n1Edges = GetOrInsertEdgesForNode(edge.node1);
            List<PathEdge> n2Edges = GetOrInsertEdgesForNode(edge.node2);
            n1Edges.Add(edge);
            n2Edges.Add(edge);
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
                return ang1.CompareTo(ang2);
            });
        }
    }

    float GetClockwiseAngle(float z, float x)
    {
        float angle = Mathf.Atan2(z, x);
        if (angle < 0)
        {
            angle += Mathf.PI;
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
            if (edge.tendril == null || edge.tendril.GetFirst() == null)
            {
                Gizmos.DrawLine(edge.node1.transform.position, edge.node2.transform.position);
            }
            else
            {
                Gizmos.DrawLine(edge.node1.transform.position, edge.tendril.pathTendrilNodes[0].transform.position);
                for (int i = 0; i < edge.tendril.pathTendrilNodes.Count - 1; i++)
                {
                    Gizmos.DrawLine(edge.tendril.pathTendrilNodes[i].transform.position, edge.tendril.pathTendrilNodes[i+1].transform.position);
                }
                Gizmos.DrawLine(edge.tendril.pathTendrilNodes[edge.tendril.pathTendrilNodes.Count - 1].transform.position, edge.node2.transform.position);
            }
        }
    }
}
