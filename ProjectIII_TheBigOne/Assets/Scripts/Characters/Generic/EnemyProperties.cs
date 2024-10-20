﻿using UnityEngine;

namespace Properties
{
    [CreateAssetMenu(fileName = "Data_Enemy_Properties", menuName = "Data/Properties/Enemy Properties")]
    public class EnemyProperties : CharacterProperties
    {
        [Header("Killing")] public float tooCloseRange; // This will ensure the player is heard if too close.

        [Header("Vision")] public float fieldOfVision = 140f;
        public float maxDetectionRange = 10f;
        public LayerMask watchableLayers;

        [Header("Hearing")] public float hearingMaxRange; // This will check if the player is around the hearing area.
        public LayerMask layersThatBlockHearing;
        
        [Header("Patrolling")] public float positionReachedDistance = 1f;
        public float patrolWaitTime = 5f;

        [Header("Searching")] public float maxTimeOfPerfectTracking = 1.25f;
        public float maxSearchTime = 15f;

        [Header("Sound")] public float terrorRadiusRange = 8f;
    }
}