﻿using Characters.Generic;
using Player;
using UnityEngine;

namespace Enemy
{
    public class State_Enemy_Idle : State
    {
        private EnemyController _attachedController;
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
            /*MovementManager.SetVelocity(_attachedRigidbody, Machine.characterController.currentBrain.Direction,
                _movementSpeed);*/
        }

        public override void OnStateFixedTick(float fixedTime)
        {
            base.OnStateFixedTick(fixedTime);
        }

        public override void OnStateCheckTransition()
        {
            base.OnStateCheckTransition();
            // TODO Add sensing utils to evaluate player distance.
            if (!_attachedController.currentBrain.IsVisible)
            {
                Machine.SwitchState<State_Enemy_Moving>();
                return;
            }
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _attachedController = (EnemyController)Machine.characterController;
            _attachedRigidbody = Machine.characterController.rigidbody;
            _movementSpeed = Machine.characterController.characterProperties.WalkSpeed;
            _attachedController.currentBrain._NavMeshAgent.isStopped = true;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}