using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    Player player;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            player.TraversePath();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            player.SelectNextPath(false);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            player.SelectNextPath(true);
        }
    }
}

