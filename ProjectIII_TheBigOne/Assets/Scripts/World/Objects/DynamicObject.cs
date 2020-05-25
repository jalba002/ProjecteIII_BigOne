using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace World.Objects
{
    public class DynamicObject : InteractableObject
    {
        #region Declarations

        [System.Serializable]
        public struct DoorHingeConfiguration
        {
            public float maximumAngle;
            public float minimumAngle;
            [Range(0f, 100f)] public float friction;
        }

        [System.Serializable]
        public struct DrawerHingeConfiguration
        {
            public float maximumDistance;
            [Range(0f, 100f)] public float friction;
            public float maximumForce;
        }

        [System.Serializable]
        public struct PalletConfiguration
        {
            public float forceToThrow;
            public float forceScaleApplied;
        }

        // Object type.
        // Changes the hinge used.
        public enum ObjectType
        {
            Door,
            Drawer,
            Pallet
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

        #endregion

        #region MainConfiguration

        [Header("Main Configuration", order = 0)]
        public ObjectType objectType;

        public LockedMode lockedMode;

        public DoorHingeConfiguration doorConfiguration = new DoorHingeConfiguration()
        {
            friction = 1f,
            maximumAngle = 90f,
            minimumAngle = 0f,
        };

        public DrawerHingeConfiguration drawerConfiguration = new DrawerHingeConfiguration()
        {
            friction = 10f,
            maximumDistance = 0.25f,
            maximumForce = 5f
        };

        public PalletConfiguration palletConfiguration = new PalletConfiguration()
        {
            forceToThrow = 100f,
            forceScaleApplied = 5f
        };

        public OpenState openState { get; set; }

        #endregion

        [Header("Hinge Settings", order = 1)] public GameObject HandlePosition;
        public float openForce = 5f;
        [Range(1f, 15f)] public float maxMouseInput = 3f;
        public bool UseMouseXAxis = false;
        public bool InvertInitialization = false;
        [Range(0f, 1f)] public float StartupClosePercentage = 0.96f;

        [Header("Rigidbody Settings")] public bool useGravity = false;

        [Space(2)] [Header("Other Settings")] public List<Collider> ignoredColliders;
        private Collider selfCollider;

        // Joints.
        public HingeJoint HingeJoint { get; set; }
        public ConfigurableJoint ConfgJoint { get; set; }

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
                case ObjectType.Drawer:
                    Vector3 newStartingPosition = this.gameObject.transform.localPosition;
                    Vector3 forward = HandlePosition.transform.InverseTransformDirection(HandlePosition.transform.forward);
                    newStartingPosition += (InvertInitialization ? -forward : forward) * (drawerConfiguration.maximumDistance * StartupClosePercentage);
                    this.gameObject.transform.localPosition = newStartingPosition;
                    break;
                case ObjectType.Door:
                    break;
                case ObjectType.Pallet:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void GetRigidbody()
        {
            Rigidbody = GetComponent<Rigidbody>();
            if (Rigidbody == null)
            {
                Rigidbody = gameObject.AddComponent<Rigidbody>();
            }

            Rigidbody.useGravity = useGravity;
            Rigidbody.angularDrag = 0f;
            Rigidbody.drag = objectType == ObjectType.Door
                ? doorConfiguration.friction
                : (objectType == ObjectType.Drawer ? drawerConfiguration.friction : Rigidbody.drag);
            return;
        }

        #region Joints

        private void GetJoints(ObjectType objectType)
        {
            switch (objectType)
            {
                case ObjectType.Door:
                case ObjectType.Pallet:
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
                        case ObjectType.Pallet:
                            // Debug.LogWarning("No pallet configurations!");
                            /*HingeJoint.limits = new JointLimits()
                            {
                                
                            }*/
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
        #endregion

        #region IgnoreColliders

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

        #endregion

        #region Interactable

        public override void UpdateInteractable()
        {
            base.UpdateInteractable();
            if (IsInteracting)
            {
                OnInteracting();
            }
        }

        // Interface Implementation //
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
            // TODO Play sound depending on the type? 
            AudioManager.PlaySound2D("Sound/Door/Unlock01");
        }

        public void BreakJoint()
        {
            Destroy(HingeJoint);
            Destroy(GetComponent<NavMeshObstacle>());
            Rigidbody.drag = 0f;
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

        #endregion

        public override bool Interact(bool interactEnable)
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

        #region OnInteractions
        public override void OnStartInteract()
        {
            base.OnStartInteract();
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
                case ObjectType.Pallet:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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

        public override void OnInteracting()
        {
            base.OnInteracting();
            ForceOpen(CalculateForce(openForce));
        }
        
        public override void OnEndInteract()
        {
            IsInteracting = false;
            Debug.Log($"Setting {this.gameObject.name} interaction to {IsInteracting}");
        }
        #endregion

        private float CalculateForce(float forceScale = 1f)
        {
            float calculatedForce = 1f;
            float mouseAxis = UseMouseXAxis ? Input.GetAxis("Mouse X") : Input.GetAxis("Mouse Y");
            mouseAxis = Mathf.Clamp(mouseAxis, -maxMouseInput, maxMouseInput);
            calculatedForce = (forceScale * mouseAxis) * OptionsManager.optionsData.sensitivity;
            
            return calculatedForce;
        }

        public void ForceOpen(float force)
        {
            var useForce = HandlePosition.transform.forward * force;
            Rigidbody.AddForceAtPosition(useForce, HandlePosition.transform.position, ForceMode.Force);
        }

        #region Opening
        public void StrongOpening()
        {
            var useForce = HandlePosition.transform.forward * (Rigidbody.mass * openForce);
            Rigidbody.AddForceAtPosition(useForce, HandlePosition.transform.position, ForceMode.Impulse);
        }

        public void StrongClosing()
        {
            var useForce = HandlePosition.transform.forward * (Rigidbody.mass * openForce);
            Rigidbody.AddForceAtPosition(-useForce, HandlePosition.transform.position, ForceMode.Impulse);
        }
        #endregion
        
        //Update hinge variables when inspector is modified.
        private void OnValidate()
        {
            SetJointsLimit(lockedMode);
        }
    }
}