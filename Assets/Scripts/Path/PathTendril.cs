using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTendril : MonoBehaviour {
    public List<PathTendrilNode> pathTendrilNodes = new List<PathTendrilNode>();
    MeshRenderer meshRenderer;
    public bool isTraversable = true;

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

    public void TogglePath()
    {
        if (isTraversable)
        {
            DisablePath();
        }
        else
        {
            EnablePath();
        }
    }

    public void EnablePath()
    {
        isTraversable = true;
    }

    public void DisablePath()
    {
        isTraversable = false;
    }

    public void SetTraversable(bool canTraverse)
    {
        isTraversable = canTraverse;
    }

    public void SetSelected(bool selected)
    {
        Color color;
        if (selected)
        {
            color = Color.red;
        }
        else
        {
            color = Color.white;
        }

        float alpha = meshRenderer.material.color.a;
        color.a = alpha;
        meshRenderer.material.color = color;
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
