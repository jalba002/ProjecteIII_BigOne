using Interfaces;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class InspectableCube : MonoBehaviour, IInspectable
{
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    public InspectableInfo InspectInfo { get; set; }
    public bool IsBeingInspected { get; set; }

    public void Awake()
    {
        
    }

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
            InspectInfo = new InspectableInfo(_meshFilter.mesh, _meshRenderer.materials);
        }
    }
    
    public bool StopInspect()
    {
        _meshRenderer.enabled = true;
        return false;
    }
}
