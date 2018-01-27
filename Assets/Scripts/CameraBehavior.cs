using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    public float raceStartMovementSpeed;
    public float raceStartRotationSpeed;
    public Vector3 levelCreationCamPosition;
    public Vector3 raceStartPosition;
    public Vector3 raceStartRotation;

	void Start ()
    {
        transform.position = levelCreationCamPosition;
    }

    public void LerpToRaceStart()
    {
        StartCoroutine(LerpToRaceStartPos());
        StartCoroutine(LerpToRaceStartRot());
    }

    IEnumerator LerpToRaceStartPos()
    {
        float t = 0;
        Vector3 startPos = transform.position;

        while (t < 1)
        {
            t += Mathf.Clamp01(Time.deltaTime * raceStartMovementSpeed);
            transform.position = Vector3.Lerp(startPos, raceStartPosition, t);

            yield return null;
        }
    }

    IEnumerator LerpToRaceStartRot()
    {
        float t = 0;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(raceStartRotation);

        while (t < 1)
        {
            t += Mathf.Clamp01(Time.deltaTime * raceStartRotationSpeed);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }
    }

    [SubscribeGlobal]
    public void HandleRoundStart(RoundStartEvent e)
    {
        LerpToRaceStart();
    }
}
