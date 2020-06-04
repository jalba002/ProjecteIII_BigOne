using Enemy;
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
        if (CheckPlayerKilled()) return;
        if (CheckIfHearingPlayer()) return;
        if (CheckEnterTraversing()) return;
        if (CheckEnterPatrol()) return;
        if (CheckEnterChasing()) return;
        if (CheckEnterSearching()) return;
    }

    /// All states for Enemy first phase listed below. ///
    ///
    /// 
    private bool CheckIfHearingPlayer()
    {
        if (!attachedCharacter.currentBrain.IsHearingPlayer) return false;

        if (!attachedCharacter.currentBrain.IsNoticingPlayer) return false;

        if (attachedCharacter.currentBrain.IsChasingPlayer) return false;

        if (attachedCharacter.currentBrain.IsTrackingPlayer) return false;

        attachedCharacter.stateMachine.SwitchState<State_Enemy_Noticed>();
        return true;
    }
    
    private bool CheckPlayerKilled()
    {
        if (!attachedCharacter.currentBrain.IsPlayerCloseEnoughForDeath) return false;

        attachedCharacter.stateMachine.SwitchState<State_Enemy_Killing>();
        
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
        
        attachedCharacter.stateMachine.SwitchState<State_Enemy_DestructionTraversing>();

        return true;
    }
}