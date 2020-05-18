using System;
using UnityEngine;

public class PuzzleInspector : MonoBehaviour
{
    private IInteractable _currentInteractable;

    public bool Interact(IInteractable newInteractable)
    {
        try
        {
            if (newInteractable.attachedGameobject.GetComponent<IPuzzle>() == null) return false;
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