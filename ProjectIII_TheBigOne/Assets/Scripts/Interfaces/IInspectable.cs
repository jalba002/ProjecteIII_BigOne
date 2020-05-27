using UnityEngine;


[System.Serializable]
public struct InspectableInfo
{
    public Mesh objectMesh;
    public Material[] objectTexture;
    public Transform objectTransform;
    public Vector2 maxRanges;

    public InspectableInfo(Mesh mesh, Material[] material, Transform objectTransform, Vector2 maxRanges)
    {
        objectMesh = mesh;
        objectTexture = material;
        this.objectTransform = objectTransform;
        this.maxRanges = maxRanges;
    }
}

public interface IInspectable
{
    InspectableInfo InspectInfo { get; set; }
    bool IsBeingInspected { get; set; }
    InspectableInfo Inspect();
    bool StopInspect();
}