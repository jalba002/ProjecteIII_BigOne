using Enemy;
using UnityEngine;
using CharacterController = Characters.Generic.CharacterController;

public class BehaviourTree_Enemy_FirstPhase : BehaviourTree
{
    protected new EnemyController attachedCharacter;

    public BehaviourTree_Enemy_FirstPhase(CharacterController characterController)
    {
        base.Setup(characterController);
        attachedCharacter = (EnemyController) characterController;
    }

    public override void CalculateNextState(bool forceExitState)
    {
        // Analyze all possible states.
        if (CheckEnterIdle()) return;
        if (CheckEnterPatrol()) return;
        if (CheckEnterChasing()) return;
    }


    /// All states for Enemy first phase listed below. ///
    private bool CheckEnterDisable()
    {
        // TODO Add Conditions.

        return true;
    }


    private bool CheckEnterIdle()
    {
        // TODO Add conditions
        // If player is looking at the enemy.
        // If player has the light turned off.

        Debug.Log("EnterIdle Check");

        if (!attachedCharacter.currentBrain.IsVisible) return false;
        
        if (!attachedCharacter.currentBrain.IsPlayerNearLight) return false;

        attachedCharacter.stateMachine.SwitchState<State_Enemy_Idle>();
        return true;
    }

    private bool CheckEnterPatrol()
    {
        // TODO Add conditions.
        // If player is not in safe zone.
        // 
        
        //Debug.Log("EnterPatrol Check");
        if (!attachedCharacter.currentBrain.IsPlayerInSight) return false;

        if (attachedCharacter.currentBrain.IsChasingPlayer) return false;

        attachedCharacter.stateMachine.SwitchState<State_Enemy_Patrolling>();
        return true;
    }

    private bool CheckEnterChasing()
    {
        // TODO Add conditions.
        // 
        
        Debug.Log("EnterPatrol Chasing");
        //if (!attachedCharacter.currentBrain.IsChasingPlayer) return false;
        
        attachedCharacter.stateMachine.SwitchState<State_Enemy_Chasing>();
        return true;
    }
}