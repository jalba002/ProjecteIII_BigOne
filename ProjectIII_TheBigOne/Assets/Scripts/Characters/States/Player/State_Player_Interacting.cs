using System.Diagnostics;
using Characters.Generic;
using UnityEngine;
using CharacterController = UnityEngine.CharacterController;

namespace Player
{
    public class State_Player_Interacting : State
    {
        private Rigidbody _attachedRigidbody;
        private PlayerController _playerController;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedRigidbody = Machine.characterController.rigidbody;
            _playerController = (PlayerController) Machine.characterController;
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
            if (_playerController.interactablesManager.CurrentInteractable == null ||
                !_playerController.interactablesManager.CurrentInteractable.IsInteracting)
            {
                _playerController.stateMachine.SwitchState<State_Player_Walking>();
            }
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _playerController.cameraController.angleLocked = true;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _playerController.cameraController.angleLocked = false;
        }
    }
}