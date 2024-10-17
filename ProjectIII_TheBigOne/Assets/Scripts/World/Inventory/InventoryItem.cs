using UnityEngine;

[System.Serializable]
public abstract class ItemData
{
    public string itemID = "Default_ID";
    public string itemName = "New Item";
    public string itemDescription = "new description";
    public Sprite itemIcon = null;
    public bool isQuestItem = false;

    public ItemData(string itemID, string name, string description, Sprite icon, bool isQuestItem = false)
    {
        this.itemID = itemID;
        this.itemName = name;
        this.itemDescription = description;
        this.itemIcon = icon;
        this.isQuestItem = isQuestItem;
    }

    //USE
    public abstract void UseItem();
}
