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

        protected override void GetJoints()
        {
            HingeJoint = gameObject.GetComponent<HingeJoint>();
            if (HingeJoint == null)
            {
                HingeJoint = gameObject.AddComponent<HingeJoint>();
                ConfigureNewJoint(HingeJoint);
            }
        }

        protected override void SetInitialPositions()
        {

        }

        protected override void CheckObjectOpening()
        {

        }

        protected override void SetJointsLimit()
        {

        }
    }
}