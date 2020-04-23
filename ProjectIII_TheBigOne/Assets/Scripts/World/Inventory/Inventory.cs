using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> characterItems = new List<InventoryItem>();  //create a new list called items                                                                             
    public InventoryItemList database;                                      //pick the list we want to get info from
    public InventoryDisplay inventoryDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        characterItems.Add(itemToAdd);                                   //add reference to our local items list
        inventoryDisplay.AddNewItem(itemToAdd);
        //     InventoryEvents.OnItemAddedToInventory(itemToAdd);      //call event using our referenced item, the event will tell the display to show it.
        //   Debug.Log("Item addded: " + itemToAdd.itemName);
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
            characterItems.Remove(item);
            Debug.Log("Item removed: " + item.itemName);
        }
    }

}
