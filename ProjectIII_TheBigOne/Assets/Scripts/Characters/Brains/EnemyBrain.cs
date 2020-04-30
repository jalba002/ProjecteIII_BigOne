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
    public NavMeshAgent _NavMeshAgent { get; protected set; }
    public PlayerController archnemesis { get; protected set; }

    public bool IsVisible { get; private set; }

    public bool IsPlayerInSight { get; private set; }

    public bool IsPlayerNearLight { get; private set; }

    public bool IsChasingPlayer { get; private set; }

    public bool UpdateRotation { get; private set; }

    private void Awake()
    {
        IsVisible = true;
        _NavMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        selfCharacter = this.gameObject.GetComponent<EnemyController>();
        archnemesis = FindObjectOfType<PlayerController>();
    }

    // This method is updated every frame. 
    // It is mostly used to debug functions with shortcuts.
    public override void GetActions()
    {
        IsVisible = selfCharacter.meshRenderer.isVisible;
        
        IsPlayerNearLight = SensesUtil.HasFlashlightEnabled(archnemesis);

        IsPlayerInSight = SensesUtil.IsInSight(selfCharacter.gameObject, archnemesis.gameObject,
            selfCharacter.characterProperties.maxDetectionRange, selfCharacter.characterProperties.watchableLayers, false);
    }
    
    
}