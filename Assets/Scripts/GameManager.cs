using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // The player who currently has control.
    public int currentPlayer;

    public float playerOneScore;
    public float playerTwoScore;

    public Timer timer;

    [SubscribeGlobal]
    public void HandleRoundStart(RoundStartEvent e)
    {
        TogglePlayerControl();
    }

    [SubscribeGlobal]
    public void HandleRoundEnd(RoundEndEvent e)
    {
        // ghetto score heuristic - tweak later
        float scoreBonus = (timer.raceTime - e.remainingTime) / timer.raceTime;
        playerOneScore += currentPlayer == 1 ? scoreBonus : 0;
        playerTwoScore += currentPlayer == 2 ? scoreBonus : 0;

        TogglePlayerControl();
    }

    void TogglePlayerControl()
    {
        // TODO: if we use controllers, start ignoring the inactive player's controller here
        currentPlayer = currentPlayer == 1 ? 2 : 1;
        EventBus.PublishEvent(new ToggleCurrentPlayerEvent(currentPlayer));
    }
}
