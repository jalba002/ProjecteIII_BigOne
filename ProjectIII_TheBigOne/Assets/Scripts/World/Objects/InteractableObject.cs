using System;
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

    [Header("Debug")] public bool showLogDebug = false;

    [Header("Interactable Settings")] public InteractionType interactionType;

    public string displayName;

    public Sprite previewImage;
    public Sprite displayImage;

    public bool IsInteracting { get; protected set; }

    public virtual void UpdateInteractable()
    {
        if (showLogDebug)
            Debug.Log("Updating " + this.gameObject.name, this);
        GameManager.Instance.CanvasController.ChangeCursor(previewImage,
            new Vector3(.6f, .6f, 1f));
    }

    public virtual bool Interact(bool enableInteraction)
    {
        return false;
    }

    public virtual void OnStartInteract()
    {
        if (showLogDebug)
            Debug.Log("Started interacting with " + this.gameObject.name, this);
        IsInteracting = true;
    }

    public virtual void OnInteracting()
    {
        if (showLogDebug)
            Debug.Log("Interacting with " + this.gameObject.name, this);
        try
        {
            GameManager.Instance.CanvasController.ChangeCursor(displayImage, new Vector3(1f, 1f, 1f));
        }
        catch (NullReferenceException)
        {
        }
    }

    public virtual void OnEndInteract()
    {
        if (showLogDebug)
            Debug.Log("Ending interaction with " + this.gameObject.name, this);
        try
        {
            GameManager.Instance.CanvasController.ChangeCursor(
                GameManager.Instance.CanvasController.CrosshairController.defaultCrosshair,
                new Vector3(0.1f, 0.1f, 1f));
        }
        catch (NullReferenceException)
        {
        }

        IsInteracting = false;
    }
}