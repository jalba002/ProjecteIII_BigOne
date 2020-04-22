using System.Diagnostics;
using Characters.Generic;
using UnityEngine;
using CharacterController = UnityEngine.CharacterController;

namespace Player
{
    public class State_Player_Walking : State
    {
        private float _currentStamina;

        public float currentStamina
        {
            get { return _currentStamina; }
            set
            {
                _currentStamina = Mathf.Clamp(value, 0f, Machine.characterController.characterProperties.maximumStamina);
            }
        }

        private float _currentDelay;

        public float currentDelay
        {
            get { return _currentDelay; }
            set
            {
                _currentDelay = Mathf.Clamp(value, 0f, Machine.characterController.characterProperties.rechargeDelay);
            }
        }
        /*private float _staminaRechargeDelay;
        private float _maxStamina;
        private float _staminaRechargePerSecond;*/

        private float _movementSpeed;
        private Rigidbody _attachedRigidbody;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);
            
            // Start recharging stamina if enough time has passed. 
            if (currentDelay > 0f)
            {
                currentDelay -= deltaTime;
            }
            else if (currentStamina < Machine.characterController.characterProperties.maximumStamina)
            {
                currentStamina += Machine.characterController.characterProperties.staminaRechargePerSecond * deltaTime;
            }

            // If character is moving, has stamina and is running, deplete stamina and run.
            if ((Machine.characterController.currentBrain.Running && Machine.characterController.currentBrain.Direction != Vector3.zero) && currentStamina > 0f)
            {
                _movementSpeed = Machine.characterController.characterProperties.RunSpeed;
                currentStamina -= deltaTime;
                currentDelay = Machine.characterController.characterProperties.rechargeDelay;
            }
            else
                _movementSpeed = Machine.characterController.characterProperties.WalkSpeed;

            // Movement code.
            MovementManager.SetVelocity(_attachedRigidbody, Machine.characterController.currentBrain.Direction,
                _movementSpeed);
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
            _attachedRigidbody = Machine.characterController.rigidbody;

            _movementSpeed = Machine.characterController.characterProperties.WalkSpeed;
            currentStamina = Machine.characterController.characterProperties.maximumStamina;
            currentDelay = Machine.characterController.characterProperties.rechargeDelay;
            
            // TODO Profile if variables are better than constant variable accesses.
            /*
            _maxStamina = Machine.characterController.characterProperties.maximumStamina;
            _staminaRechargeDelay = Machine.characterController.characterProperties.rechargeDelay;
            _staminaRechargePerSecond = Machine.characterController.characterProperties.rechargeDelay;
            */
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}