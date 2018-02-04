using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public AudioSource source;

    // The player who currently has control.
    public int currentPlayer;

    public float playerOneScore;
    public float playerTwoScore;

    public Material coreMaterial;
    public Material finishLineCoreMaterial;

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
        Cursor.visible = false;
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
        int seed = (int)(System.DateTime.Now - new System.DateTime(2017, 1, 1)).TotalSeconds;
        //int seed = 33923864;
        Random.InitState(seed);
        //Debug.Log(seed);

        PathGraph graph = FindObjectOfType<PathGraph>();
        if (graph.finishNode)
        {
            SetCoreMat(graph.finishNode.gameObject, coreMaterial);
        }
        PathGraph.RandomPath randomPath = graph.GetRandomPath(targetPathLength);
        graph.startNode = randomPath.startNode;
        graph.finishNode = randomPath.finishNode;
        // Debug.Log("CanReachFinish: " + graph.CanReachFinish());
        // Debug.Log("pathEdges: " + randomPath.pathEdges.Count);
        foreach (PathEdge edge in graph.edges)
        {
            edge.tendril.gameObject.SetActive(true);
            edge.tendril.SetTraversable(false);
        }

        foreach (PathEdge edge in randomPath.pathEdges)
        {
            edge.tendril.SetTraversable(true);
        }

        SetCoreMat(graph.finishNode.gameObject, finishLineCoreMaterial);

        EventBus.PublishEvent(new GraphConfiguredEvent(graph));
    }

    public void SetCoreMat(GameObject obj, Material mat)
    {
        MeshRenderer rendera = obj.GetComponent<MeshRenderer>();
        if (rendera)
        {
            Material[] mats = rendera.materials;
            if (mats.Length >= 2)
            {
                mats[1] = mat;
                rendera.materials = mats;
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
        float scoreBonus = 0f;
        if (e.remainingTime > 0.0f)
        {
            scoreBonus = .2f + e.remainingTime / timer.raceTime;
        }
        playerOneScore += currentPlayer == 1 ? scoreBonus : 0;
        playerTwoScore += currentPlayer == 2 ? scoreBonus : 0;

        this.gameObject.PublishEvent(new PlayerGainedScoreEvent(currentPlayer, scoreBonus));
    }

    [SubscribeGlobal]
    public void HandleDogSequenceEnd(EndDogSequenceEvent e)
    {
        Cursor.visible = true;
        int winner = GetPlayerWhoWon();
        if (winner < 0)
        {
            EventBus.PublishEvent(new LevelCreationStartEvent());
        }
        else
        {
            source.Play();
            EventBus.PublishEvent(new WinrarEvent(winner));
        }
    }

    [SubscribeGlobal]
    public void HandleWinrarEvent(WinrarEvent e)
    {
        List<Doge> dogs = GetComponent<DogSequenceController>().dogs;
        Doge winningDog = dogs[e.player];
        Doge losingDog = e.player == 1 ? dogs[0] : dogs[1];

        winningDog.dog.GetComponent<Animator>().SetTrigger("Win");
        losingDog.dog.GetComponent<Animator>().SetTrigger("Lose");

        Invoke("FadeToEnd", 5);
    }

    void FadeToEnd()
    {
        StartCoroutine(FadeToLevel());
    }

    // Copy and paste, bitches!
    IEnumerator FadeToLevel ()
    {
        Image fadePanel = GameObject.Find("FadePanel").GetComponent<Image>();

        while (fadePanel.color.a < 1)
        {
            Color col = fadePanel.color;
            col.a += 2f * Time.deltaTime;
            fadePanel.color = col;

            yield return null;
        }
        SceneManager.LoadSceneAsync("Credits");
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
