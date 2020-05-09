using System;
using Characters.Generic;
using Player;

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
            _movementSpeed = _attachedController.characterProperties.WalkSpeed;
            //_attachedController.currentBrain._NavMeshAgent.updateRotation = false;
            _attachedController.targetPositionDummy.transform.parent = _attachedController.currentBrain.archnemesis.transform;
            _attachedController.NavMeshAgent.SetDestination(_attachedController.targetPositionDummy.transform.position);
            _attachedController.NavMeshAgent.isStopped = false;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _attachedController.targetPositionDummy.transform.parent = null;
        }
    }
}