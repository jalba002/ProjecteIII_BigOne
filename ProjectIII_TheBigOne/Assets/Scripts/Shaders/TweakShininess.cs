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

    private Renderer attachedRenderer;

    public bool taken = false;

    private void Start()
    {        
        _currentShininess = minShininess;
        _increasing = true;
        attachedRenderer = GetComponentInChildren<Renderer>();
        
        Material matInstance = new Material(attachedRenderer.material);
        attachedRenderer.material = matInstance;
        this.attachedRenderer.sharedMaterial.SetFloat("_Shininess", minShininess);
        
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
        this.attachedRenderer.sharedMaterial.SetFloat("_Shininess", 0);
        taken = true;
    }

    public void ChangeMatValues()
    {        
        if (_increasing)
        {
            this.attachedRenderer.sharedMaterial.SetFloat("_Shininess", this.attachedRenderer.sharedMaterial.GetFloat("_Shininess") + shininessIncreasePerSecond * Time.deltaTime);
        }
        else
        {
            this.attachedRenderer.sharedMaterial.SetFloat("_Shininess", this.attachedRenderer.sharedMaterial.GetFloat("_Shininess") - shininessIncreasePerSecond * Time.deltaTime);
        }

        if (this.attachedRenderer.sharedMaterial.GetFloat("_Shininess") > maxShininess)
        {
            _increasing = false;
        }
        if (this.attachedRenderer.sharedMaterial.GetFloat("_Shininess") < minShininess)
        {
            _increasing = true;
        }
        _currentShininess = this.attachedRenderer.sharedMaterial.GetFloat("_Shininess");
    }
}
