using Characters.Brains;
using Properties;
using Tavaris.States;
using UnityEngine;

namespace Tavaris.Entities
{
    public abstract class EntityController : MonoBehaviour
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