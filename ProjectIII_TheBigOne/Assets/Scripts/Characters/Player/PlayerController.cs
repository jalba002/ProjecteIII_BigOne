using System;
using Characters.Brains;
using Characters.Generic;
using Characters.Player;
using Properties;
using UnityEngine;
using CharacterController = Characters.Generic.CharacterController;
using Random = UnityEngine.Random;

namespace Player
{
    [RequireComponent(typeof(Brain))]
    [RequireComponent(typeof(State))]
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : CharacterController
    {
        [Space(2)] [Header("Components")] public Collider attachedCollider;
        public CameraController cameraController;

        public FlashlightController attachedFlashlight;

        public Inventory playerInventory;
        [Header("Config")] public PlayerProperties characterProperties;
        public new PlayerBrain currentBrain { get; private set; }

        public ObjectInspector objectInspector { get; private set; }

        private DynamicActivator dynamicActivator;

        private PuzzleInspector puzzleInspector;

        [HideInInspector] public InteractablesManager interactablesManager;

        public StateMachine stateMachine;

        [Header("Sound settings")] 
        public string footstepPath;
        public string playerDeathPath;
        

        public int InteractCooldown = 5;

        [Header("Physical Materials")] public PhysicMaterial playerMovingMaterial;
        public PhysicMaterial playerStoppedMaterial;

        [Header("Stamina")] private float _currentStamina;

        public float currentStamina
        {
            get { return _currentStamina; }
            set { _currentStamina = Mathf.Clamp(value, 0f, characterProperties.maximumStamina); }
        }

        private float _currentDelay;

        public float currentDelay
        {
            get { return _currentDelay; }
            set { _currentDelay = Mathf.Clamp(value, 0f, characterProperties.rechargeDelay); }
        }

        public void Awake()
        {
            currentBrain = GetComponent<PlayerBrain>();

            if (stateMachine == null)
                stateMachine = GetComponent<StateMachine>();

            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody>();

            if (cameraController == null)
                cameraController = GetComponent<CameraController>();

            if (stateMachine == null)
                stateMachine = GetComponent<StateMachine>();

            if (characterProperties != null)
            {
                characterProperties = Instantiate(characterProperties);
            }
            else
            {
                Debug.LogWarning("Character properties not set!");
            }

            if (attachedCollider == null)
                attachedCollider = GetComponent<Collider>();
            

            if (attachedFlashlight == null)
                attachedFlashlight = GetComponent<FlashlightController>();

            if (playerInventory == null)
            {
                playerInventory = GetComponent<Inventory>();
            }

            objectInspector = GetComponent<ObjectInspector>();

            dynamicActivator = GetComponent<DynamicActivator>();

            puzzleInspector = GetComponent<PuzzleInspector>();

            interactablesManager = GetComponent<InteractablesManager>();

            Cursor.visible = false;
        }

        private void Start()
        {
            stateMachine.SwitchState<State_Player_Walking>();
            SetupStamina();
        }

        private void SetupStamina()
        {
            currentStamina = characterProperties.maximumStamina;
            currentDelay = characterProperties.rechargeDelay;
        }

        void Update()
        {
            if (currentBrain.isActiveAndEnabled)
                currentBrain.GetActions();

            // TODO Disable state machine when game pauses. 
            // Access with events and disable StateMachine.
            // DO NOT REFERENCE GAMEMANAGER FROM HERE.
            float dt = Time.deltaTime;

            ManageStamina(dt);

            if (stateMachine.isActiveAndEnabled)
            {
                stateMachine.UpdateTick(dt);
            }

            if (InteractCooldown > 0) InteractCooldown -= 1;

            CorrectRigidbody();

            if (currentBrain.ShowInventory)
                ToggleInventory();

            UseFlashlight();

            if (InspectPuzzle()) return;
            InspectObjects();
            InteractDynamics();
        }

        bool InspectPuzzle()
        {
            if (puzzleInspector)
            {
                if (currentBrain.Interact && interactablesManager.CurrentInteractable != null)
                {
                    if (interactablesManager.CurrentInteractable.interactionType ==
                        InteractableObject.InteractionType.Puzzle)
                    {
                        return puzzleInspector.Interact(interactablesManager.CurrentInteractable);
                    }
                }
            }

            return false;
        }

        bool InspectObjects()
        {
            if (objectInspector && objectInspector.isActiveAndEnabled)
            {
                if (currentBrain.Interact)
                {
                    if (interactablesManager.CurrentInteractable.interactionType ==
                        InteractableObject.InteractionType.Inspect)
                    {
                        if (objectInspector.Activate(interactablesManager.CurrentInteractable))
                        {
                            // Disable camera and allow the object inspector the use of mouse input.
                            bool enableStuff = objectInspector.GetEnabled();
                            cameraController.angleLocked = enableStuff;
                            if (enableStuff)
                            {
                                stateMachine.SwitchState<State_Player_Inspecting>();
                            }
                            else
                                stateMachine.SwitchState<State_Player_Walking>();

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        void InteractDynamics()
        {
            if (dynamicActivator != null)
            {
                if (interactablesManager != null && interactablesManager.CurrentInteractable != null)
                {
                    if (currentBrain.MouseInteractRelease)
                    {
                        dynamicActivator.Interact(interactablesManager.CurrentInteractable, false);
                        InteractCooldown = 20;
                    }
                    else if (currentBrain.MouseInteract &&
                             !interactablesManager.CurrentInteractable.IsInteracting &&
                             InteractCooldown <= 0)
                    {
                        if (dynamicActivator.Interact(interactablesManager.CurrentInteractable, true))
                        {
                            stateMachine.SwitchState<State_Player_Interacting>();
                        }
                    }
                }
            }

            /*
            if (_dynamicObjectActivator && _dynamicObjectActivator.isActiveAndEnabled)
            {
                if (currentBrain.MouseInteract)
                {
                    if (_dynamicObjectActivator.Activate(interactablesManager.CurrentInteractable))
                        cameraController.angleLocked = true;
                }
                else if (currentBrain.MouseInteractRelease)
                {
                    if (_dynamicObjectActivator.Deactivate())
                        cameraController.angleLocked = false;
                }

                return true;
            }
            return false;*/
        }

        private void UseFlashlight()
        {
            if (currentBrain.FlashlightToggle)
            {
                attachedFlashlight.ToggleFlashlight();
            }
        }

        private void ManageStamina(float deltaTime)
        {
            if (currentDelay > 0f)
            {
                currentDelay -= deltaTime;
            }
            else if (currentStamina < characterProperties.maximumStamina)
            {
                currentStamina += characterProperties.staminaRechargePerSecond * deltaTime;
            }

            // If character is moving, has stamina and is running, deplete stamina and run.
            if ((currentBrain.Running && currentBrain.Direction != Vector3.zero) && currentStamina > 0f)
            {
                currentStamina -= deltaTime;
                currentDelay = characterProperties.rechargeDelay;
            }
        }

        private void FixedUpdate()
        {
            // TODO Reenable stop functionality with GameController.
            // Access with events and disable StateMachine.
            // DO NOT REFERENCE GAMEMANAGER FROM HERE.
            if (stateMachine.isActiveAndEnabled)
            {
                stateMachine.FixedUpdateTick(Time.fixedDeltaTime);
            }
        }

        public void ToggleInventory()
        {
            if (playerInventory && playerInventory.isActiveAndEnabled)
            {
                var enabled = playerInventory.ToggleInventory();
                cameraController.angleLocked = enabled;
                cameraController.cursorLock = !enabled;
                Cursor.visible = enabled;

                if (enabled)
                {
                    //SHOW CURSOR
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
                }
                else
                {
                    //HIDE CURSOR
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
                }
            }
        }


        private void CorrectRigidbody()
        {
            if (rigidbody.angularVelocity != Vector3.zero)
            {
                rigidbody.angularVelocity = Vector3.zero;
            }

            if (currentBrain.Direction == Vector3.zero)
            {
                if (attachedCollider.material.dynamicFriction < 1f)
                {
                    attachedCollider.material = playerStoppedMaterial;
                }
            }
            else
            {
                if (attachedCollider.material.dynamicFriction > 0f)
                {
                    attachedCollider.material = playerMovingMaterial;
                }
            }
        }

        public void PlayerStartsBeingChased()
        {
            AudioManager.PlaySound2D("Sound/Ambience/ManBreathScared");
        }

        public void PlayerStopsBeingChased()
        {
            GameObject.Destroy(GameObject.Find("ManBreathScared"));
        }

        public void PlayFootStepAudio()
        {
            SoundManager.Instance.PlaySound2D(footstepPath);

            /*
            int i = Random.Range(1, footstepSounds.Length);
            audioSource.clip = footstepSounds[i];
            audioSource.PlayOneShot(audioSource.clip);
            footstepSounds[i] = footstepSounds[0];
            footstepSounds[0] = audioSource.clip;
            */
        }

        public Transform ReturnSelf()
        {
            return this.gameObject.transform;
        }

        // Deth
        public override bool Kill()
        {
            // Disable all managers.
            if (IsDead) return false;
            IsDead = true;

            stateMachine.SwitchState<State_Player_Dying>();

            interactablesManager.ClearInteractable();

            SoundManager.Instance.PlaySound2D(playerDeathPath);

            FailproofEnabling(interactablesManager, false);
            FailproofEnabling(puzzleInspector, false);
            FailproofEnabling(objectInspector, false);
            FailproofEnabling(dynamicActivator, false);
            FailproofEnabling(attachedFlashlight, false);
            FailproofEnabling(playerInventory, false);
            FailproofEnabling(cameraController, false);
            return true;
        }

        public bool Resurrect()
        {
            // Enable all managers.
            if (!IsDead) return false;
            IsDead = false;

            stateMachine.SwitchState<State_Player_Walking>();

            FailproofEnabling(interactablesManager, true);
            interactablesManager.ClearInteractable();

            FailproofEnabling(objectInspector, true);
            FailproofEnabling(dynamicActivator, true);
            FailproofEnabling(playerInventory, true);
            FailproofEnabling(attachedFlashlight, true);
            FailproofEnabling(puzzleInspector, true);
            FailproofEnabling(cameraController, true);
            FailproofEnabling(stateMachine, true);
            return true;
        }

        private void FailproofEnabling(MonoBehaviour component, bool enable)
        {
            try
            {
                component.enabled = enable;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message, this.gameObject);
            }
        }
    }
}