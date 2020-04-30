using UnityEngine;

// Scriptable object created for external configuration.
// If inputs are added or removed, modify the class below.
namespace Game.Inputs
{
    [CreateAssetMenu(fileName = "DATA_Default_Inputs", menuName = "Inputs/Input List")]
    public class Inputs : ScriptableObject
    {
        public InputNames interact;
        public InputNames mouseInteract;
        public InputNames flashlight;
        public InputNames running;
        public InputNames horizontal;
        public InputNames forward;
    }
}