using System;
using UnityEngine;
namespace Tavaris.Interactable
{
    public class InteractableObject : MonoBehaviour
    {
        public InteractSettings interactSettings;

        [Header("Debug")] public bool showLogDebug = false;

        [Header("Events")]
        public Action OnStartInteraction;
        public Action OnEndInteraction;

        public bool IsInteracting { get; protected set; } = false;

        public virtual void UpdateInteractable()
        {
            if (showLogDebug)
                Debug.Log("Updating " + this.gameObject.name, this);
            GameManager.Instance.CanvasController.ChangeCursor(interactSettings.previewImage,
                Vector3.one * 0.6f);
        }

        public virtual bool Interact(bool enableInteraction)
        {
            if (showLogDebug)
                Debug.Log("Interacting with " + this.gameObject.name, this);
            return false;
        }

        public virtual void StartInteract()
        {
            if (showLogDebug)
                Debug.Log("Started interacting with " + this.gameObject.name, this);
            OnStartInteraction?.Invoke();
            IsInteracting = true;
        }

        public virtual void Interacting()
        {
            if (showLogDebug)
                Debug.Log("Interacting with " + this.gameObject.name, this);
            try
            {
                GameManager.Instance.CanvasController.ChangeCursor(interactSettings.displayImage, Vector3.one);
            }
            catch (NullReferenceException)
            {
            }
        }

        public virtual void EndInteract()
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
            finally
            {
                OnEndInteraction?.Invoke();
                IsInteracting = false;
            }
        }
    }
}