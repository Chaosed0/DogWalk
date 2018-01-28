using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTendril : MonoBehaviour {
    public List<PathTendrilNode> pathTendrilNodes = new List<PathTendrilNode>();
    MeshRenderer meshRenderer;
    public bool isTraversable = false;

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
        // Debug.Log("this tendril: " + gameObject.name);
        // Debug.Log("isTraversable: " + isTraversable);
        if (isTraversable)
        {
            DisablePath();
        }
        else
        {
            EnablePath();
        }
        // Debug.Log("isTraversable: " + isTraversable);
    }

    public void EnablePath()
    {
        // Debug.Log("EnablePath()");
        SetTraversable(true);
    }

    public void DisablePath()
    {
        // Debug.Log("DisablePath()");
        SetTraversable(false);
    }

    public void SetTraversable(bool canTraverse)
    {
        // Debug.Log("SetTraversable( " + canTraverse + " )");
        isTraversable = canTraverse;

        Connection con = GetComponent<Connection>();
        if (con.tendrilImage != null)
        {
            con.tendrilImage.GetComponent<ConnectionUI>().ConfigureImage();
        }
        // this.gameObject.SetActive(canTraverse);
        EventBus.PublishEvent(new ToggleTendrilEvent(this));
    }

    [SubscribeGlobal]
    public void HandleRoundStart(RoundStartEvent e)
    {
        gameObject.SetActive(isTraversable);
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
