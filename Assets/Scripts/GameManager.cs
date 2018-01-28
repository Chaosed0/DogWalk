using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    // The player who currently has control.
    public int currentPlayer;

    public float playerOneScore;
    public float playerTwoScore;

    public Timer timer;

    public int targetPathLength = 6;

    public int currentLevel = 0;
    public List<GameObject> levels;

    private void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance);
        }
    }

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
    public void HandleDogSequenceEnd (LevelCreationStartEvent e)
    {
        if (currentPlayer == 1)
        {
            if (!(currentLevel == levels.Count - 1))
            {
                Destroy(levels[currentLevel]);
                currentLevel++;
                levels[currentLevel].SetActive(true);
            }
            else
            {
                Debug.Log("game is over yo");
            }
        }

        InitializeStartingPath();
    }

    public void InitializeStartingPath()
    {
        PathGraph graph = FindObjectOfType<PathGraph>();
        PathGraph.RandomPath randomPath = graph.GetRandomPath(targetPathLength);
        graph.startNode = randomPath.startNode;
        graph.finishNode = randomPath.finishNode;
        // Debug.Log("CanReachFinish: " + graph.CanReachFinish());
        // Debug.Log("pathEdges: " + randomPath.pathEdges.Count);
        foreach (PathEdge edge in graph.edges)
        {
            edge.tendril.gameObject.SetActive(true);
            if (randomPath.pathEdges.Contains(edge))
            {
                // Debug.Log("edge" + edge.tendril);
                edge.tendril.SetTraversable(true);
            }
            else
            {
                edge.tendril.SetTraversable(false);
            }
        }

        EventBus.PublishEvent(new GraphConfiguredEvent(graph));
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
