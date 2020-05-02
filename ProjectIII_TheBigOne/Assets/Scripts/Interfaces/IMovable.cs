using UnityEngine;

namespace Interfaces
{
    public interface IMovable
    {
        bool Use(float force);
        void OnStartInteract();
    }
}
