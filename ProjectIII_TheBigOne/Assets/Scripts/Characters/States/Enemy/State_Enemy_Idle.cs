using System;
using Characters.Generic;
using Player;
using UnityEngine;

namespace Enemy
{
    public class State_Enemy_Idle : State
    {
        private EnemyController _attachedController;

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
            
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            /*_attachedController.targetPositionDummy.transform.position =
                _attachedController.currentBrain.archnemesis.transform.position;
            _attachedController.NavMeshAgent.SetDestination(_attachedController.targetPositionDummy.transform.position);*/
        }
    }
}