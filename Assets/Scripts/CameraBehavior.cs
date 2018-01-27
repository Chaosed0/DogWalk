using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    public float raceStartMovementSpeed;
    public float raceStartRotationSpeed;
    public Vector3 levelCreationCamPosition;
    public Vector3 raceStartPosition;
    public Vector3 raceStartRotation;

    public float dogTransitionMovementSpeed;
    public float dogTransitionRotationSpeed;
    public Vector3 dogViewPosition;
    public Vector3 dogViewRotation;

    public float skyboxLerpRate;

	void Start ()
    {
        transform.position = levelCreationCamPosition;
    }

    public void LerpToRaceStart()
    {
        StartCoroutine(LerpToPos(raceStartPosition, raceStartMovementSpeed));
        StartCoroutine(LerpToRot(raceStartRotation, raceStartRotationSpeed));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(LerpToPos(dogViewPosition, dogTransitionMovementSpeed));
            StartCoroutine(LerpToRot(dogViewRotation, dogTransitionRotationSpeed));
            StartCoroutine(LerpSky());
        }
    }

    IEnumerator LerpToPos(Vector3 targetPos, float speed)
    {
        float t = 0;
        Vector3 startPos = transform.position;

        while (t < 1)
        {
            t += Mathf.Clamp01(Time.deltaTime * speed);
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }
    }

    IEnumerator LerpToRot(Vector3 targetRotation, float speed)
    {
        float t = 0;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(targetRotation);

        while (t < 1)
        {
            t += Mathf.Clamp01(Time.deltaTime * speed);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }
    }

    IEnumerator LerpSky()
    {
        float t = 0;

        while (t < 1)
        {
            t += Mathf.Clamp01(Time.deltaTime * skyboxLerpRate);
            RenderSettings.skybox.SetFloat("_Blend", t);
            yield return null;
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
