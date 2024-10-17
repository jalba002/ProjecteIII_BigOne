using Tavaris.Entities;

namespace Tavaris.States
{
    public class State_Enemy_Halt : State
    {
        private EnemyController _attachedController;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = (EnemyController)Machine.characterController;
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);
        }

        public override void OnStateFixedTick(float fixedTime)
        {
            base.OnStateFixedTick(fixedTime);
        }

        public override void OnStateCheckTransition()
        {
            base.OnStateCheckTransition();
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();

            _attachedController.NavMeshAgent.isStopped = true;
            _attachedController.currentBrain.SetBrainDead();
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}