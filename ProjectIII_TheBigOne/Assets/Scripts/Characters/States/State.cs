using Characters.Generic;
using UnityEngine;

namespace Player
{
    public abstract class State : MonoBehaviour
    {
        [HideInInspector] public StateMachine Machine;

        /// <summary>
        /// Base initialization, do not modify. Called automatically.
        /// </summary>
        /// <param name="machine">
        /// StateMachine the state is attached to</param>
        public virtual void Initialize(StateMachine machine)
        {
            this.Machine = machine;
            OnStateInitialize();
        }

        /// <summary>
        /// OnStateInitialize allows to make changes or set variables at the creation of the component.
        /// This can be modified when inherited. Remember to use base.OnStateInitialize().
        /// </summary>
        /// <param name="machine"></param>
        protected virtual void OnStateInitialize(StateMachine machine = null)
        {
            enabled = false;
        }


        /// <summary>
        /// Automatic update. Add all Update functions here.
        /// </summary>
        public virtual void OnStateTick(float deltaTime)
        {
        }

        /// <summary>
        /// Automatic FixedUpdate. Add all FixedUpdate updates here. 
        /// </summary>
        public virtual void OnStateFixedTick(float fixedDeltaTime)
        {
        }


        /// <summary>
        /// Called automatically after OnStateTick, add all transition checks here.
        /// </summary>
        public virtual void OnStateCheckTransition()
        {
        }

        /// <summary>
        /// Automatic state enter. DO NOT MODIFY. Change OnStateEnter instead.
        /// </summary>
        public void StateEnter()
        {
            enabled = true;
            OnStateEnter();
        }

        /// <summary>
        /// Automatic state exit. DO NOT MODIFY. Change OnStateExit instead.
        /// </summary>
        public void StateExit()
        {
            OnStateExit();
            enabled = false;
        }

        /// <summary>
        /// This is executed on all inherited classes. Use it if you need to set variables.
        /// </summary>
        protected virtual void OnStateEnter()
        {
        }

        /// <summary>
        /// This is executed on all inherited classes. Use it if you need to set variables.
        /// </summary>
        protected virtual void OnStateExit()
        {
        }

        //No need to modify this zone.

        #region Other

        public static implicit operator bool(State state)
        {
            return state != null;
        }

        public void OnEnable()
        {
            if (Machine == null) return;
            if (this != Machine.GetCurrentState && Application.isPlaying)
            {
                enabled = false;
                Debug.LogWarning("Do not enable States directly. Use StateMachine.SwitchState");
            }
        }

        private void OnDisable()
        {
            if (Machine == null) return;
            if (this == Machine.GetCurrentState && Application.isPlaying)
            {
                enabled = true;
                Debug.LogWarning("Do not disable States directly. Use StateMachine.SwitchState");
            }
        }

        #endregion
    }
}