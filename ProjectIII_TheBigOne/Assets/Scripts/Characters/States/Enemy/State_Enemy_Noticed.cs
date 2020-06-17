using UnityEngine;
using State = Player.State;
using StateMachine = Characters.Generic.StateMachine;

namespace Enemy
{
    public class State_Enemy_Noticed : State
    {
        private EnemyController _attachedController;
        private float _movementSpeed;
        private float trackTime;

        private Dimitry_AnimatorController enemyAnimator;

        private Vector3 playerDirection;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = (EnemyController) Machine.characterController;
            enemyAnimator = _attachedController.GetComponentInChildren<Dimitry_AnimatorController>();
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);

            // Move target point to closest patrol point, or a point that has not been visited BUT is near the player.
            _attachedController.CheckForPlayerNearLight();
            _attachedController.CheckForPlayerOnSight();
            _attachedController.CheckForEnemyVisibility();
            _attachedController.HearPlayerAround();
            //_attachedController.CheckForMeshLink();
            _attachedController.CheckForPlayerKilling();


            if (!_attachedController.currentBrain.IsHearingPlayer)
            {
                ResolveState();
            }
            else if (_attachedController.currentBrain.IsPlayerInSight)
            {
                trackTime = _attachedController.characterProperties.timeToChasePlayer;
                _attachedController.currentBrain.IsNoticingPlayer = false;
                ResolveState();
            }
        }

        public override void OnStateFixedTick(float fixedTime)
        {
            base.OnStateFixedTick(fixedTime);
        }

        public override void OnStateCheckTransition()
        {
            base.OnStateCheckTransition();
            /*_attachedController.transform.forward = Vector3.Slerp(_attachedController.transform.forward,
                playerDirection, trackTime / _attachedController.characterProperties.timeToChasePlayer);*/
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _attachedController.NavMeshAgent.isStopped = true;
            _attachedController.NavMeshAgent.updateRotation = false;

            trackTime = 0f;
            
            enemyAnimator.enemyAnimator.SetBool("IsNoticing", true);

            playerDirection =
                (_attachedController.currentBrain.archnemesis.transform.position -
                 _attachedController.gameObject.transform.position).normalized;
        }

        private void ResolveState()
        {
            if (trackTime >= _attachedController.characterProperties.timeToChasePlayer)
            {
                _attachedController.currentBrain.HasSucceededToNotice = true;
            }
            else
            {
                _attachedController.currentBrain.HasFailedToNotice = true;
            }
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _attachedController.NavMeshAgent.updateRotation = true;
            _attachedController.NavMeshAgent.isStopped = false;
            enemyAnimator.enemyAnimator.SetBool("IsNoticing", false);
        }
    }
}