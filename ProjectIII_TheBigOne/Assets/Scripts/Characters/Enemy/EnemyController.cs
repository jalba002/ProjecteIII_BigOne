﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Game;
using Characters.Generic;
using Player;
using Properties;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
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
        [Header("Sound")] public string terrorRadiusPath;
        [Range(0,50)]public float maxRadius = 15f;

        [Header("Enemy Settings")] public new EnemyProperties characterProperties;

        [Header("Components")] public Collider attachedCollider;
        public GameObject dimitryEyes;
        [Header("NavMesh")] public EnemyTargetDummy targetPositionDummy;
        public NavMeshAgent NavMeshAgent;

        [Header("Components from Thirds")] private FlashlightController playerFlashlight;

        public List<PatrolPoint> patrolPoints = new List<PatrolPoint>();

        public UnityEvent OnPhaseChange = new UnityEvent();

        public void Awake()
        {
            currentBrain = GetComponent<EnemyBrain>();

            NavMeshAgent = GetComponent<NavMeshAgent>();

            try
            {
                SoundManager.Instance.PlayEvent(terrorRadiusPath, transform, 0, maxRadius);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }

            SetStartingBehaviourTree();

            if (stateMachine == null)
                stateMachine = GetComponent<StateMachine>();

            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody>();

            if (characterProperties == null)
            {
                characterProperties = ScriptableObject.CreateInstance<EnemyProperties>();
            }
            characterProperties = Instantiate(characterProperties);

            if (!attachedCollider)
                attachedCollider = GetComponent<Collider>();

            SetupEyes();
        }

        private void SetupEyes()
        {
            if (dimitryEyes == null)
            {
                dimitryEyes = new GameObject("DimitryEyes");
                dimitryEyes.transform.parent = this.transform;
                dimitryEyes.transform.position = new Vector3(0f, 1f, 0f);
                dimitryEyes.transform.eulerAngles = Vector3.zero;
            }
        }

        private void SetStartingBehaviourTree()
        {
            switch (startingBehaviour)
            {
                case StartingBehaviour.Halt:
                    SetNewPhase(new BehaviourTree_Enemy_Halted(this));
                    break;
                case StartingBehaviour.FirstPhase:
                    SetNewPhase(new BehaviourTree_Enemy_FirstPhase(this));
                    break;
                case StartingBehaviour.SecondPhase:
                    SetNewPhase(new BehaviourTree_Enemy_SecondPhase(this));
                    break;
                case StartingBehaviour.OnlyChase:
                    SetNewPhase(new BehaviourTree_Enemy_OnlyChase(this));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetNewPhase(BehaviourTree newBehaviour)
        {
            currentBehaviourTree = newBehaviour;

            if (newBehaviour is BehaviourTree_Enemy_FirstPhase)
            {
                patrolPoints = ObjectTracker.PatrolPointsFirstPhase;
            }
            else if (newBehaviour is BehaviourTree_Enemy_SecondPhase)
            {
                patrolPoints = ObjectTracker.PatrolPointsSecondPhase;
            }

            OnPhaseChange.Invoke();
        }

        private void Start()
        {
            SetStartingBehaviourTree();

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
                currentBrain.IsPlayerInSight = SensesUtil.IsInSight(dimitryEyes,
                    currentBrain.archnemesis.gameObject,
                    currentBrain.archnemesis.cameraController.m_PitchControllerTransform,
                    characterProperties.maxDetectionRange,
                    characterProperties.watchableLayers, false, characterProperties.fieldOfVision);
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

        public void HearPlayerAround()
        {
            try
            {
                // Make a sphere cast. 
                // Check the player distances, if too close, he is heard.
                // If inside but far, if he moves, he is detected.
                // Both should transition to the alerted state, then do their thing.

                currentBrain.IsHearingPlayer = SensesUtil.IsHearingPlayer(this, currentBrain.archnemesis,
                                                   characterProperties.hearingMaxRange,
                                                   characterProperties.layersThatBlockHearing) &&
                                               currentBrain.archnemesis.currentBrain.Direction != Vector3.zero;
            }
            catch (NullReferenceException)
            {
            }
        }

        public void CheckForPlayerKilling()
        {
            try
            {
                currentBrain.IsPlayerCloseEnoughForDeath = currentBrain.IsPlayerInSight &&
                                                           currentBrain.DistanceToPlayer <=
                                                           characterProperties.tooCloseRange
                                                           && !currentBrain.archnemesis.IsDead;
            }
            catch (NullReferenceException)
            {
            }
        }

        public void CheckForMeshLink()
        {
            currentBrain.IsOnOffMeshLink = NavMeshAgent.isOnOffMeshLink;
        }

        public void ChillDown()
        {
            currentBehaviourTree = new BehaviourTree_Enemy_Halted(this);
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