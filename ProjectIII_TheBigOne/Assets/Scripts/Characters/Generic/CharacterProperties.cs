using UnityEngine;

namespace Properties
{
    public class CharacterProperties : ScriptableObject
    {
        [Header("Speeds")] public float SlowWalkSpeed = 0.5f;
        public float WalkSpeed = 1f;
        public float RunSpeed = 2f;
    }
}