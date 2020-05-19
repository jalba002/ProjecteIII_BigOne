using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem>
        characterItems =
            new List<InventoryItem>(); //create a new list called items                                                                             

    public InventoryItemList database; //pick the list we want to get info from
    public InventoryDisplay inventoryDisplay;

    // Start is called before the first frame update
    private void Awake()
    {
        inventoryDisplay = FindObjectOfType<InventoryDisplay>();
        if (inventoryDisplay == null)
        {
            Debug.LogWarning("No inventory canvas exists.");
        }
    }

    public void GiveItem(string itemID)
    {
        InventoryItem itemToAdd = database.GetItem(itemID);
        characterItems.Add(itemToAdd);
        //  Debug.Log("gave item:" + itemToAdd.itemName);
        inventoryDisplay.AddNewItem(itemToAdd); //add the item to the inventory display
    }

    public bool AddItem(string itemID)
    {
        InventoryItem itemToAdd = database.GetItem(itemID); //get reference to our listed item

        if (itemToAdd.isStackable)
        {
            InventoryItem itemSearched = CheckThisItem(itemID);
            if (itemSearched != null)
            {
                itemSearched.SetActualQuantity(itemSearched.GetActualQuantity() + 1);
            }
            else
            {
                itemToAdd.SetActualQuantity(1);
                characterItems.Add(itemToAdd); //add reference to our local items list
                inventoryDisplay.AddNewItem(itemToAdd);
            }
        }

        else
        {
            characterItems.Add(itemToAdd);
            inventoryDisplay.AddNewItem(itemToAdd);
        }
        inventoryDisplay.canvas.AddPickupMessage(itemToAdd.itemName);
        return true;
    }

    public InventoryItem CheckThisItem(string itemName)
    {
        return characterItems.Find(InventoryItem => InventoryItem.itemName == itemName);
    }


    public void RemoveItem(string itemName)
    {
        InventoryItem item = CheckThisItem(itemName);
        if (item != null)
        {
            if (!item.isStackable)
            {
                characterItems.Remove(item);
                Debug.Log("Item removed: " + item.itemName);
            }
            else
            {
                int a = item.GetActualQuantity();
                if (a == 1)
                {
                    characterItems.Remove(item);
                    inventoryDisplay.ClearFromInventory(item);
                    Debug.Log("Item removed: " + item.itemName);
                }
                else
                {
                    item.SetActualQuantity(a - 1);
                    Debug.Log("Removed 1 unit of " + item.itemName + ", now " + item.GetActualQuantity() + " remaining.");
                }
            }
        }
    }

    public bool ToggleInventory()
    {
        return inventoryDisplay.ToggleInventoryUI();
    }
}