using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearDissolve : MonoBehaviour
{
    // Start is called before the first frame update
    Material mat;
    void Start()
    {
        mat = new Material(GetComponent<Renderer>().material);
        GetComponent<Renderer>().material = mat;

        mat.SetFloat("_DissolveAmount", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(mat.GetFloat("_DissolveAmount") > 0)
        {
            mat.SetFloat("_DissolveAmount", mat.GetFloat("_DissolveAmount") - 0.4f * Time.deltaTime);
        }
    }
}
