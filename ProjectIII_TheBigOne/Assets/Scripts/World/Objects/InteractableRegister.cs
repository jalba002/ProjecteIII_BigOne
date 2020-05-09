using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using Player;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class InteractableRegister : MonoBehaviour
{
    private IInteractable _interactable;
    private IInteractable AttachedInteractable
    {
        get => _interactable ?? (_interactable = GetComponentInParent<IInteractable>());
        set => _interactable = value;
    }
    
    private InteractablesManager _interactablesManager;
    
    private void SetInteractableManager(InteractablesManager interactablesManager)
    {
        _interactablesManager = interactablesManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        SetInteractableManager(other.gameObject.GetComponent<InteractablesManager>());
        if (_interactablesManager != null)
        {
            _interactablesManager.registeredInteractables.Add(AttachedInteractable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveFromList();
    }

    private void RemoveFromList()
    {
        if (_interactablesManager != null)
        {
            _interactablesManager.registeredInteractables.Remove(AttachedInteractable);
        }
    }

    private void OnDisable()
    {
        RemoveFromList();
    }

    private void OnDestroy()
    {
        RemoveFromList();
    }
}