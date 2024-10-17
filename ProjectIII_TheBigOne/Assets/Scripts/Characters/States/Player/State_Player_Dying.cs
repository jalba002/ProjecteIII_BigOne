using Tavaris.Manager;
using UnityEngine;

namespace Tavaris.States
{
    public class State_Player_Dying : State
    {
        private Rigidbody _attachedRigidbody;
        private float deathTime;
        private float originalValue = 3f;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedRigidbody = Machine.characterController.GetComponent<Rigidbody>();
            originalValue = 3f;
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);
            // Movement code.
            MovementManager.SetVelocity(_attachedRigidbody, Vector3.zero, 0f);
            if (deathTime > 0f)
            {
                deathTime -= deltaTime;
                // Fill screen with black stuff.
                // deathTime / originalValue
            }
            else if (deathTime <= 0f)
            {
                // Do stuff.
                // Certainly die.
                Machine.SwitchState<State_Player_Dead>();
            }
        }

        public override void OnStateFixedTick(float fixedTime)
        {
            base.OnStateFixedTick(fixedTime);
        }

        public override void OnStateCheckTransition()
        {
            base.OnStateCheckTransition();
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _attachedRigidbody.velocity = Vector3.zero;
            _attachedRigidbody.angularVelocity = Vector3.zero;
            // play required animations.
            deathTime = originalValue;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}