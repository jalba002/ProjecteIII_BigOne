using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedSlot : MonoBehaviour
{
    public InventoryDisplay inventoryDisplayRef;

    public InventorySlot firstSlot;
    public InventorySlot secondSlot;
    public InventorySlot combinedSlot;
    public InventoryItemList database;

    
    private void Start()
    {
        database = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().database;    
    }

    private void Update()
    {
        if (database == null)
        {
            database = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().database;
        }

        if (firstSlot.item != null && secondSlot != null)
        {
            combinedSlot.Setup(SearchCombination(firstSlot.item, secondSlot.item));
        }
        
    }

    public InventoryItem SearchCombination(InventoryItem a, InventoryItem b)
    {
        foreach (InventoryItem item in database.itemList)
        {
            foreach (string combineItem in item.combinedItems)
            {
                if (combineItem == a.itemName)
                {
                    foreach (string combineItem2 in item.combinedItems)
                    {
                        if (combineItem2 == b.itemName && combineItem2 != combineItem)
                        {
                            return item;
                        }
                    }
                }
            }
        }
        return null;
    }

    public void GetTheCombinedItem()
    {
        if (combinedSlot.item != null)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().AddItem(combinedSlot.item.itemName);

            inventoryDisplayRef.RemoveItem(firstSlot.item);
            inventoryDisplayRef.inventoryRef.RemoveItem(firstSlot.item.itemName);
            firstSlot.Setup(ResetSlot());

            inventoryDisplayRef.RemoveItem(secondSlot.item);
            inventoryDisplayRef.inventoryRef.RemoveItem(secondSlot.item.itemName);
            secondSlot.Setup(ResetSlot());

            combinedSlot.Setup(ResetSlot());
        }
        
    }

    public InventoryItem ResetSlot()
    {
        return null;
    }
}
