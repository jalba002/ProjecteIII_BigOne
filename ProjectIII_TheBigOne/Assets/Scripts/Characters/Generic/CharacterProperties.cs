using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "DATA_CharacterProperties", menuName = "Data/Character Properties")]
    public class CharacterProperties : ScriptableObject
    {
        [Header("Speeds")]
        public float WalkSpeed = 1f;
        public float RunSpeed = 2f;

        [Space(10)]
        [Header("Running")]
        [Tooltip("Maximum time in seconds the player can run without resting.")] public float maximumStamina = 5f;
        [Tooltip("Time before stamina starts regenerating.")] public float rechargeDelay = 0f;
        [Tooltip("Amount of stamina regenerated per second.")] public float staminaRechargePerSecond = 2f;

    }
}
