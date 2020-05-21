using System;
using Assets.Scripts.Game;
using Characters.Generic;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using Random = System.Random;

namespace Enemy
{
    public class State_Enemy_Patrolling : State
    {
        private EnemyController _attachedController;
        private float _movementSpeed;
        private PatrolPoint[] _patrolPoints;
        private Random Alea = new Random();
        private NavMeshPath lastNavMeshPath;
        private float currentWaitTime = 3f;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = (EnemyController) Machine.characterController;
            _patrolPoints = FindObjectsOfType<PatrolPoint>();
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);
            // Move target point to closes patrol point, or a point that has not been visited BUT is near the player.

            _attachedController.CheckForPlayerOnSight();
            _attachedController.CheckForPlayerNearLight();
            _attachedController.CheckForEnemyVisibility();
          

            if (_attachedController.NavMeshAgent.remainingDistance <=
                     _attachedController.characterProperties.positionReachedDistance && lastNavMeshPath == null)
            {
                currentWaitTime = _attachedController.characterProperties.patrolWaitTime;

                SetNewPatrolPoint();
            }
        }

        public override void OnStateFixedTick(float fixedTime)
        {
            base.OnStateFixedTick(fixedTime);
        }

        public override void OnStateCheckTransition()
        {
            base.OnStateCheckTransition();

            // This goes in the Behaviour Tree.
            /*
            if (_attachedController.currentBrain.IsPlayerInSight)
            {
                _attachedController.stateMachine.SwitchState<State_Enemy_Chasing>();
            }*/
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            if (lastNavMeshPath != null)
            {
                _attachedController.NavMeshAgent.path = lastNavMeshPath;
                lastNavMeshPath = null;
            }

            _movementSpeed = _attachedController.characterProperties.WalkSpeed;
            //_attachedController.currentBrain._NavMeshAgent.updateRotation = false;
            //_attachedController.NavMeshAgent.SetDestination(_attachedController.targetPositionDummy.transform.position);
            _attachedController.NavMeshAgent.isStopped = false;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            lastNavMeshPath = _attachedController.NavMeshAgent.path;
        }

        private void SetNewPatrolPoint()
        {
            // Find a random point
            PatrolPoint chosenPatrolPoint = GetRandomPatrolPoint();
            _attachedController.targetPositionDummy.transform.position = chosenPatrolPoint.transform.position;
            _attachedController.NavMeshAgent.SetDestination(_attachedController.targetPositionDummy.transform.position);
            // TODO Maybe move the point from outside the state. Probably from Paranormal or even EnemyController.
        }

        private PatrolPoint GetRandomPatrolPoint()
        {
            // Calculate new position.

            return _patrolPoints[Alea.Next(0, _patrolPoints.Length)];
        }
    }
}