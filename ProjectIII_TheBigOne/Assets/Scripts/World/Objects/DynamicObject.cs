using System;
using System.Collections.Generic;
using System.Linq;
using Tavaris.Interactable;
using UnityEngine;
using UnityEngine.AI;

namespace Tavaris.Dynamic
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class DynamicObject : InteractableObject
    {
        #region Declarations

        // If the door is closed and must be opened by turning the handle.
        public enum OpenState
        {
            Closed,
            Opened
        }

        #endregion

        #region MainConfiguration

        private OpenState _openState;
        protected OpenState CurrentOpenState
        {
            get { return _openState; }
        }

        #endregion

        [Header("Hinge Settings", order = 1)] 
        public GameObject HandlePosition;
        public bool applyHandleRotation = false;
        public float openForce = 5f;
        [Range(1f, 15f)] public float maxMouseInput = 3f;
        public bool UseMouseXAxis = false;

        [Header("Rigidbody Settings")]
        public bool useGravity = false;

        [Space(2)]
        [Header("Collision Settings")]
        public bool ignorePlayerCollider = false;

        public List<Collider> ignoredColliders;

        private Collider selfCollider;
        private Quaternion originalHandleRotation;

        private Rigidbody rb;
        public Rigidbody rigidbody
        {
            get
            {
                if(rb == null) rb = GetComponent<Rigidbody>();
                return rb;
            }
            private set
            {
                rb = value;
                ConfigureRigidbody(rb);
            }
        }

        public virtual void Awake()
        {
            selfCollider = gameObject.GetComponent<Collider>();

            GetJoint();

            if (HandlePosition == null)
            {
                HandlePosition = GetComponentsInChildren<GameObject>().ToList().Find(x => x.name.ToLower().Contains("handle"));
            }
        }

        public virtual void Start()
        {
            IgnoreColliders();
            ConfigureJoint();
            SetInitialPositions();
            originalHandleRotation = HandlePosition.transform.rotation;
        }

        protected virtual void SetInitialPositions()
        {

        }

        private void GetRigidbody()
        {
            rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = gameObject.AddComponent<Rigidbody>();
            }
        }

        protected virtual void ConfigureRigidbody(Rigidbody rb)
        {
            rigidbody.useGravity = useGravity;
            rigidbody.angularDrag = 0f;
        }

        #region Joints

        protected abstract void GetJoint();
        protected abstract void ConfigureJoint();

        #endregion

        #region IgnoreColliders

        private void IgnoreColliders()
        {
            if (ignorePlayerCollider) ignoredColliders.Add(GameManager.Player.attachedCollider);

            foreach (Collider currentCollider in ignoredColliders)
            {
                if (currentCollider == null) break;
                if (showLogDebug)
                    Debug.Log($"Ignoring {currentCollider.gameObject.name} with {selfCollider.gameObject.name}");
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
                Interacting();
            }
        }

        // Interface Implementation //
        public bool ForceClose()
        {
            throw new NotImplementedException();
        }

        public virtual void BreakJoint()
        {
            Destroy(GetComponent<NavMeshObstacle>());
            rigidbody.drag = 0f;
            this.gameObject.layer = 0;
        }
        #endregion

        public override bool Interact(bool interactEnable)
        {
            if (!interactEnable)
            {
                EndInteract();
                return true;
            }

            if (!IsInteracting)
            {
                StartInteract();
                return true;
            }

            return false;
        }

        #region OnInteractions

        public override void StartInteract()
        {
            base.StartInteract();
            GetRigidbody();
            SetHandleDirection(GameManager.Player.transform.position);
        }

        public override void Interacting()
        {
            base.Interacting();
            ForceOpen(CalculateForce(openForce));
        }

        public override void EndInteract()
        {
            base.EndInteract();
        }

        #endregion

        protected virtual void SetHandleDirection(Vector3 position)
        {
            if (!applyHandleRotation) return;

            // Maybe do this with the handle transform?
            Transform objectTransform = this.transform;
            Vector3 forwardVector = objectTransform.forward;
            Plane directionPlane = new Plane(forwardVector, objectTransform.position);
            bool playerOnSide = directionPlane.GetSide(position);

            HandlePosition.transform.forward = playerOnSide ? -forwardVector : forwardVector;
        }

        public void ResetHandle()
        {
            HandlePosition.transform.rotation = originalHandleRotation;
        }

        private float CalculateForce(float forceScale = 1f)
        {
            // TODO: Move to new Input and handle on Player Controller / Camera
            float calculatedForce = 1f;
            float mouseAxis = UseMouseXAxis ? Input.GetAxis("Mouse X") : Input.GetAxis("Mouse Y");
            mouseAxis = Mathf.Clamp(mouseAxis, -maxMouseInput, maxMouseInput);
            calculatedForce = (forceScale * mouseAxis) * OptionsManager.Instance.sensibility;

            return calculatedForce;
        }

        public void ForceOpen(float force)
        {
            var useForce = HandlePosition.transform.forward * force;
            rigidbody.AddForceAtPosition(useForce, HandlePosition.transform.position, ForceMode.Force);
        }

        #region Opening

        public void StrongOpening()
        {
            var useForce = HandlePosition.transform.forward * (rigidbody.mass * openForce);
            rigidbody.AddForceAtPosition(useForce, HandlePosition.transform.position, ForceMode.Impulse);
        }

        public void BreakOpening(Vector3 direction, float force = 10f)
        {
            var useForce = direction * (rigidbody.mass * force);
            rigidbody.AddForceAtPosition(useForce, HandlePosition.transform.position, ForceMode.Impulse);
        }

        public void StrongClosing()
        {
            var useForce = HandlePosition.transform.forward * (rigidbody.mass * openForce);
            rigidbody.AddForceAtPosition(-useForce, HandlePosition.transform.position, ForceMode.Impulse);
        }

        #endregion
    }
}