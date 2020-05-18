using System;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;
using World.Objects;

public class InteractableBlocker : MonoBehaviour
{
    public DynamicObject attachedInteractable;

    public List<GameObject> nearbyCharacters = new List<GameObject>();

    private static InteractablesManager playerInteractableManager;

    // Start is called before the first frame update
    void Start()
    {
        if (attachedInteractable == null)
            attachedInteractable = GetComponentInChildren<DynamicObject>();
        if (playerInteractableManager == null)
            playerInteractableManager = FindObjectOfType<InteractablesManager>();
    }

    public void Block()
    {
        attachedInteractable.gameObject.layer = 2;
        attachedInteractable.OnEndInteract();
        try
        {
            if (playerInteractableManager.CurrentInteractable.attachedGameobject == attachedInteractable.gameObject)
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