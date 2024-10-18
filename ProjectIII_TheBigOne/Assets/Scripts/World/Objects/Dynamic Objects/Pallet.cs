using UnityEngine;

namespace Tavaris.Dynamic
{
    public class Pallet : DynamicObject
    {
        #region Pallet Structs
        [System.Serializable]
        public struct PalletConfiguration
        {
            public float forceToThrow;
            public float forceScaleApplied;
        }
        #endregion

        #region Pallet Variables
        public PalletConfiguration palletConfiguration = new PalletConfiguration()
        {
            forceToThrow = 100f,
            forceScaleApplied = 5f
        };
        public HingeJoint HingeJoint { get; private set; }
        #endregion

        protected override void ConfigureRigidbody(Rigidbody rb)
        {
            base.ConfigureRigidbody(rb);
            //Nothing atm. Others configure drag.
        }

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
            HingeJoint.anchor = new Vector3(.5f, .5f, .5f);
            HingeJoint.axis = new Vector3(0f, 1f, 0f);
            HingeJoint.useLimits = true;
            HingeJoint.useSpring = true;
            HingeJoint.autoConfigureConnectedAnchor = true;
        }
        #endregion
    }
}