using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNeuronNode : MonoBehaviour {
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        DoGizmos();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        DoGizmos();
    }

    void DoGizmos()
    {
        Gizmos.DrawIcon(transform.position, "spawn-node.png", true);
    }
}
