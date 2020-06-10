using UnityEngine;


[System.Serializable]
public struct InspectableInfo
{
    public Mesh objectMesh;
    public Material[] objectTexture;
    public Transform objectTransform;
    public Vector2 maxRanges;
    public string objectInfo;

    public InspectableInfo(Mesh mesh, Material[] material, Transform objectTransform, Vector2 maxRanges, string objectInfo)
    {
        objectMesh = mesh;
        objectTexture = material;
        this.objectTransform = objectTransform;
        this.maxRanges = maxRanges;
        this.objectInfo = objectInfo;
    }
}

public interface IInspectable
{
    InspectableInfo InspectInfo { get; set; }
    bool IsBeingInspected { get; set; }
    InspectableInfo Inspect();
    bool StopInspect();
}