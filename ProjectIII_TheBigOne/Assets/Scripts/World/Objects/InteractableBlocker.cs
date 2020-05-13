using System.Collections;
using System.Collections.Generic;
using Enemy;
using Player;
using UnityEngine;
using World.Objects;

public class InteractableBlocker : MonoBehaviour
{
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
        attachedInteractable.OnEndInteract();
    }

    public void Unblock()
    {
        attachedInteractable.gameObject.layer = 9;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Block();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Unblock();
        }
    }
}