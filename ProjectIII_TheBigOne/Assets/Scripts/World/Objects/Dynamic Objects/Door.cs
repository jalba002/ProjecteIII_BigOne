using UnityEngine;

namespace Tavaris.Dynamic
{
    public class Door : LockableObject
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
        public HingeJoint HingeJoint { get; private set; }
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
        protected override void GetJoint()
        {
            HingeJoint = gameObject.GetComponent<HingeJoint>();
            if (HingeJoint == null)
            {
                HingeJoint = gameObject.AddComponent<HingeJoint>();
            }
        }

        protected override void ConfigureJoint()
        {
            // Does not work well with pre-configured anchors or hinges.
            // Its better left untouched for doors.
            //HingeJoint.anchor = new Vector3(.5f, .5f, .5f);
            //HingeJoint.axis = new Vector3(0f, 1f, 0f);
            //HingeJoint.useLimits = true;
            //HingeJoint.useSpring = true;
            //HingeJoint.autoConfigureConnectedAnchor = true;
        }

        public override void ChangeLockLimits()
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
        #endregion

        #region Lockable
        
        #endregion
    }
}