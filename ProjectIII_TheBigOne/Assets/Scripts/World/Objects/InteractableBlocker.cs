using System;
using System.Collections.Generic;
using Tavaris.Dynamic;
using Tavaris.Manager;
using UnityEngine;

public class InteractableBlocker : MonoBehaviour
{
    public Pallet attachedInteractable;

    public List<GameObject> nearbyCharacters = new List<GameObject>();

    private static InteractablesManager playerInteractableManager;

    // Start is called before the first frame update
    void Start()
    {
        if (attachedInteractable == null)
            attachedInteractable = GetComponentInChildren<Pallet>();
        if (playerInteractableManager == null)
            playerInteractableManager = FindObjectOfType<InteractablesManager>();
    }

    public void Block()
    {
        attachedInteractable.gameObject.layer = 2;
        attachedInteractable.EndInteract();
        try
        {
            if (playerInteractableManager.CurrentInteractable.gameObject == attachedInteractable.gameObject)
            {
                playerInteractableManager.ClearInteractable();
            }
        }
        catch (NullReferenceException)
        {
        }
    }

    public void Unblock()
    {
        attachedInteractable.gameObject.layer = 9;
    }

    private void CheckBlock()
    {
        if (nearbyCharacters.Count > 0)
            Block();
        else
        {
            Unblock();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            nearbyCharacters.Add(other.gameObject);
            CheckBlock();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            nearbyCharacters.Remove(other.gameObject);
            CheckBlock();
        }
    }
}