using UnityEngine;

namespace Characters.Brains
{
    public abstract class Brain : MonoBehaviour
    {
        public bool Interact { get; protected set; }
        public bool Running { get; protected set; }
        
        /* bool Cancel { get; protected set; }
        public bool Submit { get; protected set; }*/
        
        public Vector2 MouseInput { get; protected set; }
        
        public float MouseWheel { get; protected set; }
        public Vector3 Direction { get; protected set; }

        public abstract void GetActions();
    }
}