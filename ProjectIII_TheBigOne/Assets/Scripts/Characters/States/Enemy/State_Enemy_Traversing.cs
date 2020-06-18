using System;
using UnityEngine;
using World.Objects;
using State = Player.State;
using StateMachine = Characters.Generic.StateMachine;

namespace Enemy
{
    public class State_Enemy_Traversing : State
    {
        private EnemyController _attachedController;

        private float breakTime = 0f;
        private float originalBreakTime = 0f;
        private Vector3 originalPosition;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = (EnemyController) Machine.characterController;
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);

            /*_attachedController.CheckForPlayerNearLight();
            _attachedController.CheckForPlayerOnSight();
            _attachedController.CheckForEnemyVisibility();*/

            if (breakTime > 0f)
            {
                breakTime -= deltaTime;
            }

            if (breakTime <= 0f)
            {
                _attachedController.NavMeshAgent.CompleteOffMeshLink();
                _attachedController.currentBrain.IsOnOffMeshLink = false;
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
            //_attachedController.currentBrain._NavMeshAgent.updateRotation = false;
            //_attachedController.NavMeshAgent.SetDestination(_attachedController.targetPositionDummy.transform.position);
            // _attachedController.NavMeshAgent.isStopped = true;
            _attachedController.currentBrain.CurrentBlockage = _attachedController.NavMeshAgent.currentOffMeshLinkData.offMeshLink.gameObject
                .GetComponent<TraversableBlockage>();

            _attachedController.NavMeshAgent.isStopped = true;

            TeleportToCorrectPosition();

            _attachedController.NavMeshAgent.updateRotation = false;

            Vector3 alteredPos = _attachedController.currentBrain.CurrentBlockage.attachedDynamicObject.transform.position;
            alteredPos.y = _attachedController.transform.position.y;

            _attachedController.transform.forward = (alteredPos -
                                                     _attachedController.transform.position).normalized;

            breakTime = originalBreakTime = _attachedController.currentBrain.CurrentBlockage.removalTime;
        }

        private void TeleportToCorrectPosition()
        {
            Vector3 closestPoint = GetTheShortestPoint(this.gameObject.transform.position,
                _attachedController.currentBrain.CurrentBlockage.attachedLink.startTransform.position,
                _attachedController.currentBrain.CurrentBlockage.attachedLink.endTransform.position);
            _attachedController.NavMeshAgent.Warp(closestPoint);
            originalPosition = closestPoint;
        }

        private Vector3 GetTheShortestPoint(Vector3 currentPos, Vector3 firstPos, Vector3 secondPos)
        {
            firstPos.y = secondPos.y = currentPos.y;
            Vector3 chosenVector = Vector3.Distance(currentPos, firstPos) < Vector3.Distance(currentPos, secondPos)
                ? firstPos
                : secondPos;

            return chosenVector;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            //_attachedController.NavMeshAgent.isStopped = false;
            //_attachedController.NavMeshAgent.Warp(originalPosition);
            _attachedController.NavMeshAgent.isStopped = false;
            _attachedController.NavMeshAgent.updateRotation = true;
        }
    }
}