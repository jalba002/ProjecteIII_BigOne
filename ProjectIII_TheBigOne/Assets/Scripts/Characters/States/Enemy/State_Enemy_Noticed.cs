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

        private Vector3 playerDirection;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = (EnemyController) Machine.characterController;
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);

            // Move target point to closest patrol point, or a point that has not been visited BUT is near the player.
            //_attachedController.CheckForPlayerNearLight();
            _attachedController.CheckForPlayerOnSight();
            //_attachedController.CheckForEnemyVisibility();
            //_attachedController.HearPlayerAround();
            //_attachedController.CheckForMeshLink();
            //_attachedController.CheckForPlayerKilling();

            trackTime += deltaTime;

            if (!_attachedController.currentBrain.IsPlayerInSight ||
                trackTime >= _attachedController.characterProperties.timeToChasePlayer)
            {
                _attachedController.currentBrain.IsNoticingPlayer = false;
            }
        }

        public override void OnStateFixedTick(float fixedTime)
        {
            base.OnStateFixedTick(fixedTime);
        }

        public override void OnStateCheckTransition()
        {
            base.OnStateCheckTransition();
            _attachedController.transform.forward = Vector3.Slerp(_attachedController.transform.forward,
                playerDirection, trackTime / _attachedController.characterProperties.timeToChasePlayer);
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _attachedController.NavMeshAgent.isStopped = true;
            _attachedController.NavMeshAgent.updateRotation = false;

            trackTime = 0f;

            playerDirection =
                (_attachedController.currentBrain.archnemesis.transform.position -
                 _attachedController.gameObject.transform.position).normalized;
        }

        private void ResolveState()
        {
            Debug.Log("Track time: " + trackTime);
            if (trackTime >= _attachedController.characterProperties.timeToChasePlayer)
            {
                Debug.Log("Noticed to chase.");
                _attachedController.currentBrain.IsChasingPlayer = true;
            }
            else if (trackTime >= _attachedController.characterProperties.timeToSearchPlayer)
            {
                _attachedController.targetPositionDummy.transform.position =
                    _attachedController.currentBrain.archnemesis.transform.position;
                _attachedController.NavMeshAgent.SetDestination(_attachedController.targetPositionDummy.transform
                    .position);

                Debug.Log("Noticed to track.");
                _attachedController.currentBrain.IsTrackingPlayer = true;
            }
            else
            {
                _attachedController.currentBrain.HasFailedToNotice = true;
            }
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            ResolveState();
            _attachedController.NavMeshAgent.updateRotation = true;
            _attachedController.NavMeshAgent.isStopped = false;
        }
    }
}