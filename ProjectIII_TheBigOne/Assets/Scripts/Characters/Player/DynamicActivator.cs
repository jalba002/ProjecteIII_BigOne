using System;
using Interfaces;
using UnityEngine;
using World.Objects;

public class DynamicActivator : MonoBehaviour
{
    public bool Interact(IInteractable newInteractable, bool enable)
    {
        try
        {
            if (newInteractable.attachedGameobject.GetComponent<IMovable>() == null 
                && newInteractable.attachedGameobject.GetComponent<IPickable>() == null) return false;
        }
        catch (NullReferenceException)
        {
            return false;
        }
        return newInteractable.Interact(enable);
    }
}
