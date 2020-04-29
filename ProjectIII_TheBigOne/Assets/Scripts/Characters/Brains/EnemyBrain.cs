using System;
using Characters.Brains;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

// This could be the AI brain that makes decisions based on events and stuff.
public class EnemyBrain : Brain
{
    public NavMeshAgent _NavMeshAgent { get; protected set; }

    public bool IsVisible = false;

    public bool PlayerNearLight = false;

    public bool IsChasing = false;

    public bool UpdateRotation = true;

    private void Awake()
    {
        _NavMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        IsVisible = false;
    }

    // This method is updated every frame. 
    // It is mostly used to debug functions with shortcuts.
    public override void GetActions()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            IsVisible = !IsVisible;
    }
}