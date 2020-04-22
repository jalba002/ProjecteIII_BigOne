

using UnityEngine;

namespace Interfaces
{
    public interface IInspectable
    {
        bool IsBeingInspected { get; set; }
        bool Inspect();
        bool StopInspect();
    }
}