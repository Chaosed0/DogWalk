using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSparks : MonoBehaviour {

	void Awake () {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ParticleSystem.ShapeModule shape = ps.shape;
        shape.mesh = GetComponent<MeshFilter>().mesh;
	}
}
