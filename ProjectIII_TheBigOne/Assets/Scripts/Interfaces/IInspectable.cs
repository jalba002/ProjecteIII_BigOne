using UnityEngine;

namespace Interfaces
{
    [System.Serializable]
    public struct InspectableInfo
    {
        public Mesh objectMesh;
        public Material[] objectTexture;
        public Quaternion objectRotation;

        public InspectableInfo(Mesh mesh, Material[] material, Quaternion rObjectRotation)
        {
            objectMesh = mesh;
            objectTexture = material;
            objectRotation = rObjectRotation;
        }
    }
    public interface IInspectable
    {
        InspectableInfo InspectInfo { get; set; }
        bool IsBeingInspected { get; set; }
        InspectableInfo Inspect();
        bool StopInspect();
    }
}