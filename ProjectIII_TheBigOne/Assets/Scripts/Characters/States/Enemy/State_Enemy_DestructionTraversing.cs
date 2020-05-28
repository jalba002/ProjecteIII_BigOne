using System;
using UnityEngine;
using World.Objects;
using State = Player.State;
using StateMachine = Characters.Generic.StateMachine;

namespace Enemy
{
    public class State_Enemy_DestructionTraversing : State
    {
        private EnemyController _attachedController;

        private TraversableBlockage _currentBlockage;
        private float breakTime = 0f;
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
            else
            {
                _attachedController.NavMeshAgent.CompleteOffMeshLink();
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
            _currentBlockage = _attachedController.NavMeshAgent.currentOffMeshLinkData.offMeshLink.gameObject
                .GetComponent<TraversableBlockage>();
            originalPosition = _attachedController.gameObject.transform.position;
            breakTime = _currentBlockage.removalTime;
            _attachedController.NavMeshAgent.isStopped = true;
            //_attachedController.NavMeshAgent.Warp(_currentBlockage.attachedLink.startTransform.position);
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _attachedController.NavMeshAgent.isStopped = false;
            _attachedController.NavMeshAgent.Warp(originalPosition);
            ResolveBlockage();
        }

        private void ResolveBlockage()
        {
            switch (_currentBlockage.attachedDynamicObject.objectType)
            {
                case DynamicObject.ObjectType.Door:
                    // Nothing yet.
                    _currentBlockage.attachedDynamicObject.BreakJoint();
                    _currentBlockage.attachedDynamicObject.BreakOpening(Machine.characterController.transform.forward,10f);
                    break;
                case DynamicObject.ObjectType.Drawer:
                    // Can't get blocked by a drawer...?
                    break;
                case DynamicObject.ObjectType.Pallet:
                    // This should be the wall explosion.
                    _currentBlockage.gameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}