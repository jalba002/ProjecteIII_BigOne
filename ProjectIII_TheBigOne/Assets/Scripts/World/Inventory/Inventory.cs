using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerInventory
{
    public List<ItemData> characterItems = new List<ItemData>();                                                                

    public InventoryItemList database;

    public Action<List<ItemData>> inventoryChanged;

    public bool GiveItem(string itemID)
    {
        if (!database.Contains(itemID)) return false;
        ItemData itemToAdd = database.GetItem(itemID);
        AddItemToList(itemToAdd);
        return true;
    }

    private void AddItemToList(ItemData item)
    {
        characterItems.Add(item);
        inventoryChanged?.Invoke(characterItems);
    }

    private void RemoveItemFromList(ItemData item)
    {
        var isRemoved = characterItems.Remove(item);
        if(isRemoved)
            inventoryChanged?.Invoke(characterItems);
    }

    public bool HasItem(string itemID)
    {
        return characterItems.Find(x => x.itemID == itemID) != null;
    }

    public ItemData CheckThisItem(string itemName)
    {
        return characterItems.Find(ItemData => ItemData.itemName == itemName);
    }

    public void RemoveItem(ItemData item)
    {
        if (item.isQuestItem) return;
        RemoveItemFromList(item);
    }

    public void RemoveItem(string itemName)
    {
        ItemData item = CheckThisItem(itemName);
        if (item.isQuestItem) return;
        RemoveItemFromList(item);
    }
}