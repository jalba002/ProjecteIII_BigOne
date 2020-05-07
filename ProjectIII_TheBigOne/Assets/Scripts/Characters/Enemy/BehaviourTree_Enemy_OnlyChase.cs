using Enemy;
using UnityEngine;
using CharacterController = Characters.Generic.CharacterController;

public class BehaviourTree_Enemy_OnlyChase : BehaviourTree
{
    protected new EnemyController attachedCharacter;

    public BehaviourTree_Enemy_OnlyChase(CharacterController characterController)
    {
        base.Setup(characterController);
        attachedCharacter = (EnemyController) characterController;
    }

    public override void CalculateNextState(bool forceExitState)
    {
        if (CheckEnterJustChase()) return;
    }
    
    private bool CheckEnterJustChase()
    {
        attachedCharacter.stateMachine.SwitchState<State_Enemy_Moving>();
        return true;
    }
}