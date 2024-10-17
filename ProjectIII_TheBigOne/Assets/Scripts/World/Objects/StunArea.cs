using Tavaris.Dynamic;
using Tavaris.Entities;
using UnityEngine;

namespace Tavaris.Objects
{
    public class StunArea : MonoBehaviour
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
    }
}