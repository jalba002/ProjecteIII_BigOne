﻿using System;
using Enemy;
using Game.Inputs;
using UnityEngine;
using World.Objects;
using Random = System.Random;

// Also known as the cunt that always tries to scare you.
public class ParanormalManager : MonoBehaviour
{
    public EnemyTargetDummy enemyTargetDummy;

    public Transform firstSpawnPoint;
    public Transform secondSpawnPoint;
    public Transform relocationSpawnPoint;

    public AudioClip killerLaugh;
    public AudioSource ParanormalSoundEmitter;

    public EnemyController Dimitry;

    private static Random alea = new Random();

    public void Awake()
    {
        Dimitry = FindObjectOfType<EnemyController>();
        
        if (enemyTargetDummy == null)
        {
            enemyTargetDummy = FindObjectOfType<EnemyTargetDummy>();
            if (enemyTargetDummy == null)
            {
                var newDummy = new GameObject("EnemyTargetDummy");
                enemyTargetDummy = newDummy.AddComponent<EnemyTargetDummy>();
                SetDummyPosition(Vector3.zero);
            }
        }
    }

    public void Start()
    {
        
    }

    public static void UseRandomDoor()
    {
        UseDoor(ObjectTracker.doorList[alea.Next(0, ObjectTracker.doorList.Count)]);
    }

    public static void UseDoor(DynamicObject door)
    {
        throw new NotImplementedException();
        //door.Use();
    }

    /*public float CalculateRandomForce(float scale = 1f)
    {
        return Mathf.Sin(accumulatedTime) * scale;
    }*/

    public void StartFirstPhase()
    {
        // TODO Play spooky sound.
        // Move dimitry away.
        // Set new behaviour.
        if (!Dimitry.gameObject.activeSelf)
        {
            Debug.LogWarning("Dimitry is disabled in this scene.");
            return;
        }
        Debug.Log("Starting Dimitry First Phase");
        Dimitry.gameObject.SetActive(false);
        SetEnemyPosition(firstSpawnPoint);
        SetDummyParent(Dimitry.currentBrain.archnemesis.transform);
        //SetDummyLocalPosition(Vector3.zero);
        Dimitry.currentBehaviourTree = new BehaviourTree_Enemy_FirstPhase(Dimitry);
        Dimitry.gameObject.SetActive(true);
    }

    public void StartSecondPhase()
    {
        // TODO Play spooky sound.
        // Move dimitry away.
        // Set new behaviour.
        if (!Dimitry.gameObject.activeSelf)
        {
            Debug.LogWarning("Dimitry is disabled in this scene.");
            return;
        }
        
        ParanormalSoundEmitter.Play();
        Dimitry.gameObject.SetActive(true);
        Debug.Log("Starting Dimitry Second Phase");
        SetDummyParent(Dimitry.currentBrain.archnemesis.transform);
        //SetDummyLocalPosition(Vector3.zero);
        SetEnemyPosition(secondSpawnPoint);
        Dimitry.currentBehaviourTree = new BehaviourTree_Enemy_SecondPhase(Dimitry);
    }

    private void SetEnemyPosition(Transform newPosition)
    {
        if (newPosition == null)
        {
            Debug.LogWarning("Not moving Dimitry. Position is null.");
            return;
        }
        Dimitry.transform.position = newPosition.position;
    }

    public void SetDummyPosition(Vector3 newPosition)
    {
        enemyTargetDummy.transform.position = newPosition;
    }

    public void SetDummyLocalPosition(Vector3 newPosition)
    {
        enemyTargetDummy.transform.localPosition = newPosition;
    }

    public void SetDummyParent(Transform newParent)
    {
        enemyTargetDummy.transform.parent = newParent;
        SetDummyLocalPosition(Vector3.zero);
    }

    public void TeleportAway(Transform newPosition)
    {
        SetEnemyPosition(newPosition);
    }
}