using System.Collections;
using System.Collections.Generic;
using Enemy;
using Player;
using UnityEngine;
using CharacterController = Characters.Generic.CharacterController;

public class BehaviourTree_Enemy_FirstPhase : BehaviourTree
{
    protected new EnemyController attachedCharacter; 
    public BehaviourTree_Enemy_FirstPhase(CharacterController characterController)
    {
        base.Setup(characterController);
        attachedCharacter = (EnemyController)characterController;
    }
    
    public override void CalculateNextState(bool forceExitState)
    {
        // Analyze all possible states.
        if (CheckEnterIdle()) return;
        if (CheckEnterPatrol()) return;
    }


    /// All states for Enemy first phase listed below. ///
    ///

    private bool CheckEnterDisable()
    {
        // TODO Add Conditions.
        
        return true;
    }


    private bool CheckEnterIdle()
    {
        // TODO Add conditions
        // If player is not looking at him, exit.
        // If player has the light turned off, exit.
        Debug.Log("EnterIdle Check");
        if (!attachedCharacter.currentBrain.IsVisible) return false;
            
        attachedCharacter.stateMachine.SwitchState<State_Enemy_Idle>();
        return true;
    }

    private bool CheckEnterPatrol()
    {
        // TODO Add conditions.
        // If player is not in safe zone.
        // If lost the player in the chase.
        Debug.Log("EnterPatrol Check");
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
