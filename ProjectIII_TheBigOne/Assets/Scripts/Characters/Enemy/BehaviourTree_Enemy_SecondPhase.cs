using Enemy;
using UnityEngine;
using CharacterController = Characters.Generic.CharacterController;

public class BehaviourTree_Enemy_SecondPhase : BehaviourTree
{
    protected new EnemyController attachedCharacter;

    public BehaviourTree_Enemy_SecondPhase(CharacterController characterController)
    {
        base.Setup(characterController);
        attachedCharacter = (EnemyController) characterController;
    }

    public override void CalculateNextState(bool forceExitState)
    {
        // Analyze all possible states.
        if (CheckEnterMoving()) return;
        if (CheckEnterIdle()) return;
    }


    /// All states for Enemy first phase listed below. ///
    private bool CheckEnterDisable()
    {
        // TODO Add Conditions.

        return true;
    }


    private bool CheckEnterIdle()
    {
        // The player is not going anywhere.
        
        // if (!attachedCharacter.currentBrain.IsVisible) return false;
        
        // if (!attachedCharacter.currentBrain.IsPlayerNearLight) return false;

        attachedCharacter.stateMachine.SwitchState<State_Enemy_Idle>();
        return true;
    }

    private bool CheckEnterMoving()
    {
        // TODO Add conditions.
        // If player is not in safe zone.
        // If lost the player in the chase.
        // Debug.Log("EnterPatrol Check");

        //if (!attachedCharacter.currentBrain.IsChasingPlayer) return false;

        attachedCharacter.stateMachine.SwitchState<State_Enemy_Moving>();
        return true;
    }

    private bool CheckEnterChasing()
    {
        // TODO Add conditions.
        // 
        return true;
    }
}