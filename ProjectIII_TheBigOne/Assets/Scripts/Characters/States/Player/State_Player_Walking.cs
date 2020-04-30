﻿using System.Diagnostics;
using Characters.Generic;
using UnityEngine;
using CharacterController = UnityEngine.CharacterController;

namespace Player
{
    public class State_Player_Walking : State
    {
        private float _currentStamina;
        private PlayerController _attachedController;

        public float currentStamina
        {
            get { return _currentStamina; }
            set
            {
                _currentStamina = Mathf.Clamp(value, 0f, _attachedController.characterProperties.maximumStamina);
            }
        }

        private float _currentDelay;

        public float currentDelay
        {
            get { return _currentDelay; }
            set
            {
                _currentDelay = Mathf.Clamp(value, 0f, _attachedController.characterProperties.rechargeDelay);
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
            _attachedController = ((PlayerController)Machine.characterController);
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);
            
            // Start recharging stamina if enough time has passed. 
            if (currentDelay > 0f)
            {
                currentDelay -= deltaTime;
            }
            else if (currentStamina < _attachedController.characterProperties.maximumStamina)
            {
                currentStamina += _attachedController.characterProperties.staminaRechargePerSecond * deltaTime;
            }

            // If character is moving, has stamina and is running, deplete stamina and run.
            if ((_attachedController.currentBrain.Running && _attachedController.currentBrain.Direction != Vector3.zero) && currentStamina > 0f)
            {
                _movementSpeed = _attachedController.characterProperties.RunSpeed;
                currentStamina -= deltaTime;
                currentDelay = _attachedController.characterProperties.rechargeDelay;
            }
            else
                _movementSpeed = _attachedController.characterProperties.WalkSpeed;

            // Movement code.
            MovementManager.SetVelocity(_attachedRigidbody, _attachedController.currentBrain.Direction,
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
            
            _attachedRigidbody = _attachedController.rigidbody;

            _movementSpeed = _attachedController.characterProperties.WalkSpeed;
            currentStamina = _attachedController.characterProperties.maximumStamina;
            currentDelay = _attachedController.characterProperties.rechargeDelay;
            
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