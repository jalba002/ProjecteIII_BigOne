using Tavaris.Entities;
using Tavaris.States;

namespace Tavaris.AI
{
    public abstract class BehaviourTree
    {
        protected EntityController attachedCharacter;

        public void Setup(EntityController characterController)
        {
            this.attachedCharacter = characterController;
        }

        public abstract void CalculateNextState(bool forceExitState);

        /// DEFAULT TRANSITIONS ///

        protected bool CheckDeadState()
        {
            if (!attachedCharacter.IsDead)
            {
                return false;
            }

            attachedCharacter.stateMachine.SwitchState<State_Generic_Dead>();
            return true;
        }
    }
}