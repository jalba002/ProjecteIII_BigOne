using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;

public class DynamicObjectActivator : MonoBehaviour
{
    [Header("Configuration")]
    public float forceScale = 5f;

    public IInteractable detectedInteractable;
    [Space(10)]
    public UnityEvent OnObjectActivate;
    public UnityEvent OnObjectDeactivate;

    public bool Activate(IInteractable interactable)
    {
        if (detectedInteractable == null)
        {
            if (interactable != null)
            {
                detectedInteractable = interactable;
                detectedInteractable.OnStartInteract();
            }
            return false;
        }
        else
        {
            detectedInteractable.Interact(true);
            OnObjectActivate.Invoke();
            return true;
        }
    }

    public bool Deactivate()
    {
        if (detectedInteractable == null) return false;

        detectedInteractable = null;
        OnObjectDeactivate.Invoke();
        return true;
    }
}