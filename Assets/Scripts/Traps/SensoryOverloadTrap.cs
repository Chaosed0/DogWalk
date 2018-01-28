using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SensoryOverloadTrap: MonoBehaviour
{
	[Subscribe]
    public void HandlePlayerRanIntoTrap(Trap.PlayerRanIntoTrapEvent e)
    {
        e.player.GetComponent<Player>().GetConfused();
    }
}