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
            _attachedController.currentBrain.IsPlayerNearLight =
                SensesUtil.HasFlashlightEnabled(_attachedController.currentBrain.archnemesis);
            _attachedController.currentBrain.IsVisible =
                SensesUtil.IsPlayerSeeingEnemy(_attachedController.currentBrain.archnemesis, _attachedController,
                    GameManager.Instance.GameSettings.DetectionLayers, GameManager.Instance.GameSettings.PlayerViewAngle);
            Debug.Log(_attachedController.currentBrain.IsVisible);
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
            _attachedController.NavMeshAgent.isStopped = true;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}