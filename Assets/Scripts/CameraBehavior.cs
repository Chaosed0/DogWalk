using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    public float raceStartMovementSpeed;
    public float raceStartRotationSpeed;
    public Vector3 levelCreationCamPosition;

    public float dogTransitionMovementSpeed;
    public float dogTransitionRotationSpeed;
    public Vector3 dogViewPosition;
    public Vector3 dogViewRotation;

    public float skyboxLerpRate;

    PlayerCameraBehavior playerCameraBehavior;

    int runningCoroutines = 0;
    System.Action OnCoroutinesStopped;

    void Awake()
    {
        playerCameraBehavior = GetComponent<PlayerCameraBehavior>();
        playerCameraBehavior.enabled = false;
    }

	void Start ()
    {
        transform.position = levelCreationCamPosition;
    }

    public void LerpToRaceStart()
    {
        StartCoroutine(LerpToPos(playerCameraBehavior.GetTargetPosition(), raceStartMovementSpeed));
        StartCoroutine(LerpToRot(playerCameraBehavior.GetTargetRotation(), raceStartRotationSpeed));

        OnCoroutinesStopped = () => { playerCameraBehavior.enabled = true; };
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerCameraBehavior.enabled = false;

            StartCoroutine(LerpToPos(dogViewPosition, dogTransitionMovementSpeed));
            StartCoroutine(LerpToRot(Quaternion.Euler(dogViewRotation), dogTransitionRotationSpeed));
            StartCoroutine(LerpSky());
        }
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

    IEnumerator LerpSky()
    {
        ++runningCoroutines;
        float t = 0;

        while (t < 1)
        {
            t += Mathf.Clamp01(Time.deltaTime * skyboxLerpRate);
            RenderSettings.skybox.SetFloat("_Blend", t);
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
        LerpToRaceStart();
    }

    void OnDestroy()
    {
        RenderSettings.skybox.SetFloat("_Blend", 0);
    }
}
