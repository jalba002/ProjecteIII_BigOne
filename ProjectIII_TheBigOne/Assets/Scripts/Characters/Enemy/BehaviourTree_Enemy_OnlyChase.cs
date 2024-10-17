using Tavaris.Entities;
using Tavaris.States;

namespace Tavaris.AI
{
    public class BehaviourTree_Enemy_OnlyChase : BehaviourTree
    {
        protected new EnemyController attachedCharacter;

        public BehaviourTree_Enemy_OnlyChase(EnemyController characterController)
        {
            base.Setup(characterController);
            attachedCharacter = characterController;
        }

        public override void CalculateNextState(bool forceExitState)
        {
            if (CheckEnterJustChase()) return;
        }

        private bool CheckEnterJustChase()
        {
            attachedCharacter.stateMachine.SwitchState<State_Enemy_Patrolling>();
            return true;
        }
    }
}