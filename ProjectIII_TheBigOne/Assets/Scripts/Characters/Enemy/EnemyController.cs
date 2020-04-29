using System;
using Characters.Brains;
using Characters.Generic;
using Player;
using Properties;
using UnityEngine;
using UnityEngine.AI;
using CharacterController = Characters.Generic.CharacterController;

namespace Enemy
{
    [RequireComponent(typeof(EnemyBrain))]
    [RequireComponent(typeof(State))]
    [RequireComponent(typeof(StateMachine))]
    public class EnemyController : CharacterController
    {
        public Collider attachedCollider;

        public new EnemyBrain currentBrain { get; private set; }
        public BehaviourTree currentBehaviourTree;

        public new EnemyProperties characterProperties;

        public GameObject targetPositionDummy;

        public void Awake()
        {
            currentBrain = GetComponent<EnemyBrain>();

            currentBehaviourTree = new BehaviourTree_Enemy_FirstPhase(this);

            if (stateMachine == null)
                stateMachine = GetComponent<StateMachine>();

            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody>();

            if (characterProperties == null)
            {
                characterProperties = ScriptableObject.CreateInstance<EnemyProperties>();
            }
            else
            {
                characterProperties = Instantiate(characterProperties);
            }

            if (!attachedCollider)
                attachedCollider = GetComponent<Collider>();
        }

        private void Start()
        {
            stateMachine.SwitchState<State_Enemy_Idle>();
            currentBrain._NavMeshAgent.speed = characterProperties.WalkSpeed;
        }

        void Update()
        {
            if (currentBrain.isActiveAndEnabled)
                currentBrain.GetActions();

            if (currentBehaviourTree != null)
            {
                currentBehaviourTree.CalculateNextState(false);
            }
            else
            {
                Debug.LogError("Enemy has no behaviour tree.");
            }

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
            Debug.LogWarning("I am hidden.");
        }

        private void OnBecameVisible()
        {
            //IsVisible = true;
            Debug.LogWarning("I am visible.");
        }

        public override bool Kill()
        {
            // TODO Add kill functionality?
            return false;
        }

        /// ENEMY SENSES ///

        public bool HasPlayerLight()
        {
            return true;
        }
    }
}