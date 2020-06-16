using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveSphere : MonoBehaviour {

    Material mat;

    private void Start() {
        mat = GetComponent<Renderer>().material;
        mat.SetFloat("_DissolveAmount", 1);
    }

    private void Update() {
        mat.SetFloat("_DissolveAmount", mat.GetFloat("_DissolveAmount") - 0.5f * Time.deltaTime);
    }
}