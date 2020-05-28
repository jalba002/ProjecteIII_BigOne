using System;
using System.Collections.Generic;
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
        private Random Alea = new Random();
        private float patrolCooldown = 1f;

        private PatrolPoint lastPatrolPoint;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = (EnemyController) Machine.characterController;
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);
            // Move target point to closes patrol point, or a point that has not been visited BUT is near the player.

            _attachedController.CheckForPlayerOnSight();
            _attachedController.CheckForPlayerNearLight();
            _attachedController.CheckForEnemyVisibility();

            if (_attachedController.NavMeshAgent.remainingDistance <=
                _attachedController.characterProperties.positionReachedDistance)
            {
                if (patrolCooldown > 0f)
                {
                    patrolCooldown -= deltaTime;
                }
                else if (patrolCooldown <= 0f)
                {
                    patrolCooldown = _attachedController.characterProperties.patrolWaitTime;
                    SetNewPatrolPoint();
                }
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
            _attachedController.NavMeshAgent.speed = _attachedController.characterProperties.SlowWalkSpeed;
            _attachedController.NavMeshAgent.isStopped = false;

            if (Machine.lastState is State_Enemy_Traversing)
            {
                SetNewPatrolPoint(lastPatrolPoint);
            }
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _attachedController.targetPositionDummy.transform.parent = null;
        }

        private void SetNewPatrolPoint(PatrolPoint forcedPoint = null)
        {
            PatrolPoint chosenPatrolPoint = forcedPoint != null ? forcedPoint : GetRandomPatrolPoint();
            if (!(Machine.lastState is State_Enemy_Traversing) && lastPatrolPoint == chosenPatrolPoint)
            {
                patrolCooldown = 0.1f;
                return;
            }
            
            lastPatrolPoint = chosenPatrolPoint;

            _attachedController.targetPositionDummy.transform.position = chosenPatrolPoint.transform.position;
            _attachedController.NavMeshAgent.SetDestination(_attachedController.targetPositionDummy.transform.position);
        }

        private PatrolPoint GetRandomPatrolPoint()
        {
            // Calculate new position.

            return _attachedController.patrolPoints[Alea.Next(0, _attachedController.patrolPoints.Count)];
        }
    }
}