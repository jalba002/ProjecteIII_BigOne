namespace Tavaris.States
{
    public class State_Generic_Dead : State
    {
        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
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
            Machine.characterController.Kill();
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}