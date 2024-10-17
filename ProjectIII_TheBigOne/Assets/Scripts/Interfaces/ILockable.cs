using System;

namespace Tavaris.Interfaces
{
    public enum LockState
    {
        Unlocked,
        Locked
    }

    public interface ILockable
    {
        LockState CurrentLockState { get; set; }
        bool IsLocked { get; }
        Action OnLock { get; set; }
        Action OnUnlock { get; set; }
        void Unlock();
        void Lock();
    }
}