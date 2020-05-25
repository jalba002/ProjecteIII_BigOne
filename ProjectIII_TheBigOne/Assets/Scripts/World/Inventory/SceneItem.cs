using System;
using System.Configuration;
using System.Security.Cryptography;
using Player;
using UnityEditor;
using UnityEngine;

public class SceneItem : MonoBehaviour, IInteractable, IPickable
{
    public string itemID;
    public string displayName;
    private bool alreadyUsed = false;

    public string DisplayName { get; set; }
    public GameObject attachedGameobject
    {
        get { return gameObject; }
    }

    public void UpdateInteractable()
    {
        // Nothing.
    }

    public bool IsInteracting { get; set; }

    private void Start()
    {
        DisplayName = $"Pick up {displayName}";
        IsInteracting = false;
    }
    
    public bool Interact(bool interactEnable)
    {
        if (alreadyUsed || IsInteracting || !interactEnable) return false;
        if (FindObjectOfType<PlayerController>().playerInventory.AddItem(itemID))
        {
            this.gameObject.SetActive(false);
            alreadyUsed = true;
        }
        return true;
    }

    public void OnStartInteract()
    {
        IsInteracting = true;
    }

    public void OnInteracting()
    {
        //throw new System.NotImplementedException();
    }

    public void OnEndInteract()
    {
        IsInteracting = false;
    }
}