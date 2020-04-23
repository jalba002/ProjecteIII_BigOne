using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Characters.Generic
{
    public class StateMachine : MonoBehaviour
    {
        protected List<State> statesList = new List<State>();

        //public State initialState;
        protected State currentState;

        public CharacterController characterController;
        [Header("Debug Options")] public bool enableDebug = false;
        
        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        public void UpdateTick(float deltaTime)
        {
            currentState.OnStateTick(deltaTime);
            currentState.OnStateCheckTransition();
        }

        public void FixedUpdateTick(float fixedTime)
        {
            currentState.OnStateFixedTick(fixedTime);
        }

        /// <summary>
        /// Switch the currentstate to a specific new State.
        /// </summary>
        /// <param name="newState">
        /// The state object to set as the currentState</param>
        /// <returns></returns>
        protected virtual State SetState(State newState)
        {
            if (newState && newState != currentState)
            {
                State oldState = currentState;
                currentState = newState;
                if (oldState)
                    oldState.StateExit();
                currentState.StateEnter();
                if (enableDebug)
                {
                    try
                    {
                        Debug.Log($"{oldState.GetType()} to {currentState.GetType()}");
                    }
                    catch (NullReferenceException)
                    {
                        Debug.Log($"{currentState.GetType()}");
                    }
                }

                return currentState;
            }

            return null;
        }

        public void ForceSetState(State state)
        {
            SetState(state);
        }

        /// <summary>
        /// Switches the currentState to a State of a given type, checking if it already exists.
        /// </summary>
        /// <typeparam name="StateType">
        /// The type of the State to use for the currentState</typeparam>
        /// <returns>Whether the State was changed</returns>
        public virtual State SwitchState<StateType>() where StateType : State
        {
            // if the state can be found in the list of states.
            // already created, switch to the existing version.
            foreach (State state in statesList)
            {
                if (state is StateType)
                {
                    return SetState(state);
                }
            }

            State stateComponent = GetComponent<StateType>();
            if (stateComponent)
            {
                stateComponent.Initialize(this);
                statesList.Add(stateComponent);
                return SetState(stateComponent);
            }

            // if the state is not found in the list make a new instance.
            State newState = gameObject.AddComponent<StateType>();
            newState.Initialize(this);
            statesList.Add(newState);
            return SetState(newState);
        }

        /// <summary>
        /// Returns current machine state.
        /// </summary>
        public State GetCurrentState
        {
            get { return currentState; }
        }
    }
}