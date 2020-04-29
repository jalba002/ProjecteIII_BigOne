using UnityEngine;

// Data needed to create each input.
namespace Game.Inputs
{
    [System.Serializable]
    public struct InputNames
    {
        [Tooltip("Exact name of the input in Unity Settings")]
        public string name;
        [Tooltip("Toggles between GetButtonDown or GetButton")]
        public bool holdButton;
    }
}

