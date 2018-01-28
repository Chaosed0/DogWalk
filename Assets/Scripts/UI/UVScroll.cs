using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroll : MonoBehaviour {

    public Material mat;
    public Vector2 offset = new Vector2();
    public Vector2 rate;

	void Update () {
        offset.x += rate.x * Time.deltaTime;
        offset.y += rate.y * Time.deltaTime;

        mat.SetTextureOffset("_MainTex", offset);
	}
}
