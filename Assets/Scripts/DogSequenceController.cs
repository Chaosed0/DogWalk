using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSequenceController : MonoBehaviour {
    public GameManager gameManager;

    public float time = 3.0f;

    [System.Serializable]
    public class Doge
    {
        public GameObject dog;
        public Transform pathStart;
        public Transform pathEnd;
    }

    public List<Doge> dogs;

    int playerWhoGainedScore;
    float scoreBonus;

    void Awake()
    {
        gameManager.gameObject.Subscribe<GameManager.PlayerGainedScoreEvent>((x) => {
            this.playerWhoGainedScore = x.player-1;
            this.scoreBonus = x.score;
        });
    }

    [SubscribeGlobal]
    void OnStartDogSequence(StartDogSequenceEvent e)
    {
        StartCoroutine(MoveDog());
    }

    IEnumerator MoveDog()
    {
        Doge dog = dogs[this.playerWhoGainedScore];
        Animator anim = dog.dog.GetComponent<Animator>();

        float start;
        float end;
        if (this.playerWhoGainedScore == 0) {
            start = gameManager.playerOneScore - scoreBonus;
            end = gameManager.playerOneScore;
        } else {
            start = gameManager.playerTwoScore - scoreBonus;
            end = gameManager.playerTwoScore;
        }

        float timer = 0.0f;

        anim.SetTrigger("Walk");

        while (timer <= time)
        {
            // TODO: Play animation
            float lerp = Mathf.Lerp(start, end, timer / time);
            Vector3 position = Vector3.Lerp(dog.pathStart.position, dog.pathEnd.position, lerp);
            dog.dog.transform.position = position;
            timer += Time.deltaTime;
            yield return null;
        }

        anim.SetTrigger("Walk");

        EventBus.PublishEvent(new EndDogSequenceEvent());
    }
}
