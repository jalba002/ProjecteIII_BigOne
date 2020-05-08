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
    public class PlayerController : CharacterController
    {
        public Collider attachedCollider { get; set; }
        [Header("Components")] public CameraController cameraController;

        public FlashlightController attachedFlashlight;

        public Inventory playerInventory;
        [Header("Config")] public PlayerProperties characterProperties;
        public new PlayerBrain currentBrain { get; private set; }


        [Header("Sound settings")] public AudioClip[] footstepSounds;
        private AudioSource audioSource;


        /*[Header("Ground Detection")] public Transform groundPosition;
        [Range(0.01f, 1f)] public float castRadius = 1f;
        public LayerMask walkableLayers;
        public bool isPlayerGrounded { get; private set; }*/


        public void Awake()
        {
            currentBrain = GetComponent<PlayerBrain>();

            if (stateMachine == null)
                stateMachine = GetComponent<StateMachine>();

            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody>();

            if (cameraController == null)
                cameraController = GetComponent<CameraController>();

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

            if (playerInventory == null)
            {
                playerInventory = GetComponent<Inventory>();
            }

            Cursor.visible = false;
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
            
            CorrectRigidbody();
            ToggleInventory();
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
        
        private void UseFlashlight()
        {
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

        public void ToggleInventory()
        {
            if (playerInventory && playerInventory.isActiveAndEnabled)
            {
                if (currentBrain.ShowInventory)
                {
                    var enabled = playerInventory.ToggleInventory();
                    cameraController.angleLocked = enabled;
                    cameraController.cursorLock = !enabled;
                    Cursor.visible = enabled;
                }
            }
        }

        public void CorrectRigidbody()
        {
            if (rigidbody.angularVelocity != Vector3.zero)
            {
                rigidbody.angularVelocity = Vector3.zero;
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
            int i = Random.Range(1, footstepSounds.Length);
            audioSource.clip = footstepSounds[i];
            audioSource.PlayOneShot(audioSource.clip);
            footstepSounds[i] = footstepSounds[0];
            footstepSounds[0] = audioSource.clip;
        }

      

        public Transform ReturnSelf()
        {
            return this.gameObject.transform;
        }
    }
}