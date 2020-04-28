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
            
            // Updates the rotation based on the brain decisions.
            _attachedController.currentBrain._NavMeshAgent.updateRotation = false;
            _attachedController.currentBrain._NavMeshAgent.isStopped = true;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}