using System.Collections;
using System.Collections.Generic;
using Enemy;
using Player;
using UnityEngine;


public abstract class BehaviourTree
{
    protected Characters.Generic.CharacterController attachedCharacter;

    public void Setup(Characters.Generic.CharacterController characterController)
    {
        this.attachedCharacter = characterController;
    }

    public abstract void CalculateNextState(bool forceExitState);

    /// DEFAULT TRANSITIONS ///

    protected bool CheckDeadState()
    {
        if (!attachedCharacter.IsDead)
        {
            return false;
        }

        attachedCharacter.stateMachine.SwitchState<State_Generic_Dead>();
        return true;
    }
}
