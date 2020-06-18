using System;
using System.Runtime.CompilerServices;
using FMOD;
using UnityEngine;
using Debug = UnityEngine.Debug;
using State = Player.State;
using StateMachine = Characters.Generic.StateMachine;

namespace Enemy
{
    public class State_Enemy_NoticedSucceeded : State
    {
        private EnemyController _attachedController;
        private float _movementSpeed;

        private Vector3 playerDirection;
        private Animator attachedAnimator;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = (EnemyController) Machine.characterController;
            attachedAnimator = _attachedController.GetComponentInChildren<Animator>();
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);

            // Move target point to closest patrol point, or a point that has not been visited BUT is near the player.
            _attachedController.CheckForPlayerNearLight();
            _attachedController.CheckForPlayerOnSight();
            _attachedController.CheckForEnemyVisibility();
            //_attachedController.HearPlayerAround();
            //_attachedController.CheckForMeshLink();
            _attachedController.CheckForPlayerKilling();
        }

        public override void OnStateFixedTick(float fixedTime)
        {
            base.OnStateFixedTick(fixedTime);
        }

        public override void OnStateCheckTransition()
        {
            base.OnStateCheckTransition();
            /*
            try
            {
                if (!attachedAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dmitry_ConfirmNotice"))
                {
                    _attachedController.currentBrain.HasSucceededToNotice = false;
                }
            }
            catch (NullReferenceException)
            {
                Debug.LogError("ATTACHED CONTROLLER NOT ASSIGNED");
                _attachedController.currentBrain.HasSucceededToNotice = false;
            }
            */
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _attachedController.NavMeshAgent.isStopped = true;
            _attachedController.NavMeshAgent.updateRotation = false;
            _attachedController.currentBrain.IsNoticingPlayer = false;

        }

        protected override void OnStateExit()
        {
            base.OnStateExit();

            _attachedController.currentBrain.IsChasingPlayer = true;

            _attachedController.targetPositionDummy.transform.position =
                _attachedController.currentBrain.archnemesis.transform.position;

            
            //_attachedController.NavMeshAgent.updateRotation = true;
            
            _attachedController.NavMeshAgent.isStopped = false;
        }
    }
}