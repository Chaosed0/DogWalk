using System.Collections;
using UnityEngine;

class Util
{
    public static IEnumerator DeferForOneFrame(System.Action action)
    {
        yield return null;
        action();
    }
}
