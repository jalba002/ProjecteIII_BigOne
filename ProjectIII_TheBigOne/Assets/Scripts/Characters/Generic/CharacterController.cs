using Characters.Brains;
using Player;
using Properties;
using UnityEngine;

namespace Characters.Generic
{
    public abstract class CharacterController : MonoBehaviour
    {
        //[Header("Required Components")] 
        [System.NonSerialized]
        public Brain currentBrain;
        public StateMachine stateMachine { get; protected set; }
        public Rigidbody rigidbody;
        
        public bool IsDead { get; protected set; }

        [System.NonSerialized]
        public CharacterProperties characterProperties;

        public virtual bool Kill()
        {
            return false;
        }
    }
}