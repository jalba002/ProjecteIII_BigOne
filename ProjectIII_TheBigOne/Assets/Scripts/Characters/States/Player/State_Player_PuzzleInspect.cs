using Tavaris.Manager;
using UnityEngine;

namespace Tavaris.States
{
    public class State_Player_PuzzleInspect : State
    {
        private Rigidbody _attachedRigidbody;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedRigidbody = Machine.characterController.GetComponent<Rigidbody>();
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);
            // Movement code.
            MovementManager.SetVelocity(_attachedRigidbody, Vector3.zero, 0f);
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
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}