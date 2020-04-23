using Characters.Generic;
using UnityEngine;

namespace Player
{
    public class State_Enemy_Idle : State
    {
        private float _movementSpeed;
        private Rigidbody _attachedRigidbody;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);
            
            // Movement code.
            MovementManager.SetVelocity(_attachedRigidbody, Machine.characterController.currentBrain.Direction,
                _movementSpeed);
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
            _attachedRigidbody = Machine.characterController.rigidbody;

            _movementSpeed = Machine.characterController.characterProperties.WalkSpeed;
           }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}

