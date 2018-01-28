using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class NeurotransmitterTrap: MonoBehaviour
{
	[Subscribe]
    public void HandlePlayerRanIntoTrap(Trap.PlayerRanIntoTrapEvent e)
    {
        e.player.GetComponent<PlayerMovement>().ReverseMovement();
    }
}