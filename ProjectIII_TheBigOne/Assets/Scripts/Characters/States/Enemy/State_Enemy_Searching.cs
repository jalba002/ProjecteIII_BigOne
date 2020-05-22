using System;
using Assets.Scripts.Game;
using Characters.Generic;
using Player;
using UnityEngine;
using Random = System.Random;

namespace Enemy
{
    public class State_Enemy_Searching : State
    {
        private EnemyController _attachedController;
        private float _movementSpeed;
        private Random Alea = new Random();
        private float trackTime;

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
            currentSearchTime = 0f;
            _attachedController.NavMeshAgent.SetDestination(_attachedController.targetPositionDummy.transform.position);
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