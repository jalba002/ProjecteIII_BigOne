using System;
using System.Collections.Generic;
using System.Xml;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.HighDefinition;

namespace World.Objects
{
    public class DynamicObject : MonoBehaviour, IInteractable, IMovable
    {
        #region Declarations
        [System.Serializable]
        public struct DoorHingeConfiguration
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
        public DoorHingeConfiguration doorConfiguration = new DoorHingeConfiguration()
        {
            friction = 1f,
            maximumAngle = 90f,
            minimumAngle = 0f,
            openForce = 5f
        };

        public DrawerHingeConfiguration drawerConfiguration = new DrawerHingeConfiguration()
        {
            friction = 10f,
            maximumDistance = 0.25f,
            maximumForce = 5f
        };
        
        #endregion
        
        public LockedMode lockedMode;
        public OpenState openState { get; set; }

        [Header("Hinge Settings", order = 1)] public GameObject HandlePosition;

        [Header("Other Settings")] private HingeJoint HingeJoint;
        private ConfigurableJoint ConfgJoint;

        public List<Collider> ignoredColliders;
        private Collider selfCollider;
        
        [Header("Events")] public UnityEvent OnUnlockEvent = new UnityEvent();
        public UnityEvent OnStartInteracting = new UnityEvent();

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

            DisplayName = $"Drag {objectType.ToString()}";
        }

        public void Start()
        {
            IgnoreColliders();
            SetJointsLimit(lockedMode);
            SetInitialPositions();
            OnUnlockEvent.AddListener(OnUnlock);
        }

        private void SetInitialPositions()
        {
            switch (objectType)
            {
                case ObjectType.Door:

                    break;
                case ObjectType.Drawer:
                    Vector3 newStartingPosition = this.gameObject.transform.localPosition;
                    newStartingPosition.z -= (drawerConfiguration.maximumDistance * 0.9f);
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
                ? doorConfiguration.friction
                : drawerConfiguration.friction;
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

        private void SetJointsLimit(LockedMode lockMode)
        {
            switch (lockMode)
            {
                case LockedMode.Locked:
                    switch (objectType)
                    {
                        case ObjectType.Door:
                            if (HingeJoint == null) return;
                            HingeJoint.limits = new JointLimits()
                            {
                                max = 2f,
                                min = -2f
                            };
                            break;
                        case ObjectType.Drawer:
                            
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(objectType), objectType, null);
                    }

                    break;
                case LockedMode.Unlocked:
                    switch (objectType)
                    {
                        case ObjectType.Door:
                            if (HingeJoint == null) return;
                            HingeJoint.limits = new JointLimits()
                            {
                                max = doorConfiguration.maximumAngle,
                                min = doorConfiguration.minimumAngle
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
                                limit = drawerConfiguration.maximumDistance,
                                contactDistance = 1000f
                            };
                            /*ConfgJoint.linearLimitSpring = new SoftJointLimitSpring()
                            {
                                damper = DrawerJointConfiguration.friction
                            };*/
                            ConfgJoint.zDrive = new JointDrive()
                            {
                                maximumForce = drawerConfiguration.maximumForce,
                                //positionDamper = DrawerJointConfiguration.friction
                            };
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(objectType), objectType, null);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            lockedMode = lockMode;
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
            SetJointsLimit(lockedMode);
        }

        void Update()
        {
            if (IsInteracting)
            {
                OnInteracting();
            }
        }

        // Interface Implementation //

        public string DisplayName { get; set; }

        public GameObject attachedGameobject
        {
            get { return gameObject;}
        }

        public bool IsInteracting { get; set; }
        
        public bool ForceClose()
        {
            throw new NotImplementedException();
        }

        public bool Unlock()
        {
            SetJointsLimit(LockedMode.Unlocked);
            OnUnlockEvent.Invoke();
            return true;
        }

        public void SetUnlock()
        {
            Unlock();
        }

        private void OnUnlock()
        {
            AudioManager.PlaySound2D("Sound/Door/Unlock01");
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

        public bool Interact(bool interactEnable)
        {
            if (!interactEnable)
            {
                Debug.Log("Ending Interaction");
                OnEndInteract();
                return true;
            }
            if (!IsInteracting)
            {
                OnStartInteract();
                Debug.Log("Starting Interaction");
                return true;
            }

            return false;
        }

        public void OnStartInteract()
        {
            OnStartInteracting.Invoke();
            
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

            IsInteracting = true;
        }

        public void OnInteracting()
        {
            var useForce = HandlePosition.transform.forward * CalculateForce(doorConfiguration.openForce);
            Rigidbody.AddForceAtPosition(useForce, HandlePosition.transform.position, ForceMode.Force);
        }

        public void OnEndInteract()
        {
            IsInteracting = false;
            Debug.Log($"Setting {this.gameObject.name} interaction to {IsInteracting}");
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