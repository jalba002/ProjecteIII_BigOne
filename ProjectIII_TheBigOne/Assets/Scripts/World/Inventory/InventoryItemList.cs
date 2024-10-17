using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "New Item List", order = 1)]
public class InventoryItemList : ScriptableObject
{
    public List<ItemData> itemList;

    public bool Contains(string id)
    {
        return GetItem(id) != null;
    }

    public ItemData GetItem(string id)
    {
        return itemList.Find(x => x.itemID == id);
    }
}
