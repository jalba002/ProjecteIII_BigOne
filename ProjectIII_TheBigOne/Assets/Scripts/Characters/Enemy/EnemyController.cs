using System;
using Characters.Brains;
using Characters.Generic;
using Player;
using UnityEngine;
using UnityEngine.AI;
using CharacterController = Characters.Generic.CharacterController;

namespace Enemy
{
    [RequireComponent(typeof(Brain))]
    [RequireComponent(typeof(State))]
    [RequireComponent(typeof(StateMachine))]
    public class EnemyController : CharacterController
    {
        public Collider attachedCollider;

        public new EnemyBrain currentBrain;
        
        public void Awake()
        {
            if (defaultBrain == null)
                defaultBrain = GetComponent<EnemyBrain>();

            currentBrain = (EnemyBrain)defaultBrain;

            if (defaultState == null)
                defaultState = GetComponent<State>();
            
            if (stateMachine == null)
                stateMachine = GetComponent<StateMachine>();
            
            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody>();
            
            if (characterProperties == null)
            {
                characterProperties = ScriptableObject.CreateInstance<CharacterProperties>();
            }
            else
            {
                characterProperties = Instantiate(characterProperties);
            }

            if (!attachedCollider)
                attachedCollider = GetComponent<Collider>();

            // audioSource = GetComponent<AudioSource>();
            /*m_StepTime = Time.time + m_StepTimeRange;
            m_StepTimeRange = m_StepTimeWaking;*/
        }

        private void Start()
        {
            // enableAirControl = true;
            currentBrain._NavMeshAgent.speed = characterProperties.WalkSpeed;
            
            stateMachine.SwitchState<State_Enemy_Idle>();
        }

        void Update()
        {
            if (currentBrain.isActiveAndEnabled)
                currentBrain.GetActions();

            // TODO Disable state machine when game pauses. 
            // Access with events and disable StateMachine.
            // DO NOT REFERENCE GAMEMANAGER FROM HERE.
            if (stateMachine.isActiveAndEnabled)
            {
                stateMachine.UpdateTick(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            // TODO Reenable stop functionality with GameController.
            // Access with events and disable StateMachine.
            // DO NOT REFERENCE GAMEMANAGER FROM HERE.
            if (stateMachine.isActiveAndEnabled)
                stateMachine.FixedUpdateTick(Time.fixedDeltaTime);
        }

        public Transform ReturnSelf()
        {
            return this.gameObject.transform;
        }

        private void OnBecameInvisible()
        {
            //IsVisible = false;
            Debug.LogWarning("I see you.");
        }

        private void OnBecameVisible()
        {
            //IsVisible = true;
            Debug.LogWarning("I can not move.");
        }
    }
}