using Characters.Generic;
using Player;
using UnityEngine;

namespace Enemy
{
    public class State_Enemy_Killing : State
    {
        private EnemyController _attachedController;
        private float killTime;

        protected override void OnStateInitialize(StateMachine machine)
        {
            base.OnStateInitialize(machine);
            _attachedController = (EnemyController) Machine.characterController;
        }

        public override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);
            if (killTime > 0f)
            {
                killTime -= deltaTime;
            }
            else if (killTime <= 0f)
            {
                // do stuff.
                //Maybe disable killing stuff.
            }
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
            Debug.Log("TIME TO KILLLLLLLLL");
            _attachedController.NavMeshAgent.isStopped = true;
            killTime = 3f;
            GameManager.Instance.EndGame();
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}