using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> characterItems = new List<InventoryItem>();  //create a new list called items                                                                             
    public InventoryItemList database;                                      //pick the list we want to get info from
    public InventoryDisplay inventoryDisplay;
    [HideInInspector]public ItemEffectsManager effectsManager;

    // Start is called before the first frame update
    void Start()
    {
        effectsManager = GetComponent<ItemEffectsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (effectsManager == null)
        {
            effectsManager = GetComponent<ItemEffectsManager>();
        }
    }

    public void GiveItem(string itemName)
    {
        InventoryItem itemToAdd = database.GetItem(itemName);
        characterItems.Add(itemToAdd);
        //  Debug.Log("gave item:" + itemToAdd.itemName);
        inventoryDisplay.AddNewItem(itemToAdd);         //add the item to the inventory display

    }
    public void AddItem(string itemName)
    {
        InventoryItem itemToAdd = database.GetItem(itemName);   //get reference to our listed item
        
        if (itemToAdd.isStackable)
        {
            InventoryItem itemSearched = CheckThisItem(itemName);
            if (itemSearched != null)
            {   
                itemSearched.SetActualQuantity(itemSearched.GetActualQuantity() + 1);    
            }
            else
            {
                itemToAdd.SetActualQuantity(1);
                characterItems.Add(itemToAdd);                                   //add reference to our local items list
                inventoryDisplay.AddNewItem(itemToAdd);
            }
        }
        
        else
        {
            characterItems.Add(itemToAdd);                                   
            inventoryDisplay.AddNewItem(itemToAdd);
        }
        
        
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
                Debug.Log(a);
                if (a == 1)
                {
                    characterItems.Remove(item);
                    Debug.Log("Item removed: " + item.itemName);
                }
                else
                {
                    item.SetActualQuantity(a - 1);
                    Debug.Log("Item removed: " + item.itemName);
                }
            }
            
        }
    }   
}
