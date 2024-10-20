﻿using UnityEngine;
using World.Objects;

public class ItemRequirements : MonoBehaviour
{
    public string requiredItemID;
    private static Inventory playerInventory;
    public DynamicObject lockedDoor;

    private void Awake()
    {
        if (playerInventory == null)
            playerInventory = GameObject.FindObjectOfType<Inventory>();

        if (lockedDoor == null)
        {
            lockedDoor = gameObject.GetComponentInChildren<DynamicObject>();
        }

        lockedDoor.OnStartInteracting.AddListener(Unlock);
    }

    public void Unlock()
    {
        if (lockedDoor.lockedMode == DynamicObject.LockedMode.Unlocked) return;

        var item = playerInventory.characterItems.Find(x => x.itemID == requiredItemID);
        bool hasItem = item != null;

        if (hasItem)
        {
            playerInventory.inventoryDisplay.RemoveItem(item);
            playerInventory.characterItems.Remove(item);
            lockedDoor.Unlock();
        }
    }
}