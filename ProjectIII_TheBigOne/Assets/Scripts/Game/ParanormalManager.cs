using System;
using Enemy;
using Game.Inputs;
using UnityEngine;
using World.Objects;
using Random = System.Random;

// Also known as the cunt that always tries to scare you.
public class ParanormalManager : MonoBehaviour
{
    private EnemyController Dimitry;

    public float accumulatedTime = 0f;

    private static Random alea = new Random();

    public void Start()
    {
        accumulatedTime = 0f;
        Dimitry = FindObjectOfType<EnemyController>();
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
            UseDoor(ObjectTracker.doorList[0], -450f);*/
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

    public float CalculateRandomForce(float scale = 1f)
    {
        return Mathf.Sin(accumulatedTime) * scale;
    }
}