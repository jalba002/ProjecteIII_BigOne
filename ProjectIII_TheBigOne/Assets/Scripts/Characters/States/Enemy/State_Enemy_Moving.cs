using Characters.Generic;
using Player;
using UnityEngine;

namespace Enemy
{
    public class State_Enemy_Moving : State
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
            
            // Calculate transform to avoid enemy pitch rotation.
            /*Vector3 lookingPosition = _attachedController.targetPositionDummy.transform.position;
            lookingPosition.y = Machine.characterController.transform.position.y;
            
            Machine.characterController.transform.LookAt(lookingPosition);
*/
            // Movement code.
            /*MovementManager.SetVelocity(_attachedRigidbody, Machine.characterController.currentBrain.Direction,
                _movementSpeed);*/
            // _attachedController.currentBrain._NavMeshAgent.SetDestination(_attachedController.targetPositionDummy.transform.position);
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
            _attachedController.currentBrain._NavMeshAgent.SetDestination(_attachedController.targetPositionDummy.transform.position);
            _attachedController.currentBrain._NavMeshAgent.isStopped = false;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}