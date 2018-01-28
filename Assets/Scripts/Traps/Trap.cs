using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {
    PathTendril tendril;

    public struct PlayerRanIntoTrapEvent {
        public GameObject player;
        public PlayerRanIntoTrapEvent(GameObject player) { this.player = player; }
    }

	[SubscribeGlobal]
    public void HandleRoundEndEvent(RoundEndEvent e)
    {
        Destroy(gameObject);
    }

    public void ApplyAffectsToPlayer(GameObject player)
    {
        if (player.GetComponent<PlayerMovement>().IsMoving())
        {
            this.gameObject.PublishEvent(new PlayerRanIntoTrapEvent(player));
        }
    }

    public void SetTendril(PathTendril tendril)
    {
        this.tendril = tendril;
    }

    public PathTendril GetTendril()
    {
        return this.tendril;
    }
}
