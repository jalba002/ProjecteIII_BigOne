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

        private Vector3 originalPosition;

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
            _attachedController.CheckForMeshLink();
            _attachedController.CheckForPlayerKilling();

            if (trackTime > 0f)
            {
                trackTime -= deltaTime;
            }
            else if (trackTime <= 0f)
            {
                _attachedController.currentBrain.IsNoticingPlayer = false;
                _attachedController.currentBrain.IsTrackingPlayer = true;
            }
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
            trackTime = 0.5f;
            _attachedController.NavMeshAgent.isStopped = true;

            _attachedController.gameObject.transform.forward =
                (_attachedController.currentBrain.archnemesis.transform.position -
                 _attachedController.gameObject.transform.position).normalized;

            _attachedController.targetPositionDummy.transform.position =
                _attachedController.currentBrain.archnemesis.transform.position;
            _attachedController.NavMeshAgent.SetDestination(_attachedController.targetPositionDummy.transform.position);
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _attachedController.NavMeshAgent.updatePosition = true;
            _attachedController.NavMeshAgent.isStopped = false;
        }
    }
}