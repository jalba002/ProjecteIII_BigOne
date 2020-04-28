using System;
using Characters.Brains;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

// This could be the AI brain that makes decisions based on events and stuff.
public class EnemyBrain : Brain
{
    public NavMeshAgent _NavMeshAgent { get; protected set; }

    public bool IsVisible { get; set; }

    private void Awake()
    {
        _NavMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        IsVisible = false;
    }

    public override void GetActions()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            IsVisible = !IsVisible;
    }
}