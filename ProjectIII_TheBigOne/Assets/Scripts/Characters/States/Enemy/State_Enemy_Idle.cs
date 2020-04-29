using Characters.Generic;
using Player;
using UnityEngine;

namespace Enemy
{
    public class State_Enemy_Idle : State
    {
        private EnemyController _attachedController;

        private float stalkTime = 5f;
        private float currentStalkTime = 0f;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = (EnemyController) Machine.characterController;
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);

            if (currentStalkTime < stalkTime)
            {
                currentStalkTime += deltaTime;
            }
            else
            {
                Debug.LogWarning("STALKED LONG ENOUGH.");
            }

            _attachedController.currentBrain.IsPlayerInSight = SensesUtil.IsInSight(_attachedController,
                _attachedController.currentBrain.archnemesis.gameObject,
                _attachedController.characterProperties.maxDetectionRange,
                _attachedController.characterProperties.watchableLayers, true);
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

            // Updates the rotation based on the brain decisions.
            //_attachedController.currentBrain._NavMeshAgent.updateRotation = false;
            _attachedController.currentBrain._NavMeshAgent.isStopped = true;
            currentStalkTime = 0f;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}