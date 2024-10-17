using Tavaris.Entities;
using UnityEngine;

namespace Tavaris.States
{
    public class State_Enemy_Killing : State
    {
        private EnemyController _attachedController;

        private Animator enemyAnimator;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = (EnemyController)Machine.characterController;
            enemyAnimator = _attachedController.GetComponentInChildren<Animator>();

        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);
            _attachedController.CheckForEnemyVisibility();
            _attachedController.CheckForPlayerNearLight();
            _attachedController.CheckForPlayerOnSight();
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

            // TODO Change to animation kill.
            /*if (!GameManager.Instance.GameSettings.isPlayerInvincible)
            {
                GameManager.Instance.EndGame();
            }*/
            enemyAnimator.SetBool("Attack", true);
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _attachedController.currentBrain.IsPlayerCloseEnoughForDeath = false;
            _attachedController.NavMeshAgent.isStopped = false;
            enemyAnimator.SetBool("Attack", false);
        }
    }
}