using Characters.Brains;
using Characters.Generic;
using Properties;
using UnityEngine;
using CharacterController = Characters.Generic.CharacterController;

namespace Player
{
    [RequireComponent(typeof(Brain))]
    [RequireComponent(typeof(State))]
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SimpleActivator))]
    [RequireComponent(typeof(ObjectInspector))]
    public class PlayerController : CharacterController
    {
        public CameraController cameraController;

        // public bool enableAirControl { get; set; }
        public Collider attachedCollider;
        public ObjectInspector objectInspector;

        public SimpleActivator simpleActivator;

        public PlayerProperties characterProperties;

        public new PlayerBrain currentBrain { get; private set; }

        [Header("Sound settings")]
        public AudioClip[] footstepSounds;
        private AudioSource audioSource;

        /*[Header("Ground Detection")] public Transform groundPosition;
        [Range(0.01f, 1f)] public float castRadius = 1f;
        public LayerMask walkableLayers;
        public bool isPlayerGrounded { get; private set; }*/

        public FlashlightController attachedFlashlight;

        public void Awake()
        {
            currentBrain = GetComponent<PlayerBrain>();

            if (stateMachine == null)
                stateMachine = GetComponent<StateMachine>();

            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody>();

            if (cameraController == null)
                cameraController = GetComponent<CameraController>();

            simpleActivator = this.gameObject.GetComponent<SimpleActivator>();

            objectInspector = this.gameObject.GetComponent<ObjectInspector>();

            if (characterProperties != null)
            {
                characterProperties = Instantiate(characterProperties);
            }
            else
            {
                Debug.LogWarning("Character properties not set!");
            }

            if (!attachedCollider)
                attachedCollider = GetComponent<Collider>();

            audioSource = GetComponent<AudioSource>();

            if (attachedFlashlight == null)
                attachedFlashlight = GetComponent<FlashlightController>();
        }

        private void Start()
        {
            // enableAirControl = true;
            stateMachine.SwitchState<State_Player_Walking>();
        }

        void Update()
        {
            if (currentBrain.isActiveAndEnabled)
                currentBrain.GetActions();

            /*if (currentBrain.Shooting && !GameController.Instance.m_GamePaused)
            {
                equippedWeapon.MainFire();
            }*/

            /*if (currentBrain.Aiming && !GameController.Instance.m_GamePaused)
            {
                if (equippedWeapon.AltFire())
                    weaponAnimator.SetTrigger("ChangeColor");
            }*/

            // TODO Disable state machine when game pauses. 
            // Access with events and disable StateMachine.
            // DO NOT REFERENCE GAMEMANAGER FROM HERE.
            if (stateMachine.isActiveAndEnabled)
            {
                stateMachine.UpdateTick(Time.deltaTime);
            }

            InspectObjects();
            InteractDoors();
            UseFlashlight();

            //Cheat to test sounds
            /*
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.G))
            {
                PlayerStartsBeingChased();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                PlayerStopsBeingChased();
            }
#endif
*/
        }

        void InspectObjects()
        {
            if (objectInspector != null && objectInspector.isActiveAndEnabled)
            {
                if (currentBrain.Interact)
                {
                    if (objectInspector.Activate(cameraController.attachedCamera))
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
                    }
                }
            }
        }

        void InteractDoors()
        {
            if (simpleActivator != null && simpleActivator.isActiveAndEnabled)
            {
                if (currentBrain.MouseInteract)
                {
                    if (simpleActivator.Activate(cameraController.attachedCamera))
                        cameraController.angleLocked = true;
                }
                else if (currentBrain.MouseInteractRelease)
                {
                    if (simpleActivator.Deactivate())
                        cameraController.angleLocked = false;
                }
            }
        }

        public void PlayerStartsBeingChased()
        {
            AudioManager.Instance.PlaySound2D("Sound/Ambience/ManBreathScared");
        }
        public void PlayerStopsBeingChased()
        {
            GameObject.Destroy(GameObject.Find("ManBreathScared"));
        }

        public void PlayFootStepAudio()
        {
            int i = Random.Range(1, footstepSounds.Length);
            audioSource.clip = footstepSounds[i];
            audioSource.PlayOneShot(audioSource.clip);
            footstepSounds[i] = footstepSounds[0];
            footstepSounds[0] = audioSource.clip;
        }

        private void UseFlashlight()
        {
            // TODO Remap controls for flashlight.
            if (currentBrain.FlashlightToggle)
            {
                attachedFlashlight.ToggleFlashlight();
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

        public Transform ReturnSelf()
        {
            return this.gameObject.transform;
        }
    }
}