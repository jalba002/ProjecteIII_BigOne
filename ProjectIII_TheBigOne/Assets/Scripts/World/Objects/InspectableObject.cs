using System;
using System.Collections.Generic;
using Tavaris.Interactable;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InspectableObject : InteractableObject, IInspectable
{
    public Vector2 maxLimits;
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    public InspectableInfo InspectInfo { get; set; }
    public bool IsBeingInspected { get; set; }

    public List<Collider> ignoredColliders;
    private Collider selfCollider;

    private void Start()
    {
        selfCollider = gameObject.GetComponent<Collider>();

        GenerateIgnoredColliders(selfCollider);
    }

    public void UpdateInteractable()
    {
        // Nothing.
    }

    private bool GenerateIgnoredColliders(Collider selfCollider)
    {
        try
        {
            Collider[] collectedColliders = gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider collider in collectedColliders)
            {
                if (collider.GetHashCode() != selfCollider.GetHashCode())
                {
                    ignoredColliders.Add(collider);
                }
            }
        }
        catch (NullReferenceException)
        {
            Debug.LogWarning("Ignored colliders generated wrongly in ", this.gameObject);
            return false;
        }

        return true;
    }

    private void IgnoreColliders()
    {
        foreach (Collider currentCollider in ignoredColliders)
        {
            if (currentCollider == null) break;
            Physics.IgnoreCollision(currentCollider, selfCollider, true);
        }
    }

    // Inspected object code starts below.//
    public InspectableInfo Inspect()
    {
        CreateInspectInfo();
        _meshRenderer.enabled = false;
        StartInteract();
        return InspectInfo;
    }

    private void CreateInspectInfo()
    {
        //Debug.Log("Created inspect info.");
        if (InspectInfo.objectMesh == null || InspectInfo.objectTexture == null)
        {
            _meshFilter = GetComponentInChildren<MeshFilter>();
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            InspectInfo = new InspectableInfo(_meshFilter.mesh, _meshRenderer.materials, transform, maxLimits);
        }
    }

    public bool StopInspect()
    {
        _meshRenderer.enabled = true;
        EndInteract();
        return false;
    }
    
    public override bool Interact(bool interactEnable)
    {
        //
        Inspect();
        return false;
    }

    public override void StartInteract()
    {
        base.StartInteract();
    }

    public override void Interacting()
    {
        base.Interacting();
    }

    public override void EndInteract()
    {
        base.EndInteract();
    }
}