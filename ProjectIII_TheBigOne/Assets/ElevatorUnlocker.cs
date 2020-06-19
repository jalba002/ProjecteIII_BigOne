using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorUnlocker : MonoBehaviour
{
    public EndgamePuzzle hammerPuzzle;
    public EndgamePuzzle sicklePuzzle;

    public Animation elevatorAnimation;

    private bool alreadyUnlocked = false;

    private void Start()
    {
        alreadyUnlocked = false;
    }

    public void CheckForElevatorOpening()
    {
        if (hammerPuzzle.alreadyCompleted && sicklePuzzle.alreadyCompleted && !alreadyUnlocked)
        {
            UnlockElevator();
        }
    }

    private void UnlockElevator()
    {
        Debug.Log("Unlocking Elevator");
        elevatorAnimation.Play();
        alreadyUnlocked = true;
    }
}