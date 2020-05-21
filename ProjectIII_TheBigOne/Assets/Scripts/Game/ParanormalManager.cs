using System;
using Enemy;
using Game.Inputs;
using UnityEngine;
using World.Objects;
using Random = System.Random;

// Also known as the cunt that always tries to scare you.
public class ParanormalManager : MonoBehaviour
{
    [Header("Spawn points.")] public Transform firstSpawnPoint;
    public Transform secondSpawnPoint;
    public Transform relocationSpawnPoint;

    [Header("Audio Settings")] public AudioClip killerLaugh;
    public AudioSource ParanormalSoundEmitter;

    [Header("Enemy")] public EnemyController Dimitry;
    public EnemyTargetDummy enemyTargetDummy;

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

    public static void OpenDoor(DynamicObject door)
    {
        //throw new NotImplementedException();
        //door.Use();
        door.StrongOpening();
    }

    public static void CloseDoor(DynamicObject door)
    {
        door.StrongClosing();
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
        Debug.Log("Starting Dimitry First Phase");

        if (!Dimitry.gameObject.activeSelf)
        {
            Debug.LogWarning("Dimitry is disabled in this scene.");
            return;
        }

        SetEnemyPosition(firstSpawnPoint);

        //SetDummyParent(Dimitry.currentBrain.archnemesis.transform);

        //SetDummyLocalPosition(Vector3.zero);

        Dimitry.currentBehaviourTree = new BehaviourTree_Enemy_FirstPhase(Dimitry);
    }

    public void StartSecondPhase()
    {
        // TODO Play spooky sound.
        // Move dimitry away.
        // Set new behaviour.
        Debug.Log("Starting Dimitry Second Phase");

        if (!Dimitry.gameObject.activeSelf)
        {
            Debug.LogWarning("Dimitry is disabled in this scene.");
            return;
        }

        try
        {
            ParanormalSoundEmitter.Play();
        }
        catch (Exception error)
        {
            Debug.LogWarning(error.Message);
        }

        SetEnemyPosition(secondSpawnPoint);

        //Dimitry.gameObject.SetActive(true);

        //SetDummyParent(Dimitry.currentBrain.archnemesis.transform);

        //SetDummyLocalPosition(Vector3.zero);
        
        PalletsSetDestructible(true);

        Dimitry.currentBehaviourTree = new BehaviourTree_Enemy_SecondPhase(Dimitry);
    }

    private void SetEnemyPosition(Transform newPosition)
    {
        if (newPosition == null || Dimitry == null)
        {
            Debug.LogWarning("Not moving Dimitry. Position is null.");
            return;
        }

        Dimitry.NavMeshAgent.Warp(newPosition.position);
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

    private void PalletsSetDestructible(bool enable)
    {
        Debug.Log("Pallets can now be destructed!");
        foreach (TraversableBlockage pallet in ObjectTracker.palletList)
        {
            pallet.attachedLink.activated = enable;
        }
    }
}