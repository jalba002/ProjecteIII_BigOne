using Characters.Brains;
using Characters.Generic;
using TMPro.Examples;
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
        public CameraController cameraController;

        // public bool enableAirControl { get; set; }
        public Collider attachedCollider;

        public SimpleActivator simpleActivator;

        /*[Header("Sound settings")]
        public AudioClip[] m_FootstepSounds;
        public AudioClip m_JumpSound;
        public AudioClip m_LandSound;*/

        /*[Range(0.1f, 2.0f)] public float m_StepTimeWaking;
        private float m_StepTimeRange;
        private float m_StepTime = 0.0f;
        private AudioSource m_AudioSource;*/

        /*[Header("Ground Detection")] public Transform groundPosition;
        [Range(0.01f, 1f)] public float castRadius = 1f;
        public LayerMask walkableLayers;
        public bool isPlayerGrounded { get; private set; }*/

        public void Awake()
        {
            if (defaultBrain == null)
            {
                defaultBrain = GetComponent<Brain>();
            }

            currentBrain = defaultBrain;

            if (defaultState == null)
                defaultState = GetComponent<State>();

            if (stateMachine == null)
                stateMachine = GetComponent<StateMachine>();

            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody>();

            if (cameraController == null)
                cameraController = GetComponent<CameraController>();

            if (simpleActivator == null)
                simpleActivator = GetComponent<SimpleActivator>();

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

            // audioSource = GetComponent<AudioSource>();
            /*m_StepTime = Time.time + m_StepTimeRange;
            m_StepTimeRange = m_StepTimeWaking;*/
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
            
            InteractDoors();
        }

        void InteractDoors()
        {
            if (simpleActivator != null && simpleActivator.isActiveAndEnabled && currentBrain.Interact)
            {
                if (simpleActivator.Activate(cameraController.attachedCamera))
                {
                    cameraController.angleLocked = true;
                }
                else if (simpleActivator.Deactivate())
                {
                    cameraController.angleLocked = false;
                }
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

        /* private bool CheckOnGround()
         {
             bool foundGround;
             var list = Physics.OverlapSphere(groundPosition.position, castRadius, walkableLayers);
             return list.Length > 0;
         }*/

        // public bool IsGrounded() => isPlayerGrounded;

        /*public void ChangeMaterialFriction(bool grounded)
        {
            if (grounded)
            {
                attachedCollider.material = onGroundMaterial;
            }
            else
            {
                attachedCollider.material = onAirMaterial;
            }
        }*/

        /*private void PlayFootStepAudio()
        {
            if (!IsGrounded())
                return;
    
            int i = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[i];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            m_FootstepSounds[i] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }*/

        /*public void LandSound()
        {
            m_AudioSource.PlayOneShot(m_LandSound);
        }*/

        public Transform ReturnSelf()
        {
            return this.gameObject.transform;
        }
    }
}