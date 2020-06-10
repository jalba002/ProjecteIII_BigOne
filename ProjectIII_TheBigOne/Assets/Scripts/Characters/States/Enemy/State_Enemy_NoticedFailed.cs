using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;
using State = Player.State;
using StateMachine = Characters.Generic.StateMachine;

namespace Enemy
{
    public class State_Enemy_NoticedFailed : State
    {
        private EnemyController _attachedController;
        
        private float _movementSpeed;
        private float trackTime;

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
            _attachedController.CheckForPlayerKilling();

            trackTime += deltaTime;
        }

        public override void OnStateFixedTick(float fixedTime)
        {
            base.OnStateFixedTick(fixedTime);
        }

        public override void OnStateCheckTransition()
        {
            base.OnStateCheckTransition();
            if (trackTime >= _attachedController.characterProperties.timeStoppedAfterFailedNotice)
            {
                _attachedController.currentBrain.HasFailedToNotice = false;
            }
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _movementSpeed = 0;
            //_attachedController.currentBrain._NavMeshAgent.updateRotation = false;
            _attachedController.NavMeshAgent.isStopped = true;

            trackTime = 0f;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _attachedController.NavMeshAgent.isStopped = false;
        }
    }
}