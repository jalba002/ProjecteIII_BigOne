using System.Collections;
using System.Collections.Generic;
using Enemy;
using Player;
using UnityEngine;
using World.Objects;

public class InteractableBlocker : MonoBehaviour
{
    public LayerMask defaultLayer;
    public LayerMask interactableLayer;

    public DynamicObject attachedInteractable;

    // Start is called before the first frame update
    void Start()
    {
        if (attachedInteractable == null)
            attachedInteractable = GetComponentInChildren<DynamicObject>();
    }

    public void Block()
    {
        attachedInteractable.gameObject.layer = 2;
        //attachedInteractable.Interact();
    }

    public void Unblock()
    {
        attachedInteractable.gameObject.layer = 9;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entering Trigger");
        if (other.GetComponent<PlayerController>() != null)
        {
            Block();
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exit");
        if (other.GetComponent<PlayerController>() != null)
        {
            Unblock();
        }
    }
}