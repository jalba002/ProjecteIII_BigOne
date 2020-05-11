using System;
using Characters.Generic;
using Player;
using UnityEngine;

namespace Enemy
{
    public class State_Enemy_Stunned : State
    {
        private EnemyController _attachedController;

        private float stunTime;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = (EnemyController) Machine.characterController;
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);
            if (stunTime > 0f)
            {
                stunTime -= deltaTime;
            }
            else
            {
                _attachedController.currentBrain.IsStunned = false;
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
            Debug.Log("STUNNED UGH");
            _attachedController.NavMeshAgent.isStopped = true;
            stunTime = 10f;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            Debug.Log("BACK IN ACTION");
            _attachedController.NavMeshAgent.isStopped = false;
            
        }
    }
}