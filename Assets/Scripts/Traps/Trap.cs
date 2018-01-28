using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

	[SubscribeGlobal]
    public void HandleRoundEndEvent(RoundEndEvent e)
    {
        Destroy(gameObject);
    }
}
