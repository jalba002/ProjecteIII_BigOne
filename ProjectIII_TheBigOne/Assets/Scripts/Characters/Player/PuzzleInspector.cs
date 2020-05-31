using System;
using UnityEngine;

public class PuzzleInspector : MonoBehaviour
{
    private InteractableObject _currentInteractable;

    public bool Interact(InteractableObject newInteractable)
    {
        try
        {
            if (newInteractable.interactionType != InteractableObject.InteractionType.Puzzle) return false;
        }
        catch (NullReferenceException)
        {
            return false;
        }

        _currentInteractable = newInteractable;

        if (_currentInteractable != null)
        {
            _currentInteractable.Interact(!_currentInteractable.IsInteracting);
            return _currentInteractable.IsInteracting;
        }

        return false;
    }
}