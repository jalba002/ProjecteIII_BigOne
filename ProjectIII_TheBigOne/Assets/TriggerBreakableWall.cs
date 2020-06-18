using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TriggerBreakableWall : MonoBehaviour
{
    [Header("Components")]
    public Animator attachedAnimation;
    [Header("Obstacles")]
    public NavMeshObstacle[] obstaclesToDisable;

    [Space(10)]
    private bool alreadyActivated = false;

    public void Start()
    {
        alreadyActivated = false;
    }

    public void Activate()
    {
        if (alreadyActivated) return;

        attachedAnimation.SetTrigger("Break");

        alreadyActivated = true;
    }

    public void DisableObstacles()
    {
        if (obstaclesToDisable.Length <= 0) return;

        foreach (NavMeshObstacle obstacle in obstaclesToDisable)
        {
            obstacle.enabled = false;
        }
    }

    public void PlayWallSound()
    {
        FindObjectOfType<SoundEmitterCalls>()?.PlayBreakingWallSound();
    }

    public void StartSecondPhase()
    {
        FindObjectOfType<ParanormalManager>()?.StartSecondPhase();
    }
}