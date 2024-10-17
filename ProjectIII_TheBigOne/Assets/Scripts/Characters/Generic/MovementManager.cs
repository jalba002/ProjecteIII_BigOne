using System;
using UnityEngine;

namespace Tavaris.Manager
{
    // This class handles all movements. No movement is programmed in states.
    public static class MovementManager
    {
        public static void MoveWithVelocity(Rigidbody rigidbody, Vector3 direction, Vector3 force, ForceMode forceMode)
        {
        }

        public static void MoveWithVelocity(Rigidbody rigidbody, Vector3 direction, float force = 1f,
            ForceMode forceMode = ForceMode.VelocityChange)
        {
        }


        /// <summary>
        /// Instantly sets the velocity for a rigidbody.
        /// </summary>
        /// <param name="rigidbody"></param>
        /// <param name="direction"></param>
        /// <param name="force"></param>
        public static void SetVelocity(Rigidbody rigidbody, Vector3 direction, float force = 1f)
        {
            try
            {
                Vector3 newVelocity = rigidbody.transform.TransformDirection(direction);
                newVelocity *= force;

                rigidbody.velocity = new Vector3(newVelocity.x, rigidbody.velocity.y, newVelocity.z);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}