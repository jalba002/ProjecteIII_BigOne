using UnityEngine;

namespace Tavaris.Manager
{
    internal abstract class LockableUnlocker : MonoBehaviour
    {
        public abstract bool ResolveLock();
        public abstract void Complete();
    }
}