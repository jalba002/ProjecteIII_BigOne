using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;

namespace World.Objects
{
    public class DynamicObject : MonoBehaviour, IMovable
    {
        [System.Serializable]
        public struct HingeConfiguration
        {
            public float maximumAngle;
            public float minimumAngle;
            [Range(1f, 10f)] public float friction;
        }

        [System.Serializable]
        public struct DrawerHingeConfiguration
        {
            public float maximumDistance;
            [Range(1f, 9999f)] public float friction;
        }

        // Object type.
        // Changes the hinge used.
        public enum ObjectType
        {
            Door,
            Drawer
        }

        // If the door is closed by a key or not.
        public enum LockedMode
        {
            Locked,
            Unlocked
        }

        // If the door is closed and must be opened by turning the handle.
        public enum OpenState
        {
            Closed,
            Opened
        }

        [Header("Main Configuration", order = 0)]
        public ObjectType objectType;

        public LockedMode lockedMode;
        public OpenState openState { get; set; }

        [Header("Hinge Settings", order = 1)] public GameObject HandlePosition;

        public HingeConfiguration DoorJointConfiguration = new HingeConfiguration()
        {
            friction = 1f,
            maximumAngle = 90f,
            minimumAngle = 0f
        };

        public DrawerHingeConfiguration DrawerJointConfiguration = new DrawerHingeConfiguration()
        {
            friction = 9999f,
            maximumDistance = 0.25f
        };

        [Header("Other Settings")] private HingeJoint HingeJoint;
        private ConfigurableJoint ConfgJoint;

        public List<Collider> ignoredColliders;
        private Collider selfCollider;

        [Header("Events")] public UnityEvent OnUnlock;

        public Rigidbody Rigidbody { get; protected set; }

        public void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            selfCollider = gameObject.GetComponent<Collider>();

            GetJoints(objectType);

            GenerateIgnoredColliders(selfCollider);
            if (HandlePosition == null)
            {
                throw new NullReferenceException($"Missing Handle in {this.gameObject.name}");
            }
        }

        private void GetJoints(ObjectType objectType)
        {
            switch (objectType)
            {
                case ObjectType.Door:
                    HingeJoint = gameObject.GetComponent<HingeJoint>();
                    if (HingeJoint == null)
                    {
                        HingeJoint = gameObject.AddComponent<HingeJoint>();
                    }

                    break;
                case ObjectType.Drawer:
                    ConfgJoint = gameObject.GetComponent<ConfigurableJoint>();
                    if (ConfgJoint == null)
                    {
                        ConfgJoint = gameObject.AddComponent<ConfigurableJoint>();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(objectType), objectType, null);
            }
        }

        private ConfigurableJoint GetDrawerJoint()
        {
            ConfigurableJoint joint = new ConfigurableJoint()
            {
                
            };
            return joint;
        }

        private void SetJointsLimit(ObjectType objectType)
        {
            switch (objectType)
            {
                case ObjectType.Door:
                    if (HingeJoint == null) return;
                    HingeJoint.limits = new JointLimits()
                    {
                        max = DoorJointConfiguration.maximumAngle,
                        min = DoorJointConfiguration.minimumAngle
                    };
                    HingeJoint.spring = new JointSpring()
                    {
                        damper = DoorJointConfiguration.friction
                    };
                    break;
                case ObjectType.Drawer:
                    if (ConfgJoint == null) return;
                    ConfgJoint.linearLimit = new SoftJointLimit()
                    {
                        limit = DrawerJointConfiguration.maximumDistance
                    };
                    ConfgJoint.linearLimitSpring = new SoftJointLimitSpring()
                    {
                        damper = DrawerJointConfiguration.friction
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(objectType), objectType, null);
            }
        }

        public void Start()
        {
            IgnoreColliders();
            SetJointsLimit(objectType);
        }

        private bool GenerateIgnoredColliders(Collider selfCollider)
        {
            try
            {
                Collider[] collectedColliders = gameObject.GetComponentsInChildren<Collider>();
                foreach (Collider collider in collectedColliders)
                {
                    if (collider.GetHashCode() != selfCollider.GetHashCode())
                    {
                        ignoredColliders.Add(collider);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Ignored colliders generated wrongly in ", this.gameObject);
                return false;
            }

            return true;
        }

        private void IgnoreColliders()
        {
            foreach (Collider currentCollider in ignoredColliders)
            {
                if (currentCollider == null) break;
                Physics.IgnoreCollision(currentCollider, selfCollider, true);
            }
        }

        //Update hinge variables when inspector is modified.
        private void OnValidate()
        {
            SetJointsLimit(objectType);
        }

        // Interface Implementation //

        public bool Use(float force)
        {
            if (Rigidbody == null)
            {
                Rigidbody = GetComponent<Rigidbody>();
                if (!Rigidbody)
                {
                    Debug.LogWarning("No rigidbody was found. Adding a new one.");
                    Rigidbody = gameObject.AddComponent<Rigidbody>();
                    Rigidbody.useGravity = false;
                }
            }

            var useForce = HandlePosition.transform.forward * force;
            Rigidbody.AddForceAtPosition(useForce, HandlePosition.transform.position, ForceMode.Force);
            return true;
        }

        public bool ForceClose()
        {
            throw new NotImplementedException();
        }

        public bool Unlock()
        {
            throw new NotImplementedException();
        }

        public bool Lock()
        {
            throw new NotImplementedException();
        }

        public bool OnClose()
        {
            throw new NotImplementedException();
        }

        public bool OnOpen()
        {
            throw new NotImplementedException();
        }
    }
}