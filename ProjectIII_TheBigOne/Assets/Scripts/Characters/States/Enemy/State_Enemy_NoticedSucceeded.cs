using UnityEngine;
using State = Player.State;
using StateMachine = Characters.Generic.StateMachine;

namespace Enemy
{
    public class State_Enemy_NoticedSucceeded : State
    {
        private EnemyController _attachedController;
        private float _movementSpeed;

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
            _attachedController.CheckForPlayerNearLight();
            _attachedController.CheckForPlayerOnSight();
            _attachedController.CheckForEnemyVisibility();
            _attachedController.HearPlayerAround();
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
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _attachedController.NavMeshAgent.isStopped = true;
            _attachedController.NavMeshAgent.updateRotation = false;

            playerDirection =
                (_attachedController.currentBrain.archnemesis.transform.position -
                 _attachedController.gameObject.transform.position).normalized;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _attachedController.NavMeshAgent.updateRotation = true;
            _attachedController.currentBrain.IsChasingPlayer = true;
            _attachedController.transform.forward = playerDirection;
            _attachedController.NavMeshAgent.isStopped = false;
        }
    }
}