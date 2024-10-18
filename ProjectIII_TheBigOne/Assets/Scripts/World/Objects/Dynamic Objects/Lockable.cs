using System;
using Tavaris.Interfaces;
using Tavaris.Manager;
using UnityEngine;

namespace Tavaris.Dynamic
{
    [RequireComponent(typeof(LockableUnlocker))]
    public abstract class LockableObject : DynamicObject, ILockable
    {
        #region Custom Config

        public LockableSettings lockableSettings;

        [SerializeField]
        private LockState _lockState = LockState.Unlocked;
        public LockState CurrentLockState { get => _lockState; set => _lockState = value; }

        public bool IsLocked => CurrentLockState == LockState.Locked;

        public Action OnLock { get; set; }
        public Action OnUnlock { get; set; }

        LockableUnlocker attachedUnlocker;

        #endregion

        public override void Awake()
        {
            base.Awake();
            attachedUnlocker = GetComponent<LockableUnlocker>();
        }

        public override void Start()
        {
            base.Start();
            ChangeLockLimits();
        }

        public abstract void ChangeLockLimits();

        protected override void ConfigureRigidbody(Rigidbody rb)
        {
            base.ConfigureRigidbody(rb);
        }

        public override void StartInteract()
        {
            Debug.Log("Locked Start Interact");
            if (IsLocked)
            {
                // On Click then try to unlock
                Unlock();
            }
            base.StartInteract();
        }

        public override void Interacting()
        {
            base.Interacting();
        }

        public override void EndInteract()
        {
            base.EndInteract();
        }

        protected virtual bool TryToUnlock()
        {
            return attachedUnlocker.ResolveLock();
        }

        public virtual void Unlock()
        {
            if (TryToUnlock())
            {
                Debug.Log("Unlocking Object!");
                CurrentLockState = LockState.Unlocked;
                attachedUnlocker.Complete();
                OnUnlock?.Invoke();
            }
        }

        public virtual void Lock()
        {
            CurrentLockState = LockState.Locked;
            OnLock?.Invoke();
        }

        #region Unity Events
        void OnDestroy()
        {

        }

        void OnEnable()
        {
            OnLock += ChangeLockLimits;
            OnUnlock += ChangeLockLimits;
        }

        void OnDisable()
        {
            OnLock -= ChangeLockLimits;
            OnUnlock -= ChangeLockLimits;
        }
        #endregion
    }
}