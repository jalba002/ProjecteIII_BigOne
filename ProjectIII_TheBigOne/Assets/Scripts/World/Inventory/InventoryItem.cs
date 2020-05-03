using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(ItemEffectsManager))]
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
    public bool canUseAlways = false;

    public List<string> combinedItems;

    private int actualQuantity = 0;


    public enum Effect
    {
        BATTERY_EFFECT, AXE_EFFECT, OTHER_EFFECT
    }

    public Effect myEffect = Effect.OTHER_EFFECT;

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

    //USE
    public void UseItem()
    {
        Debug.Log("USING ITEM");
        switch (myEffect)
        {
            case Effect.BATTERY_EFFECT:
                Debug.Log("BATTERY RELOAD");
                ItemEffectsManager.ReloadLantern();
                break;
            case Effect.AXE_EFFECT: ItemEffectsManager.PickWithAxe();
                break;
            case Effect.OTHER_EFFECT: ItemEffectsManager.PrintDebug();
                break;
            default:
                break;
        }
    }
}
