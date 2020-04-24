using UnityEngine;

namespace Interfaces
{
    [System.Serializable]
    public struct InspectableInfo
    {
        public Mesh objectMesh;
        public Material[] objectTexture;

        public InspectableInfo(Mesh mesh, Material[] material)
        {
            objectMesh = mesh;
            objectTexture = material;
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