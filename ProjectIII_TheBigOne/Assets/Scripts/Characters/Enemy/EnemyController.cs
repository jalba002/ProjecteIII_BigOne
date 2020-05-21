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
        public enum StartingBehaviour
        {
            Halt,
            FirstPhase,
            SecondPhase,
            OnlyChase
        }

        [Header("Behaviours")] public StartingBehaviour startingBehaviour;


        public new EnemyBrain currentBrain { get; private set; }

        public BehaviourTree currentBehaviourTree { get; set; }

        [Header("Enemy Settings")] public new EnemyProperties characterProperties;

        [Header("Components")] public Collider attachedCollider;
        public Renderer meshRenderer;
        [Header("NavMesh")] public EnemyTargetDummy targetPositionDummy;
        public NavMeshAgent NavMeshAgent;

        [Header("Components from Thirds")] private FlashlightController playerFlashlight;

        public void Awake()
        {
            currentBrain = GetComponent<EnemyBrain>();

            NavMeshAgent = GetComponent<NavMeshAgent>();

            SetStartingBehaviourTree();

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

            if (meshRenderer == null)
            {
                Debug.LogError("Attach a mesh renderer.", this.gameObject);
            }
        }

        private void SetStartingBehaviourTree()
        {
            switch (startingBehaviour)
            {
                case StartingBehaviour.Halt:
                    currentBehaviourTree = new BehaviourTree_Enemy_Halted(this);
                    break;
                case StartingBehaviour.FirstPhase:
                    currentBehaviourTree = new BehaviourTree_Enemy_FirstPhase(this);
                    break;
                case StartingBehaviour.SecondPhase:
                    currentBehaviourTree = new BehaviourTree_Enemy_SecondPhase(this);
                    break;
                case StartingBehaviour.OnlyChase:
                    currentBehaviourTree = new BehaviourTree_Enemy_OnlyChase(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Start()
        {
            stateMachine.SwitchState<State_Enemy_Idle>();

            NavMeshAgent.speed = characterProperties.WalkSpeed;

            playerFlashlight = FindObjectOfType<FlashlightController>();

            targetPositionDummy = FindObjectOfType<EnemyTargetDummy>();
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
                Debug.LogWarning("Enemy has no behaviour tree.");
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

        public override bool Kill()
        {
            // TODO Add kill functionality?
            return false;
        }

        public void CheckForPlayerOnSight()
        {
            try
            {
                currentBrain.IsPlayerInSight = SensesUtil.IsInSight(gameObject,
                    currentBrain.archnemesis.gameObject,
                    characterProperties.maxDetectionRange,
                    characterProperties.watchableLayers);
            }
            catch (NullReferenceException)
            {
            }
        }

        public void CheckForPlayerNearLight()
        {
            try
            {
                currentBrain.IsPlayerNearLight =
                    SensesUtil.HasFlashlightEnabled(currentBrain.archnemesis);
            }
            catch (NullReferenceException)
            {
            }
        }

        public void CheckForEnemyVisibility()
        {
            try
            {
                currentBrain.IsVisible =
                    SensesUtil.IsPlayerSeeingEnemy(currentBrain.archnemesis, this,
                        GameManager.Instance.GameSettings.DetectionLayers,
                        GameManager.Instance.GameSettings.PlayerViewAngle);
            }
            catch (NullReferenceException)
            {
            }
        }

        // Invisiblity Enabled.
        private void OnBecameInvisible()
        {
            currentBrain.IsBeingRendered = false;
        }

        private void OnBecameVisible()
        {
            currentBrain.IsBeingRendered = true;
        }
    }
}