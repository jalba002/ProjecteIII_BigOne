using System;
using Characters.Generic;
using Player;
using UnityEngine;

namespace Enemy
{
    public class State_Enemy_Lighted : State
    {
        private EnemyController _attachedController;

        private float lastFOV;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = (EnemyController) Machine.characterController;
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);

            _attachedController.CheckForPlayerNearLight();

            _attachedController.CheckForPlayerOnSight();
            _attachedController.CheckForEnemyVisibility();
            _attachedController.HearPlayerAround();
            _attachedController.CheckForPlayerKilling();

            _attachedController.targetPositionDummy.transform.position =
                _attachedController.currentBrain.archnemesis.transform.position;
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
            _attachedController.NavMeshAgent.isStopped = true;
            lastFOV = _attachedController.characterProperties.fieldOfVision;
            _attachedController.characterProperties.fieldOfVision = 200f;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _attachedController.NavMeshAgent.isStopped = false;
            _attachedController.characterProperties.fieldOfVision = lastFOV;
        }
    }
}