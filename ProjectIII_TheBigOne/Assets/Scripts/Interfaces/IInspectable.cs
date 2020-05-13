using UnityEngine;


[System.Serializable]
public struct InspectableInfo
{
    public Mesh objectMesh;
    public Material[] objectTexture;
    public Transform objectTransform;

    public InspectableInfo(Mesh mesh, Material[] material, Transform objectTransform)
    {
        objectMesh = mesh;
        objectTexture = material;
        this.objectTransform = objectTransform;
    }
}

public interface IInspectable
{
    InspectableInfo InspectInfo { get; set; }
    bool IsBeingInspected { get; set; }
    InspectableInfo Inspect();
    bool StopInspect();
}