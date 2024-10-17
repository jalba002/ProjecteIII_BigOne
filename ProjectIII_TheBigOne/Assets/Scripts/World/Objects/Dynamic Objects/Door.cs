using System;
using Tavaris.Interfaces;
using UnityEngine;
using static TMPro.Examples.TMP_ExampleScript_01;

namespace Tavaris.Dynamic
{
    public class Door : DynamicObject, ILockable
    {
        #region Door
        [System.Serializable]
        public struct DoorHingeConfiguration
        {
            [Header("Unlocked Settings")]
            public float maximumAngle;
            public float minimumAngle;
            [Header("Locked Settings")]
            public float minimumLockedAngle;
            public float maximumLockedAngle;
            [Header("General Settings")]
            [Range(0f, 100f)] public float friction;
            [Header("Bounciness")]
            public float bounciness;
            public float bouncinessMinVelocity;
        }

        public DoorHingeConfiguration doorConfiguration = new DoorHingeConfiguration()
        {
            friction = 1f,
            maximumAngle = 90f,
            minimumAngle = 0f,
            minimumLockedAngle = -2f,
            maximumLockedAngle = 2f,
            bounciness = 0f,
            bouncinessMinVelocity = 0f,
        };
        #endregion

        #region Door Variables
        //[Header($"{nameof(Door)} configuration.")]
        public HingeJoint HingeJoint { get; private set; }

        [SerializeField]
        private LockState _lockState = LockState.Unlocked;
        public LockState CurrentLockState { get => _lockState; set => _lockState = value; }
        // Expose the property to edit in the editor?

        public bool IsLocked => _lockState == LockState.Locked;

        public Action OnLock { get; set; }
        public Action OnUnlock { get; set; }

        #endregion

        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            base.Start();
        }

        #region Rigidbody
        protected override void ConfigureRigidbody(Rigidbody rb)
        {
            base.ConfigureRigidbody(rb);
            rb.drag = doorConfiguration.friction;
        }
        #endregion

        #region Joints
        protected override void GetJoints()
        {
            HingeJoint = gameObject.GetComponent<HingeJoint>();
            if (HingeJoint == null)
            {
                HingeJoint = gameObject.AddComponent<HingeJoint>();
                ConfigureNewJoint(HingeJoint);
            }
        }

        protected override void SetInitialPositions()
        {

        }

        protected override void CheckObjectOpening()
        {

        }

        protected override void SetJointsLimit()
        {
            switch (IsLocked)
            {
                case true:
                    HingeJoint.limits = new JointLimits()
                    {
                        max = doorConfiguration.maximumLockedAngle,
                        min = doorConfiguration.minimumLockedAngle,
                        bounciness = doorConfiguration.bounciness,
                        bounceMinVelocity = doorConfiguration.bouncinessMinVelocity                        
                    };
                    break;
                case false:
                    HingeJoint.limits = new JointLimits()
                    {
                        max = doorConfiguration.maximumAngle,
                        min = doorConfiguration.minimumAngle,
                        bounciness = doorConfiguration.bounciness,
                        bounceMinVelocity = doorConfiguration.bouncinessMinVelocity
                    };
                    break;
            }
        }

        public void Unlock()
        {
            throw new NotImplementedException();
        }

        public void Lock()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}