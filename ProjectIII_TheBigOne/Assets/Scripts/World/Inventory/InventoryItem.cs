using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]                         
public class InventoryItem
{
    public string itemName = "New Item";      
    public string itemDescription = "new description";
    public int MaxStackQuantity = 1;          
    public Sprite itemIcon = null;         
    public GameObject itemObject;
    public bool isUnique = false;             
         
    public bool isStackable = false;          
    public bool destroyOnUse = false;

    public List<string> combinedItems;

    private int actualQuantity = 0;
    
    public InventoryItem(InventoryItem item)
    {
        itemName = item.itemName;
        itemDescription = item.itemDescription;
        MaxStackQuantity = item.MaxStackQuantity;
        itemIcon = item.itemIcon;
        itemObject = item.itemObject;
        isUnique = item.isUnique;
        //isIndestructible = item.isIndestructible;
        //isQuestItem = item.isQuestItem;
        isStackable = item.isStackable;
        destroyOnUse = item.destroyOnUse;
        //encumbranceValue = item.encumbranceValue;

        if (actualQuantity == 0)
        {
            actualQuantity = 1;
        }
    }

    public int GetActualQuantity()
    {
        return actualQuantity;
    }
    public void SetActualQuantity(int sum)
    {
        actualQuantity = sum;
    }
}
