using System;
using Tavaris.Interactable;
using UnityEngine;

namespace Tavaris.Manager
{
    public class DynamicActivator : MonoBehaviour
    {
        public bool Interact(InteractableObject newInteractable, bool enable)
        {
            try
            {
                if (newInteractable.interactSettings.interactionType == InteractSettings.InteractionType.Drag ||
                    newInteractable.interactSettings.interactionType == InteractSettings.InteractionType.Pick)
                {
                    return newInteractable.Interact(enable);
                }
            }
            catch (NullReferenceException)
            {
                return false;
            }

            return false;
        }
    }
}