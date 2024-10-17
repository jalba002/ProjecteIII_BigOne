using Tavaris.Entities;
using Tavaris.Manager;
using UnityEngine;

namespace Tavaris.States
{
    public class State_Player_Walking : State
    {
        private PlayerController _attachedController;

        [Header("Sound setting")]
        public float stepTimeWalking;
        public float stepTimeRunning;
        private float stepTimeRange;
        private float stepTime = 0.0f;


        /*private float _staminaRechargeDelay;
        private float _maxStamina;
        private float _staminaRechargePerSecond;*/

        private float _movementSpeed;
        private Rigidbody _attachedRigidbody;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = ((PlayerController)Machine.characterController);
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);

            // Start recharging stamina if enough time has passed. 
            // If character is moving, has stamina and is running, deplete stamina and run.
            if ((_attachedController.currentBrain.Running && _attachedController.currentBrain.Direction != Vector3.zero) && _attachedController.currentStamina > 0f)
            {
                _movementSpeed = _attachedController.characterProperties.RunSpeed;
                stepTimeRange = stepTimeRunning;
            }
            else
            {
                _movementSpeed = _attachedController.characterProperties.WalkSpeed;
                stepTimeRange = stepTimeWalking;
            }

            // Movement code.
            MovementManager.SetVelocity(_attachedRigidbody, _attachedController.currentBrain.Direction,
                _movementSpeed);


            if (_attachedController.currentBrain.Direction.magnitude > .1f && stepTime <= Time.time)
            {
                _attachedController.PlayFootStepAudio();
                stepTime = Time.time + stepTimeRange;
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

            _attachedRigidbody = _attachedController.rigidbody;

            _movementSpeed = _attachedController.characterProperties.WalkSpeed;

            stepTime = Time.time + stepTimeRange;
            stepTimeRange = stepTimeWalking;

            // TODO Profile if variables are better than constant variable accesses.
            /*
            _maxStamina = _attachedController.characterProperties.maximumStamina;
            _staminaRechargeDelay = _attachedController.characterProperties.rechargeDelay;
            _staminaRechargePerSecond = _attachedController.characterProperties.rechargeDelay;
            */
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}