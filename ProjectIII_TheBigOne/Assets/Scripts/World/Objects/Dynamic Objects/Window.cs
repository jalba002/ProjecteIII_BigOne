using UnityEngine;

namespace Tavaris.Dynamic
{
    public class Window : LockableObject
    {
        #region Custom Config
        public ConfigurableJoint ConfigJoint { get; set; }

        [Header("Drawer Settings")]
        [Range(-1f, 1f)] public float startClosePercentage = 0.96f;
        public bool InvertInitialization = false;

        #endregion

        #region Drawer
        [System.Serializable]
        public struct WindowHingeConfiguration
        {
            public float maximumDistance;
            [Range(0f, 100f)] public float friction;
            public float maximumForce;
        }

        [System.Serializable]
        public struct WindowLimitsConfiguration
        {
            public bool xMovement;
            public bool yMovement;
            public bool zMovement;
        }

        public WindowHingeConfiguration windowHingeConfig = new WindowHingeConfiguration()
        {
            friction = 10f,
            maximumDistance = 0.25f,
            maximumForce = 5f
        };

        public WindowLimitsConfiguration windowLimitsConfig = new WindowLimitsConfiguration()
        {

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
            newStartingPosition += (InvertInitialization ? -HandlePosition.transform.forward : HandlePosition.transform.forward) *
                                   (windowHingeConfig.maximumDistance * startClosePercentage);
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
            ConfigJoint.zDrive = new JointDrive()
            {
                maximumForce = windowHingeConfig.maximumForce,
            };

            ConfigJoint.xMotion = windowLimitsConfig.xMovement ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Locked;
            ConfigJoint.yMotion = windowLimitsConfig.yMovement ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Locked;
            ConfigJoint.zMotion = windowLimitsConfig.zMovement ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Locked;
            ConfigJoint.angularXMotion = ConfigurableJointMotion.Locked;
            ConfigJoint.angularYMotion = ConfigurableJointMotion.Locked;
            ConfigJoint.angularZMotion = ConfigurableJointMotion.Locked;
        }

        public override void ChangeLockLimits()
        {
            switch (IsLocked)
            {
                case true:
                    ConfigJoint.linearLimit = new SoftJointLimit()
                    {
                        limit = 0,
                        contactDistance = 1000f
                    };
                    break;
                case false:
                    ConfigJoint.linearLimit = new SoftJointLimit()
                    {
                        limit = windowHingeConfig.maximumDistance,
                        contactDistance = 1000f
                    };
                    break;
            }
        }

        protected override void ConfigureRigidbody(Rigidbody rb)
        {
            base.ConfigureRigidbody(rb);
            rb.drag = windowHingeConfig.friction;
        }
    }
}