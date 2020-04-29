using Characters.Brains;
using Player;
using Properties;
using UnityEngine;

namespace Characters.Generic
{
    public abstract class CharacterController : MonoBehaviour
    {
        //[Header("Required Components")] 
        public Brain defaultBrain{ get; protected set; }
        public Brain currentBrain { get; protected set; }
        public State defaultState { get; protected set; }
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