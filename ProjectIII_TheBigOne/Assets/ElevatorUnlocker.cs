using UnityEngine;

public class ElevatorUnlocker : MonoBehaviour
{
    public EndgamePuzzle hammerPuzzle;
    public EndgamePuzzle sicklePuzzle;

    public Animator elevatorAnimation;

    public void CheckForElevatorOpening()
    {
        if (hammerPuzzle.alreadyCompleted && sicklePuzzle.alreadyCompleted)
        {
            UnlockElevator();
        }
    }

    private void UnlockElevator()
    {
        elevatorAnimation.SetTrigger("Activate");
    }
}