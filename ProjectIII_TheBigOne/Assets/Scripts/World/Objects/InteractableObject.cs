using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [System.Serializable]
    public enum InteractionType
    {
        Drag,
        Pick,
        Inspect,
        Interact
    }

    [Header("Debug")]
    public bool showLogDebug = false;
    
    [Header("Interactable Settings")]
    public InteractionType interactionType;
    
    public string displayName;

    public Image displayImage; 

    public bool IsInteracting { get; protected set; }

    public virtual void UpdateInteractable()
    {
        if (showLogDebug) 
            Debug.Log("Updating " + this.gameObject.name, this);
    }

    public virtual bool Interact(bool enableInteraction)
    {
        return false;
    }

    public virtual void OnStartInteract()
    {
        if (showLogDebug)
            Debug.Log("Started interacting with " + this.gameObject.name, this);
    }

    public virtual void OnInteracting()
    {
        if (showLogDebug)
            Debug.Log("Interacting with " + this.gameObject.name, this);
    }

    public virtual void OnEndInteract()
    {
        if (showLogDebug)
            Debug.Log("Ending interaction with " + this.gameObject.name, this);
    }
}