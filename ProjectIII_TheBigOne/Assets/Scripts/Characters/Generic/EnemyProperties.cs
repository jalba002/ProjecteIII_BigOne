using UnityEngine;

namespace Properties
{
    [CreateAssetMenu(fileName = "Data_Enemy_Properties", menuName = "Data/Properties/Enemy Properties")]
    public class EnemyProperties : CharacterProperties
    {
        [Header("Sensing Config")] public float maxDetectionRange = 10f;
        public LayerMask watchableLayers;

        [Header("Patrol Config")] public float positionReachedDistance = 1f;
        public float patrolWaitTime = 5f;

        [Header("Search Config")] public float maxTimeOfPerfectTracking = 1.25f;
        public float maxSearchTime = 15f;

        [Header("Terror Radius")] public float terrorRadiusRange = 8f;
    }
}
