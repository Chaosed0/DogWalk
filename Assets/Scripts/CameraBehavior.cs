using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    public float matchStartMovementSpeed;
    public float matchStartRotationSpeed;
    public Vector3 overheadPosition;
    public Vector3 matchStartPosition;
    public Vector3 matchStartRotation;

	void Start ()
    {
        transform.position = overheadPosition;
    }

    public void LerpToMatchStart()
    {
        StartCoroutine(LerpToMatchStartPos());
        StartCoroutine(LerpToMatchStartRot());
    }

    IEnumerator LerpToMatchStartPos()
    {
        float t = 0;
        Vector3 startPos = transform.position;

        while (t < 1)
        {
            t += Mathf.Clamp01(Time.deltaTime * matchStartMovementSpeed);
            transform.position = Vector3.Lerp(startPos, matchStartPosition, t);

            yield return null;
        }
    }

    IEnumerator LerpToMatchStartRot()
    {
        float t = 0;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(matchStartRotation);

        while (t < 1)
        {
            t += Mathf.Clamp01(Time.deltaTime * matchStartRotationSpeed);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }
    }
}
