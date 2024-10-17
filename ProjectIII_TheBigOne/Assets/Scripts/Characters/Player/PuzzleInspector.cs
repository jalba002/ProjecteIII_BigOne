using System;
using Tavaris.Interactable;
using UnityEngine;

namespace Tavaris.Manager
{
    public class PuzzleInspector : MonoBehaviour
    {
        private InteractableObject _currentInteractable;

        public bool Interact(InteractableObject newInteractable)
        {
            if (newInteractable.interactSettings.interactionType != InteractSettings.InteractionType.Puzzle) return false;

            _currentInteractable = newInteractable;

            if (_currentInteractable != null)
            {
                _currentInteractable.Interact(!_currentInteractable.IsInteracting);
                return _currentInteractable.IsInteracting;
            }

            return false;
        }
    }
}