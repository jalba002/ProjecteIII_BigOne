﻿using System;
using Tavaris.Interfaces;
using UnityEngine;

namespace Tavaris.Dynamic
{
    public class Drawer : LockableObject
    {
        #region Custom Config
        public ConfigurableJoint ConfigJoint { get; set; }

        [Header("Drawer Settings")]
        [Range(0f, 1f)] public float startClosePercentage = 0.96f;
        public bool InvertInitialization = false;

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
                                   (drawerConfiguration.maximumDistance * startClosePercentage);
            this.gameObject.transform.localPosition = newStartingPosition;
        }

        protected override void GetJoint()
        {
            ConfigJoint = gameObject.GetComponent<ConfigurableJoint>();
            if (ConfigJoint == null)
            {
                ConfigJoint = gameObject.AddComponent<ConfigurableJoint>();
            }
        }
        protected override void ConfigureJoint()
        {
            // This is the default joint setting for every object.
            // This allows to have different objects with different behaviours
            // for example, a window will behave differently than a drawer.
            // maybe we can extrapolate this configuration?

            ConfigJoint.xMotion = ConfigurableJointMotion.Locked;
            ConfigJoint.yMotion = ConfigurableJointMotion.Locked;
            ConfigJoint.zMotion = ConfigurableJointMotion.Limited;
            ConfigJoint.angularXMotion = ConfigurableJointMotion.Locked;
            ConfigJoint.angularYMotion = ConfigurableJointMotion.Locked;
            ConfigJoint.angularZMotion = ConfigurableJointMotion.Locked;
        }

        public override void ChangeLockLimits()
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

        protected override void ConfigureRigidbody(Rigidbody rb)
        {
            base.ConfigureRigidbody(rb);
            rb.drag = drawerConfiguration.friction;
        }
    }
}