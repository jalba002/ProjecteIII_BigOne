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
        if (CheckEnterStunned()) return;
        if (CheckEnterTraversing()) return;
        if (CheckEnterPatrol()) return;
        if (CheckEnterChasing()) return;
        if (CheckEnterSearching()) return;
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
        
        if (attachedCharacter.currentBrain.IsPlayerInSight) return false;

        if (attachedCharacter.currentBrain.IsChasingPlayer) return false;

        attachedCharacter.stateMachine.SwitchState<State_Enemy_Patrolling>();
        return true;
    }
    
    private bool CheckEnterChasing()
    {
        // TODO Add conditions.
        // 
        
        if (!attachedCharacter.currentBrain.IsPlayerInSight) return false;
        
        attachedCharacter.stateMachine.SwitchState<State_Enemy_Chasing>();
        return true;
    }

    private bool CheckEnterSearching()
    {
        //if (attachedCharacter.currentBrain.IsChasingPlayer) return false;

        //if (attachedCharacter.currentBrain.IsPlayerInSight) return false;
        
        attachedCharacter.stateMachine.SwitchState<State_Enemy_Searching>();
        return true;
    }

    private bool CheckEnterStunned()
    {
        if (!attachedCharacter.currentBrain.IsStunned) return false;
        
        attachedCharacter.stateMachine.SwitchState<State_Enemy_Stunned>();
        
        return true;
    }

    private bool CheckEnterTraversing()
    {
        if (!attachedCharacter.currentBrain.IsOnOffMeshLink) return false;

        //if (attachedCharacter.currentBrain.IsCurrentlyBreaking) return false;
        
        attachedCharacter.stateMachine.SwitchState<State_Enemy_Traversing>();

        return true;
    }
}