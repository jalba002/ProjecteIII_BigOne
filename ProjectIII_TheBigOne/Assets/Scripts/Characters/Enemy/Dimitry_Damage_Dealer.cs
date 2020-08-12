using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Dimitry_Damage_Dealer : MonoBehaviour
{
    [Header("Components")] public Transform fistPosition;
    public Transform hammerHeadPosition;
    
    [Header("Defaults")]
    public Transform defaultPosition;
    private Collider collider;

    public void Init()
    {
        collider = new GameObject("[META]Dimitry_Damage_Trigger").AddComponent<SphereCollider>();
        ((SphereCollider) collider).radius = 0.25f;
        ((SphereCollider) collider).isTrigger = true;
        DisableCollider();
        SetOnNewPosition(defaultPosition);
    }
    public void SetOnNewPosition(Transform newParent)
    {
        collider.transform.parent = newParent;
        collider.transform.localPosition = Vector3.zero;
    }
    public void SetFistPosition()
    {
        SetOnNewPosition(fistPosition);
    }
    public void SetHammerPosition()
    {
        SetOnNewPosition(hammerHeadPosition);
    }
    public void ToggleCollider(bool setActive)
    {
        if(setActive)
            EnableCollider();
        else
        {
            DisableCollider();
        }
    }
    void EnableCollider()
    {
        if (collider != null)
            collider.enabled = true;
    }
    void DisableCollider()
    {
        if (collider != null)
            collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
