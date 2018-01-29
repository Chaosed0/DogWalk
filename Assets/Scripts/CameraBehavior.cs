using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    public float raceStartMovementSpeed;
    public float raceStartRotationSpeed;

    public Transform levelCreationTransform;

    public float dogTransitionMovementSpeed;
    public float dogTransitionRotationSpeed;
    public Transform dogViewTransform;

    public float skyboxLerpRate;
    public Light light;

    public Color brainLightColor;
    public Color outdoorLightColor;

    public float neuronLerpRate = 4f;
    public Color neuronDimColor;
    public Color neuronBrightColor;

    public Material neuronMat;

    PlayerCameraBehavior playerCameraBehavior;

    int runningCoroutines = 0;
    System.Action OnCoroutinesStopped;

    void Awake()
    {
        playerCameraBehavior = GetComponent<PlayerCameraBehavior>();
        light = GameObject.Find("Directional Light").GetComponent<Light>();
    }

	void Start ()
    {
        transform.position = levelCreationTransform.position;
        transform.rotation = levelCreationTransform.rotation;
    }

    public void LerpToLevelCreationStart()
    {
        StartCoroutine(LerpToPos(levelCreationTransform.position, raceStartMovementSpeed));
        StartCoroutine(LerpToRot(levelCreationTransform.rotation, raceStartRotationSpeed));
        StartCoroutine(LerpSky(false));
        StartCoroutine(LerpNeuronColor(true, neuronLerpRate));

        OnCoroutinesStopped = () => { EventBus.PublishEvent(new LevelCreationActuallyStartEvent()); };
    }

    public void LerpToRaceStart()
    {
        StartCoroutine(LerpToPos(playerCameraBehavior.GetTargetPosition(), raceStartMovementSpeed));
        StartCoroutine(LerpToRot(playerCameraBehavior.GetTargetRotation(), raceStartRotationSpeed));
        StartCoroutine(LerpNeuronColor(false, neuronLerpRate));

        OnCoroutinesStopped = () => { EventBus.PublishEvent(new RoundActuallyStartEvent()); };
    }

    public void LerpToDog()
    {
        StartCoroutine(LerpToPos(dogViewTransform.position, dogTransitionMovementSpeed));
        StartCoroutine(LerpToRot(dogViewTransform.rotation, dogTransitionRotationSpeed));
        StartCoroutine(LerpSky(true));
    }

    IEnumerator LerpToPos(Vector3 targetPos, float speed)
    {
        ++runningCoroutines;
        float t = 0;
        Vector3 startPos = transform.position;

        while (t < 1)
        {
            t += Mathf.Clamp01(Time.deltaTime * speed);
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }

        CoroutineStopped();
    }

    IEnumerator LerpNeuronColor (bool dim, float speed)
    {
        ++runningCoroutines;
        float t = 0;
        Color startCol = dim ? neuronBrightColor : neuronDimColor;
        Color endCol = dim ? neuronDimColor : neuronBrightColor;

        while (t < 1)
        {
            t += Mathf.Clamp01(Time.deltaTime * speed);
            neuronMat.SetColor("_Color", Color.Lerp(startCol, endCol, t));

            yield return null;
        }

        CoroutineStopped();
    }

    IEnumerator LerpToRot(Quaternion targetRotation, float speed)
    {
        ++runningCoroutines;
        float t = 0;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = targetRotation;

        while (t < 1)
        {
            t += Mathf.Clamp01(Time.deltaTime * speed);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        CoroutineStopped();
    }

    IEnumerator LerpSky(bool lighten)
    {
        ++runningCoroutines;
        float t = 0;

        while (t < 1)
        {
            t += Mathf.Clamp01(Time.deltaTime * skyboxLerpRate);
            RenderSettings.skybox.SetFloat("_Blend", (lighten ? t : 1-t));
            light.color = lighten ? Color.Lerp(brainLightColor, outdoorLightColor, t) : Color.Lerp(outdoorLightColor, brainLightColor, t);
            yield return null;
        }

        CoroutineStopped();
    }

    void CoroutineStopped()
    {
        --runningCoroutines;
        if (runningCoroutines <= 0 && OnCoroutinesStopped != null)
        {
            OnCoroutinesStopped();
            OnCoroutinesStopped = null;
        }
    }

    [SubscribeGlobal]
    public void HandleRoundStart(RoundStartEvent e)
    {
        Invoke("LerpToRaceStart", 1.0f);
    }

    [SubscribeGlobal]
    public void HandleRoundEnd(RoundEndEvent e)
    {
        LerpToDog();

        OnCoroutinesStopped = () => { EventBus.PublishEvent(new StartDogSequenceEvent()); };
    }

    [SubscribeGlobal]
    public void HandleLevelCreationStart(LevelCreationStartEvent e)
    {
        LerpToLevelCreationStart();
    }

    void OnDestroy()
    {
        RenderSettings.skybox.SetFloat("_Blend", 0);
    }
}
