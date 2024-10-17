using System;
using Tavaris.Interfaces;
using UnityEngine;

namespace Tavaris.Dynamic
{
    public class Drawer : DynamicObject, ILockable
    {
        #region Custom Config
        public ConfigurableJoint ConfigJoint { get; set; }

        [SerializeField]
        private LockState _lockState = LockState.Unlocked;
        public LockState CurrentLockState { get => _lockState; set => _lockState = value; }

        public bool IsLocked => CurrentLockState == LockState.Locked;

        public Action OnLock { get; set; }
        public Action OnUnlock { get; set; }
        #endregion

        #region Drawer
        [System.Serializable]
        public struct DrawerHingeConfiguration
        {
            public float maximumDistance;
            [Range(0f, 100f)] public float friction;
            public float maximumForce;
        }

        public DrawerHingeConfiguration drawerConfiguration = new DrawerHingeConfiguration()
        {
            friction = 10f,
            maximumDistance = 0.25f,
            maximumForce = 5f
        };
        #endregion

        public override void Start()
        {
            base.Start();
        }

        protected override void SetInitialPositions()
        {
            Vector3 newStartingPosition = this.gameObject.transform.localPosition;
            Vector3 forward =
                HandlePosition.transform.InverseTransformDirection(HandlePosition.transform.forward);
            newStartingPosition += (InvertInitialization ? -forward : forward) *
                                   (drawerConfiguration.maximumDistance * StartupClosePercentage);
            this.gameObject.transform.localPosition = newStartingPosition;
        }

        protected override void SetJointsLimit()
        {
            switch (CurrentLockState)
            {
                case LockState.Locked:


                    break;
                case LockState.Unlocked:
                    if (ConfigJoint == null) return;
                    ConfigJoint.linearLimit = new SoftJointLimit()
                    {
                        limit = drawerConfiguration.maximumDistance,
                        contactDistance = 1000f
                    };
                    /*ConfgJoint.linearLimitSpring = new SoftJointLimitSpring()
                    {
                        damper = DrawerJointConfiguration.friction
                    };*/
                    ConfigJoint.zDrive = new JointDrive()
                    {
                        maximumForce = drawerConfiguration.maximumForce,
                        //positionDamper = DrawerJointConfiguration.friction
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void GetJoints()
        {
            ConfigJoint = gameObject.GetComponent<ConfigurableJoint>();
            if (ConfigJoint == null)
            {
                ConfigJoint = gameObject.AddComponent<ConfigurableJoint>();
                ConfigureNewJoint(ConfigJoint);
            }
        }

        protected override void ConfigureRigidbody(Rigidbody rb)
        {
            base.ConfigureRigidbody(rb);
            rb.drag = drawerConfiguration.friction;
        }

        protected override void CheckObjectOpening()
        {
            
        }

        public void Unlock()
        {
            
        }

        public void Lock()
        {
            
        }
    }
}