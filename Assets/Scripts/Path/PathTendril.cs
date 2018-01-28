using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTendril : MonoBehaviour {
    public List<PathTendrilNode> pathTendrilNodes = new List<PathTendrilNode>();
    MeshRenderer meshRenderer;
    public bool isTraversable = true;

    Color tendrilColor;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        tendrilColor = meshRenderer.material.color;
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
        SetTraversable(true);
    }

    public void DisablePath()
    {
        SetTraversable(false);
    }

    public void SetTraversable(bool canTraverse)
    {
        isTraversable = canTraverse;
        if (!isTraversable)
        {
            this.gameObject.SetActive(false);
        }
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
            color = tendrilColor;
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
