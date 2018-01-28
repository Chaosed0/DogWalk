﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathNeuronNode : MonoBehaviour {
    Canvas levelCreationCanvas;
    Image nodeImage;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        DoGizmos();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        DoGizmos();
    }

    void DoGizmos()
    {
        Gizmos.DrawIcon(transform.position, "spawn-node.png", true);
    }

    void Start()
    {
        InitializeUINode();
    }

    [SubscribeGlobal]
    public void HandleLevelCreationStart (LevelCreationStartEvent e)
    {
        StartCoroutine(Util.DeferForOneFrame(InitializeUINode));
    }

    void InitializeUINode()
    {
        levelCreationCanvas = GameObject.Find("LevelCreationUI").GetComponent<Canvas>();
        nodeImage = Instantiate(Resources.Load("NeuronImage") as GameObject).GetComponent<Image>();

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 posCanvas = new Vector2((pos.x - .5f) * levelCreationCanvas.pixelRect.width,
            (pos.y - .5f) * levelCreationCanvas.pixelRect.height);

        nodeImage.rectTransform.SetParent(levelCreationCanvas.transform);
        nodeImage.rectTransform.anchoredPosition = posCanvas;
    }
}
