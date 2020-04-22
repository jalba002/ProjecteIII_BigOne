using Characters.Brains;
using Player;
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

        [Space(5)] [Header("Properties")]
        public CharacterProperties characterProperties;
    }
}