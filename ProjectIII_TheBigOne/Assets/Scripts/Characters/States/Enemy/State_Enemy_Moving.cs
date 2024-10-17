using System;
using Tavaris.Entities;
using Tavaris.Utils;

namespace Tavaris.States
{
    public class State_Enemy_Moving : State
    {
        private EnemyController _attachedController;
        private float _movementSpeed;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = (EnemyController)Machine.characterController;
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

            // TODO Maybe move the point from outside the state. Probably from Paranormal or even EnemyController.
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
            _movementSpeed = _attachedController.characterProperties.WalkSpeed;
            //_attachedController.currentBrain._NavMeshAgent.updateRotation = false;
            _attachedController.NavMeshAgent.SetDestination(_attachedController.targetPositionDummy.transform.position);
            _attachedController.NavMeshAgent.isStopped = false;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}