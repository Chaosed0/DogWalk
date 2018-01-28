using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // The player who currently has control.
    public int currentPlayer;

    public float playerOneScore;
    public float playerTwoScore;

    public Timer timer;

    public int targetPathLength = 5;

    private void Start()
    {
        InitializeStartingPath();
    }

    public struct PlayerGainedScoreEvent {
        public int player;
        public float score;
        public PlayerGainedScoreEvent(int player, float score)
        {
            this.player = player;
            this.score = score;
        }
    }

    [SubscribeGlobal]
    public void HandleRoundStart(RoundStartEvent e)
    {
        TogglePlayerControl();
    }

    [SubscribeGlobal]
    public void HandleLevelCreationStart(LevelCreationStartEvent e)
    {
        InitializeStartingPath();
    }

    public void InitializeStartingPath()
    {
        PathGraph graph = FindObjectOfType<PathGraph>();
        PathGraph.RandomPath randomPath = graph.GetRandomPath(targetPathLength);
        graph.startNode = randomPath.startNode;
        graph.finishNode = randomPath.finishNode;
        foreach (PathEdge edge in graph.edges)
        {
            edge.tendril.gameObject.SetActive(true);
            if (randomPath.pathEdges.Contains(edge))
            {
                edge.tendril.SetTraversable(true);
            }
            else
            {
                edge.tendril.SetTraversable(false);
            }
        }
    }

    [SubscribeGlobal]
    public void HandleRoundEnd(RoundEndEvent e)
    {
        PathGraph graph = FindObjectOfType<PathGraph>();
        graph.startNode = null;
        graph.finishNode = null;

        // ghetto score heuristic - tweak later
        float scoreBonus = e.remainingTime / timer.raceTime;
        playerOneScore += currentPlayer == 1 ? scoreBonus : 0;
        playerTwoScore += currentPlayer == 2 ? scoreBonus : 0;

        this.gameObject.PublishEvent(new PlayerGainedScoreEvent(currentPlayer, scoreBonus));
    }

    [SubscribeGlobal]
    public void HandleDogSequenceEnd(EndDogSequenceEvent e)
    {
        int winner = GetPlayerWhoWon();
        if (winner < 0)
        {
            TogglePlayerControl();
            EventBus.PublishEvent(new LevelCreationStartEvent());
        }
        else
        {
            EventBus.PublishEvent(new WinrarEvent(winner));
        }
    }

    void TogglePlayerControl()
    {
        // TODO: if we use controllers, start ignoring the inactive player's controller here
        currentPlayer = currentPlayer == 1 ? 2 : 1;
        EventBus.PublishEvent(new ToggleCurrentPlayerEvent(currentPlayer));
    }

    public int GetPlayerWhoWon() {
        if (playerOneScore >= 1.0f)
        {
            return 0;
        }
        else if (playerTwoScore >= 1.0f)
        {
            return 1;
        }

        return -1;
    }
}
