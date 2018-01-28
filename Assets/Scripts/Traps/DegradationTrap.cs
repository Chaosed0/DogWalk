using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DegradationTrap: MonoBehaviour
{
    Trap trap;

    void Awake()
    {
        trap = GetComponent<Trap>();
    }

    void Start()
    {
        trap.GetTendril().isDegraded = true;
    }

    void OnDestroy()
    {
        if (trap != null && trap.GetTendril() != null)
        {
            trap.GetTendril().isDegraded = false;
        }
    }
}