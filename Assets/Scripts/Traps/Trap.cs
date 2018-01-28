using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

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
        this.gameObject.PublishEvent(new PlayerRanIntoTrapEvent(player));
    }
}
