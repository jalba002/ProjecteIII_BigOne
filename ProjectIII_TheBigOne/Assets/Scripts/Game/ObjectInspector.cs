using System;
using Interfaces;
using UnityEngine;

public class ObjectInspector : MonoBehaviour
{
    public IInspectable currentInspectedObject;

    public GameObject feedbackVisual;

    private void Start()
    {
        feedbackVisual.SetActive(false);
    }

    public void Activate()
    {
        if (Utils.SimpleRaycast())
        {
            
        }
    }
    
    public bool StartInspect()
    {
        currentInspectedObject.Inspect();
        return false;
    }

    public void StopInspect()
    {
        currentInspectedObject.StopInspect();
    }
}
