using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InspectableObject : InteractableObject, IInspectable
{
    [Header("Settings")] public Vector2 maxLimits;

    [Space(5)] [Tooltip("Description shown when inspected")] [TextArea(5, 10)]
    public string objectInformation;

    [Header("Components")] private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    public InspectableInfo InspectInfo { get; set; }
    public bool IsBeingInspected { get; set; }

    [Header("Collision")] public List<Collider> ignoredColliders;
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
        OnStartInteract();
        return InspectInfo;
    }

    private void CreateInspectInfo()
    {
        //Debug.Log("Created inspect info.");
        if (InspectInfo.objectMesh == null || InspectInfo.objectTexture == null)
        {
            _meshFilter = GetComponentInChildren<MeshFilter>();
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            InspectInfo = new InspectableInfo(_meshFilter.mesh, _meshRenderer.materials, transform, maxLimits, objectInformation);
        }
    }

    public bool StopInspect()
    {
        _meshRenderer.enabled = true;
        OnEndInteract();
        return false;
    }

    public override bool Interact(bool interactEnable)
    {
        //
        Inspect();
        return false;
    }

    public override void OnStartInteract()
    {
        //
        base.OnStartInteract();
        IsInteracting = true;
    }

    public override void OnInteracting()
    {
        //
        base.OnInteracting();
    }

    public override void OnEndInteract()
    {
        //
        base.OnEndInteract();
        IsInteracting = false;
    }
}