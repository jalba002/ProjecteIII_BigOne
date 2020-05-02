using Enemy;
using UnityEngine;
using CharacterController = Characters.Generic.CharacterController;

public class BehaviourTree_Enemy_Halted : BehaviourTree
{
    protected new EnemyController attachedCharacter;

    public BehaviourTree_Enemy_Halted(CharacterController characterController)
    {
        base.Setup(characterController);
        attachedCharacter = (EnemyController) characterController;
    }

    public override void CalculateNextState(bool forceExitState)
    {
        // Analyze all possible states.
        if(DoNothing()) return;
    }


    /// All states for Enemy first phase listed below.
    public bool DoNothing()
    {
        // Does nothing.
        attachedCharacter.stateMachine.SwitchState<State_Enemy_Halt>();
        return true;
    }
}