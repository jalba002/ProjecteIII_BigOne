using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.HighDefinition;

namespace World.Objects
{
    public class DynamicObject : MonoBehaviour, IInteractable
    {
        [System.Serializable]
        public struct HingeConfiguration
        {
            public float maximumAngle;
            public float minimumAngle;
            [Range(0f, 100f)] public float friction;
            [Header("Forces")] public float openForce;
        }

        [System.Serializable]
        public struct DrawerHingeConfiguration
        {
            public float maximumDistance;
            [Range(0f, 100f)] public float friction;
            public float maximumForce;
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
            minimumAngle = 0f,
            openForce = 5f
        };

        public DrawerHingeConfiguration DrawerJointConfiguration = new DrawerHingeConfiguration()
        {
            friction = 10f,
            maximumDistance = 0.25f,
            maximumForce = 5f
        };

        [Header("Other Settings")] private HingeJoint HingeJoint;
        private ConfigurableJoint ConfgJoint;

        public List<Collider> ignoredColliders;
        private Collider selfCollider;
        
        [Header("Events")] public UnityEvent OnUnlock;

        public Rigidbody Rigidbody { get; protected set; }

        public void Awake()
        {
            GetRigidbody();

            selfCollider = gameObject.GetComponent<Collider>();

            GetJoints(objectType);

            GenerateIgnoredColliders(selfCollider);
            if (HandlePosition == null)
            {
                throw new NullReferenceException($"Missing Handle in {this.gameObject.name}");
            }
        }
        
        public void Start()
        {
            IgnoreColliders();
            SetJointsLimit(objectType);
            SetInitialPositions();
        }

        private void SetInitialPositions()
        {
            switch(objectType)
            {
                case ObjectType.Door:
                    
                    break;
                case ObjectType.Drawer:
                    Vector3 newStartingPosition = this.gameObject.transform.localPosition;
                    newStartingPosition.z -= DrawerJointConfiguration.maximumDistance;
                    this.gameObject.transform.localPosition = newStartingPosition;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
                        ConfigureNewJoint(HingeJoint);
                    }

                    break;
                case ObjectType.Drawer:
                    ConfgJoint = gameObject.GetComponent<ConfigurableJoint>();
                    if (ConfgJoint == null)
                    {
                        ConfgJoint = gameObject.AddComponent<ConfigurableJoint>();
                        ConfigurateNewJoint(ConfgJoint);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(objectType), objectType, null);
            }
        }

        private void GetRigidbody()
        {
            Rigidbody = GetComponent<Rigidbody>();
            if (Rigidbody == null)
            {
                Rigidbody = gameObject.AddComponent<Rigidbody>();
            }

            Rigidbody.useGravity = false;
            Rigidbody.angularDrag = 0f;
            Rigidbody.drag = objectType == ObjectType.Door
                ? DoorJointConfiguration.friction
                : DrawerJointConfiguration.friction;
            return;
        }

        private void ConfigurateNewJoint(ConfigurableJoint joint)
        {
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Limited;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            
        }

        private void ConfigureNewJoint(HingeJoint joint)
        {
            joint.anchor = new Vector3(.5f, .5f, .5f);
            joint.axis = new Vector3(0f, 1f, 0f);
            joint.useLimits = true;
            joint.useSpring = true;
            joint.autoConfigureConnectedAnchor = true;
            //joint.anchor = new Vector3(0f, -0.5f, 0f);
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
                    /*HingeJoint.spring = new JointSpring()
                    {
                        damper = DoorJointConfiguration.friction
                    };*/
                    break;
                case ObjectType.Drawer:
                    if (ConfgJoint == null) return;
                    ConfgJoint.linearLimit = new SoftJointLimit()
                    {
                        limit = DrawerJointConfiguration.maximumDistance,
                        contactDistance = 1000f
                    };
                    /*ConfgJoint.linearLimitSpring = new SoftJointLimitSpring()
                    {
                        damper = DrawerJointConfiguration.friction
                    };*/
                    ConfgJoint.zDrive = new JointDrive()
                    {
                        maximumForce = DrawerJointConfiguration.maximumForce,
                        //positionDamper = DrawerJointConfiguration.friction
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(objectType), objectType, null);
            }
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

        public bool Interact()
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

            var useForce = HandlePosition.transform.forward * CalculateForce(DoorJointConfiguration.openForce);
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
        
        public void OnStartInteract()
        {
            switch (objectType)
            {
                case ObjectType.Door:
                    //Play door sound
                    AudioManager.PlaySoundAtLocation("Sound/Door/DoorOpen_07", transform.position);
                    
                    break;
                case ObjectType.Drawer:
                    //Play drawer sound
                    break;
            }
        }

        public void OnInteracting()
        {
            throw new NotImplementedException();
        }

        public void OnEndInteract()
        {
            throw new NotImplementedException();
        }

        private float CalculateForce(float force = 1f)
        {
            var calculatedForce = 0f;
            float mouseY = Input.GetAxis("Mouse Y");
            calculatedForce = (force * mouseY);

            return calculatedForce;
        }
    }
}