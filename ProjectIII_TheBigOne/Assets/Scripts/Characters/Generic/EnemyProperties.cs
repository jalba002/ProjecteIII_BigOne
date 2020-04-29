using Player;
using UnityEngine;

namespace Properties
{
    [CreateAssetMenu(fileName = "Data_Enemy_Properties", menuName = "Data/Properties/Enemy Properties")]
    public class EnemyProperties : CharacterProperties
    {
        [Header("Sensing Config")]
        public float maxDetectionRange = 10f;
        public LayerMask watchableLayers;
        
        [Header("Chase Config")] 
        [Tooltip("Maximum time the enemy will wait after losing the player.")] public float maxChaseLostWaitTime = 10f;
        //[Tooltip("Prototype tooltip")] public float chaseConfigValue = 2f;
    }
}
