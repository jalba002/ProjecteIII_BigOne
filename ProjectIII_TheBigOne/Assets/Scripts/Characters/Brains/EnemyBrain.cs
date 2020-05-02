using System;
using Characters.Brains;
using Enemy;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : Brain
{
    public EnemyController selfCharacter { get; protected set; }
    public PlayerController archnemesis { get; protected set; }

    // All sensing variables should be changed from the states.
    // So every moment they can be overwritten by whatever we want.
    public bool IsVisible { get; set; }
    
    public bool IsBeingRendered { get; set; }

    public bool IsPlayerInSight { get; set; }

    public bool IsPlayerNearLight { get; set; }

    public bool IsChasingPlayer { get; set; }

    public bool UpdateRotation { get; set; }

    private void Awake()
    {
        IsVisible = true;
        selfCharacter = this.gameObject.GetComponent<EnemyController>();
        archnemesis = FindObjectOfType<PlayerController>();
    }

    // This method is updated every frame. 
    // It is mostly used to debug functions with shortcuts.
    public override void GetActions()
    {
        /*IsVisible = selfCharacter.meshRenderer.isVisible;
        
        IsPlayerNearLight = SensesUtil.HasFlashlightEnabled(archnemesis);

        IsPlayerInSight = SensesUtil.IsInSight(selfCharacter.gameObject, archnemesis.gameObject,
            selfCharacter.characterProperties.maxDetectionRange, selfCharacter.characterProperties.watchableLayers, false);*/
    }

    public void SetBrainDead()
    {
        IsVisible = true;
        IsPlayerNearLight = false;
        IsPlayerInSight = false;
        IsChasingPlayer = false;
    }
}