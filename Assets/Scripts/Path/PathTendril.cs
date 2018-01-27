using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTendril : MonoBehaviour {
    public List<PathTendrilNode> pathTendrilNodes = new List<PathTendrilNode>();
    MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public PathTendrilNode GetFirst()
    {
        if (pathTendrilNodes.Count == 0)
            return null;
        return pathTendrilNodes[0];
    }

    public PathTendrilNode GetLast()
    {
        if (pathTendrilNodes.Count == 0)
            return null;
        return pathTendrilNodes[pathTendrilNodes.Count - 1];
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            meshRenderer.material.color = Color.red;
        }
        else
        {
            meshRenderer.material.color = Color.white;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        DoGizmos();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        DoGizmos();
    }

    void DoGizmos()
    {
        foreach (PathTendrilNode node in pathTendrilNodes)
        {
            Gizmos.DrawSphere(node.transform.position, 0.5f);
        }
    }
}
