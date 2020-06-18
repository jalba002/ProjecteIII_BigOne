using UnityEngine;
using State = Player.State;
using StateMachine = Characters.Generic.StateMachine;

namespace Enemy
{
    public class State_Enemy_NoticedSecondPhase : State
    {
        private EnemyController _attachedController;
        private float _movementSpeed;
        private float trackTime;

        private Dimitry_AnimatorController enemyAnimator;

        private Vector3 playerDirection;

        private float oldFOV;
        
        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = (EnemyController) Machine.characterController;
            enemyAnimator = _attachedController.GetComponentInChildren<Dimitry_AnimatorController>();
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);

            _attachedController.CheckForPlayerNearLight();
            _attachedController.CheckForPlayerOnSight();
            _attachedController.CheckForEnemyVisibility();
            _attachedController.HearPlayerAround();
            _attachedController.CheckForPlayerKilling();

            trackTime += deltaTime;
            
            _attachedController.transform.forward = Vector3.Slerp(_attachedController.transform.forward,
                playerDirection, (trackTime / _attachedController.characterProperties.timeToChasePlayer)*2);
        }

        public override void OnStateFixedTick(float fixedTime)
        {
            base.OnStateFixedTick(fixedTime);
        }

        public override void OnStateCheckTransition()
        {
            base.OnStateCheckTransition();
            
            if (trackTime >= 0.5f)
            {
                _attachedController.currentBrain.IsNoticingPlayer = false;
                _attachedController.currentBrain.IsPlayerInSight = true;
            }
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _attachedController.NavMeshAgent.isStopped = true;
            _attachedController.NavMeshAgent.updateRotation = false;

            trackTime = 0f;

            enemyAnimator.enemyAnimator.SetBool("IsNoticing", true);

            oldFOV = _attachedController.characterProperties.fieldOfVision;
            _attachedController.characterProperties.fieldOfVision = 350f;

            playerDirection =
                (_attachedController.currentBrain.archnemesis.transform.position -
                 _attachedController.gameObject.transform.position).normalized;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _attachedController.NavMeshAgent.isStopped = false;
            
            _attachedController.NavMeshAgent.updatePosition = true;
            _attachedController.NavMeshAgent.updateRotation = true;
            
            _attachedController.characterProperties.fieldOfVision = oldFOV;
        }
    }
}