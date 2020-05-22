using System;
using Characters.Generic;
using Player;
using UnityEngine;

namespace Enemy
{
    public class State_Enemy_Chasing : State
    {
        private EnemyController _attachedController;
        private float _movementSpeed;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = (EnemyController) Machine.characterController;
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);
            
            try
            {
                _attachedController.currentBrain.IsPlayerInSight = SensesUtil.IsInSight(_attachedController.gameObject,
                    _attachedController.currentBrain.archnemesis.gameObject,
                    _attachedController.characterProperties.maxDetectionRange,
                    _attachedController.characterProperties.watchableLayers);
            }
            catch (NullReferenceException)
            {
            }

            try
            {
                _attachedController.currentBrain.IsPlayerNearLight =
                    SensesUtil.HasFlashlightEnabled(_attachedController.currentBrain.archnemesis);
            }
            catch (NullReferenceException)
            {
            }

            try
            {
                _attachedController.currentBrain.IsVisible =
                    SensesUtil.IsPlayerSeeingEnemy(_attachedController.currentBrain.archnemesis, _attachedController,
                        GameManager.Instance.GameSettings.DetectionLayers,
                        GameManager.Instance.GameSettings.PlayerViewAngle);
            }
            catch (NullReferenceException)
            {
            }
            
            _attachedController.NavMeshAgent.SetDestination(_attachedController.targetPositionDummy.transform.position);
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
            _movementSpeed = _attachedController.characterProperties.RunSpeed;
            
            _attachedController.targetPositionDummy.transform.parent = _attachedController.currentBrain.archnemesis.transform;
            _attachedController.targetPositionDummy.transform.localPosition = Vector3.zero;
            
            _attachedController.NavMeshAgent.isStopped = false;
            _attachedController.currentBrain.IsChasingPlayer = true;

            _attachedController.NavMeshAgent.speed = _attachedController.characterProperties.WalkSpeed;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _attachedController.targetPositionDummy.transform.parent = null;
            _attachedController.currentBrain.IsChasingPlayer = false;
            _attachedController.currentBrain.IsTrackingPlayer = true;
        }
    }
}