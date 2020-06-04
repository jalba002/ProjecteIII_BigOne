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
        attachedCharacter.NavMeshAgent.autoTraverseOffMeshLink = false;
        
    }

    public override void CalculateNextState(bool forceExitState)
    {
        // Analyze all possible states.
        if (CheckEnterStunned()) return;
        if (CheckPlayerKilled()) return;
        if (CheckIfHearingPlayer()) return;
        if (CheckEnterTraversing()) return;
        if (CheckEnterIdle()) return;
        if (CheckEnterPatrol()) return;
        if (CheckEnterChasing()) return;
        if (CheckEnterSearching()) return;
    }

    private bool CheckPlayerKilled()
    {
        if (!attachedCharacter.currentBrain.IsPlayerCloseEnoughForDeath) return false;

        attachedCharacter.stateMachine.SwitchState<State_Enemy_Killing>();
        
        return true;
    }

    private bool CheckIfHearingPlayer()
    {
        if (!attachedCharacter.currentBrain.IsHearingPlayer) return false;

        if (!attachedCharacter.currentBrain.IsNoticingPlayer) return false;

        if (attachedCharacter.currentBrain.IsChasingPlayer) return false;

        if (attachedCharacter.currentBrain.IsTrackingPlayer) return false;

        attachedCharacter.stateMachine.SwitchState<State_Enemy_Noticed>();
        return true;
    }
    
    private bool CheckEnterIdle()
    {
        // TODO Add conditions
        // If player is looking at the enemy.
        // If player has the light turned off.

        if (!attachedCharacter.currentBrain.IsPlayerInSight) return false;

        if (!attachedCharacter.currentBrain.IsVisible) return false;
        
        if (!attachedCharacter.currentBrain.IsPlayerNearLight) return false;

        attachedCharacter.stateMachine.SwitchState<State_Enemy_Idle>();
        return true;
    }

    private bool CheckEnterPatrol()
    {
        if (attachedCharacter.currentBrain.IsPlayerInSight) return false;

        if (attachedCharacter.currentBrain.IsChasingPlayer) return false;

        if (attachedCharacter.currentBrain.IsTrackingPlayer) return false;

        attachedCharacter.stateMachine.SwitchState<State_Enemy_Patrolling>();
        return true;
    }
    
    private bool CheckEnterChasing()
    {
        if (!attachedCharacter.currentBrain.IsPlayerInSight) return false;
        
        attachedCharacter.stateMachine.SwitchState<State_Enemy_Chasing>();
        return true;
    }

    private bool CheckEnterSearching()
    {
        //if (attachedCharacter.currentBrain.IsChasingPlayer) return false;

        //if (!attachedCharacter.currentBrain.IsTrackingPlayer) return false;
        
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
        // attachedCharacter.NavMeshAgent.CompleteOffMeshLink();
        attachedCharacter.stateMachine.SwitchState<State_Enemy_Traversing>();

        return true;
    }
}