using Enemy;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace World.Objects
{
    public class StunArea : MonoBehaviour, IInteractable
    {
        [Header("Settings")] public bool TrappedArea = false;
        [Range(0f, 15f)] public float stunDuration = 3f;

        private EnemyController enemyController;

        private bool IsEnemyInsideArea = false;
        private bool AlreadyActivated = false;

        private DynamicObject _attachedDynamicObject;

        private DynamicObject attachedDynamicObject
        {
            get
            {
                if (_attachedDynamicObject == null)
                {
                    _attachedDynamicObject = GetComponentInChildren<DynamicObject>();
                }

                return _attachedDynamicObject;
            }
            set => _attachedDynamicObject = value;
        }

        public TraversableBlockage traversableBlockage;

        void Start()
        {
            if (enemyController == null)
            {
                enemyController = FindObjectOfType<EnemyController>();
            }
            traversableBlockage = GetComponent<TraversableBlockage>();
        }

        public void Update()
        {
            if (attachedDynamicObject.enabled)
            {
                //var value = attachedDynamicObject.HingeJoint.gameObject.transform.rotation.x * Mathf.Rad2Deg;
                //Debug.Log(value);
                
                if ( attachedDynamicObject.HingeJoint.angle > 25f)
                {
                    ActivateStun();
                    /*var newValue = attachedDynamicObject.HingeJoint.angle - 1f * Time.deltaTime;
                    newValue = Mathf.Clamp(newValue, 0f, 87f);
                    var oldMax = attachedDynamicObject.HingeJoint.limits.max;
                    attachedDynamicObject.HingeJoint.limits = new JointLimits()
                    {
                        min = newValue,
                        max = oldMax
                    };*/
                }
            }

           /* if (attachedDynamicObject.HingeJoint.angle >= 87f)
            {
               
                // Destroy(this.gameObject);
            }*/
        }

        public bool ActivateStun()
        {
            if (IsEnemyInsideArea && !AlreadyActivated)
            {
                ForceStun();
                AlreadyActivated = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ForceStun()
        {
            enemyController.currentBrain.Stun(this);
        }

        public bool RestartArea()
        {
            return false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == enemyController.gameObject)
            {
                if (TrappedArea)
                {
                    ActivateStun();
                }

                IsEnemyInsideArea = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject == enemyController.gameObject)
            {
                IsEnemyInsideArea = false;
            }
        }

        public bool Interact()
        {
            return ActivateStun();
        }

        public void OnStartInteract()
        {
            //throw new System.NotImplementedException();
        }

        public void OnInteracting()
        {
            //throw new System.NotImplementedException();
        }

        public void OnEndInteract()
        {
            //throw new System.NotImplementedException();
        }
    }
}