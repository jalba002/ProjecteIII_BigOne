using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Collider))]
public class InspectableObject : MonoBehaviour, IInspectable
{
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
        catch (Exception e)
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
        return InspectInfo;
    }

    private void CreateInspectInfo()
    {
        Debug.Log("Created inspect info.");
        if (InspectInfo.objectMesh == null || InspectInfo.objectTexture == null)
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            InspectInfo = new InspectableInfo(_meshFilter.mesh, _meshRenderer.materials, transform.rotation);
        }
    }
    
    public bool StopInspect()
    {
        _meshRenderer.enabled = true;
        return false;
    }
}
