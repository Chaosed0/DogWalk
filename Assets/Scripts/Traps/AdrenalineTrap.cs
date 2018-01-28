using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AdrenalineTrap: MonoBehaviour
{
	[Subscribe]
    public void HandlePlayerRanIntoTrap(Trap.PlayerRanIntoTrapEvent e)
    {
        e.player.GetComponent<Player>().GetHyped();
    }
}