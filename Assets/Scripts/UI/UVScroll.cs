using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroll : MonoBehaviour {

    public Material mat;
    public Vector2 offset = new Vector2();
    public float rate = 3f;

	void Update () {
        offset.x += rate * Time.deltaTime;
        mat.SetTextureOffset("_MainTex", offset);
	}
}
