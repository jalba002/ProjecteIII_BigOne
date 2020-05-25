using System;
using UnityEngine;

public class DynamicActivator : MonoBehaviour
{
    public bool Interact(InteractableObject newInteractable, bool enable)
    {
        try
        {
            if (newInteractable.interactionType == InteractableObject.InteractionType.Drag ||
                newInteractable.interactionType == InteractableObject.InteractionType.Pick)
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