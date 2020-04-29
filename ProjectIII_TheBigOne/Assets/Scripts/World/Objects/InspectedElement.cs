using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class InspectedElement : MonoBehaviour
{
    private Quaternion defaultRotation;
    private Vector3 defaultPosition;
    public MeshRenderer MeshRenderer { get; set; }
    public MeshFilter MeshFilter { get; set; }

    public void Awake()
    {
        SetDefaults();

        MeshRenderer = GetComponent<MeshRenderer>();
        MeshFilter = GetComponent<MeshFilter>();
    }

    // Set default variables for the rendered cube.
    void SetDefaults()
    {
        var objectTransform = transform;
        defaultRotation = objectTransform.localRotation;
        defaultPosition = objectTransform.localPosition;
    }

    void LoadDefaults()
    {
        var objectTransform = transform;
        objectTransform.localPosition = defaultPosition;
    }

    // Called when being rendered by ObjectInspector.
    public void SetComponents(Mesh mesh, Material[] materials, Quaternion rotation)
    {
        MeshFilter.mesh = mesh;
        MeshRenderer.materials = materials;
        transform.rotation = rotation;
        this.gameObject.SetActive(true);
    }

    // Called when stops being rendered by ObjectInspector
    public void ResetComponents()
    {
        MeshFilter.mesh = null;
        MeshRenderer.materials = new Material[]
        {
        };
        
        LoadDefaults();
        
        this.gameObject.SetActive(false);
    }
}