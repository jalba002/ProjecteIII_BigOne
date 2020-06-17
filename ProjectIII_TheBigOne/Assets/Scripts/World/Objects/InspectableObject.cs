using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

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

    public bool isPaper = true;

    private float timeToEnsureGoodReading;
    [Space(5)] public UnityEvent OnItemEndInteract = new UnityEvent();

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
            InspectInfo = new InspectableInfo(_meshFilter.mesh, _meshRenderer.materials, transform, maxLimits,
                objectInformation);
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
        if (isPaper)
            SoundManager.Instance.PlaySound2D("event:/SFX/UI/Inventory/PaperGrab");
        base.OnStartInteract();
        IsInteracting = true;
        timeToEnsureGoodReading = Time.time + 0.5f;
        SwapImages(true);
    }

    public override void OnInteracting()
    {
        /*GameManager.Instance.CanvasController.ChangeCursor(
            GameManager.Instance.CanvasController.CrosshairController.defaultCrosshair,
            new Vector3(0.1f, 0.1f, 1f));*/
    }

    public override void OnEndInteract()
    {
        //
        if (isPaper)
            SoundManager.Instance.PlaySound2D("event:/SFX/UI/Inventory/PaperLeft");
        base.OnEndInteract();

        if (Time.time >= timeToEnsureGoodReading)
            OnItemEndInteract.Invoke();
        SwapImages(false);
        IsInteracting = false;
    }

    private void SwapImages(bool enable)
    {
        if (enable)
        {
            var tempImage = previewImage;
            previewImage = displayImage;
            displayImage = tempImage;
        }
        else
        {
            var tempImage = displayImage;
            displayImage = previewImage;
            previewImage = tempImage;
        }
    }
}