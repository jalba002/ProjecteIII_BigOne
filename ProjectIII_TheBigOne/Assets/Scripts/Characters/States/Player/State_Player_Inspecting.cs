﻿using System.Diagnostics;
using Characters.Generic;
using UnityEngine;
using CharacterController = UnityEngine.CharacterController;

namespace Player
{
    public class State_Player_Inspecting : State
    {
        private Rigidbody _attachedRigidbody;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedRigidbody = Machine.characterController.rigidbody;
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);
            // Movement code.
            MovementManager.SetVelocity(_attachedRigidbody, Vector3.zero, 0f);
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
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}