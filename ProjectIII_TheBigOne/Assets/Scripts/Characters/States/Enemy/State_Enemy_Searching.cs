using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;
using State = Player.State;
using StateMachine = Characters.Generic.StateMachine;

namespace Enemy
{
    public class State_Enemy_Searching : State
    {
        private EnemyController _attachedController;
        private float _movementSpeed;
        private Random Alea = new Random();
        private float trackTime;

        private NavMeshPath forcedPath;

        private float currentSearchTime;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = (EnemyController) Machine.characterController;
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);

            // Move target point to closest patrol point, or a point that has not been visited BUT is near the player.
            _attachedController.CheckForPlayerNearLight();
            _attachedController.CheckForPlayerOnSight();
            _attachedController.CheckForEnemyVisibility();
            _attachedController.HearPlayerAround();
            _attachedController.currentBrain.IsNoticingPlayer = _attachedController.currentBrain.IsHearingPlayer;
            _attachedController.CheckForMeshLink();
            _attachedController.CheckForPlayerKilling();

            if (trackTime > 0f)
            {
                trackTime -= deltaTime;
            }
            else if (trackTime <= 0f)
            {
                StopChase();
            }

            currentSearchTime += deltaTime;
        }

        public override void OnStateFixedTick(float fixedTime)
        {
            base.OnStateFixedTick(fixedTime);
        }

        public override void OnStateCheckTransition()
        {
            base.OnStateCheckTransition();

            if (currentSearchTime >= _attachedController.characterProperties.maxSearchTime)
            {
                _attachedController.currentBrain.IsTrackingPlayer = false;
            }
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _movementSpeed = _attachedController.characterProperties.WalkSpeed;
            //_attachedController.currentBrain._NavMeshAgent.updateRotation = false;
            _attachedController.NavMeshAgent.isStopped = false;

            trackTime = _attachedController.characterProperties.maxTimeOfPerfectTracking;
            currentSearchTime = 0f - Vector3.Distance(_attachedController.targetPositionDummy.transform.position,
                                    _attachedController.NavMeshAgent.transform.position);
            _attachedController.NavMeshAgent.SetDestination(_attachedController.targetPositionDummy.transform.position);
            _attachedController.NavMeshAgent.autoRepath = false;
            //forcedPath = _attachedController.NavMeshAgent.path;
        }

        private void StopChase()
        {
            _attachedController.targetPositionDummy.transform.parent = null;
            _attachedController.NavMeshAgent.SetDestination(_attachedController.targetPositionDummy.transform.position);
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}