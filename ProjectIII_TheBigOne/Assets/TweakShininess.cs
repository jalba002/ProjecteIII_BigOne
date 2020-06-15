using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweakShininess : MonoBehaviour
{

    public float maxShininess = 0.2f;
    public float minShininess = 0.05f;
    public float shininessIncreasePerSecond = 0.03f;

    [SerializeField]private float _currentShininess;
    private bool _increasing;

    public bool taken = false;

    private void Start()
    {        
        _currentShininess = minShininess;
        _increasing = true;
        Material matInstance = new Material(GetComponent<Renderer>().material);
        GetComponent<Renderer>().material = matInstance;
        this.GetComponent<Renderer>().sharedMaterial.SetFloat("_Shininess", minShininess);
        taken = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!taken)
        {
            ChangeMatValues();
        }
    }

    public void TakePostcard()
    {
        this.GetComponent<Renderer>().sharedMaterial.SetFloat("_Shininess", 0);
        taken = true;
    }

    public void ChangeMatValues()
    {        
        if (_increasing)
        {
            this.GetComponent<Renderer>().sharedMaterial.SetFloat("_Shininess", this.GetComponent<Renderer>().sharedMaterial.GetFloat("_Shininess") + shininessIncreasePerSecond * Time.deltaTime);
        }
        else
        {
            this.GetComponent<Renderer>().sharedMaterial.SetFloat("_Shininess", this.GetComponent<Renderer>().sharedMaterial.GetFloat("_Shininess") - shininessIncreasePerSecond * Time.deltaTime);
        }

        if (this.GetComponent<Renderer>().sharedMaterial.GetFloat("_Shininess") > maxShininess)
        {
            _increasing = false;
        }
        if (this.GetComponent<Renderer>().sharedMaterial.GetFloat("_Shininess") < minShininess)
        {
            _increasing = true;
        }
        _currentShininess = this.GetComponent<Renderer>().sharedMaterial.GetFloat("_Shininess");
    }
}
