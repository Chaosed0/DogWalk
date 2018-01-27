using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNeuronNode : MonoBehaviour {
    void DoGizmos(bool doColor)
    {
        Gizmos.DrawIcon(transform.position, "spawn-node.png", true);
    }
}
