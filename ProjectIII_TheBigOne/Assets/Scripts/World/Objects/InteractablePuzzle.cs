using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class InteractablePuzzle : MonoBehaviour, IInteractable, IPuzzle
{
    public string displayName = "Use puzzle";
    static CameraController playerCameraController;

    // public Puzzle attachedPuzzle;

    void Awake()
    {
        // attachedPuzzle = GetComponent<Puzzle>();
    }
    
    void Start()
    {
        DisplayName = displayName;
        if (playerCameraController == null)
        {
            playerCameraController = FindObjectOfType<CameraController>();
        }
    }

    void Update()
    {
        if (IsInteracting)
        {
            OnInteracting();
        }
    }
    
    public string DisplayName { get; set; }

    public GameObject attachedGameobject
    {
        get { return this.gameObject; }
    }
    public bool IsInteracting { get; set; }
    public bool Interact(bool interactEnable)
    {
        if (interactEnable)
        {
            if (!IsInteracting)
            {
                OnStartInteract();
                return true;
            }
        }
        else
        {
            if (IsInteracting)
            {
                OnEndInteract();
                return true;
            }
        }

        return false;
    }

    public void ForceEndInteraction()
    {
        Debug.LogWarning("Forced end of interaction!", this.gameObject);
        OnEndInteract();
        IsInteracting = false;
    }

    public void OnStartInteract()
    {
        // Change camera position
        // Change player state to puzzle interaction.
        // Which blocks movement and camera input. Plus all necessary.
        // Show cursor to begin interacting.
        // Set IsInteracting to true.
        
        GameManager.Instance.PlayerController.cameraController.SetNewPosition(null ,true);
        //GameManager.Instance.PlayerController.stateMachine.SwitchState<State_Player_PuzzleInspect>();
        IsInteracting = true;
        //playerCameraController.SetNewPosition(attachedPuzzle.gameObject.transform,true);
        // attachedPuzzle.StartGame();

    }

    public void OnInteracting()
    {
        // Lerp camera position?
        // Read mouse inputs for clicks.
        // Raycast stuff to activate puzzle.
        // And much more!?...

        //Debug.Log("Interacting!");

    }

    public void OnEndInteract()
    {
        // Restore camera position.
        // Restore controls to player.
        // Restore cursor to usual settings.
        // Set IsInteracting to false.
        Debug.Log("Ending interaction", this.gameObject);
        playerCameraController.RestorePosition();
        //GameManager.Instance.PlayerController.stateMachine.SwitchState<State_Player_Walking>();
        IsInteracting = false;
        
        // attachedPuzzle.EndGame();
        
    }
}
