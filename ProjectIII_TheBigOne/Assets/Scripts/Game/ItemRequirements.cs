using UnityEngine;
using World.Objects;

public class ItemRequirements : MonoBehaviour
{
    public string requiredItem;
    private Inventory playerInventory;
    public DynamicObject lockedDoor;

    private void Awake()
    {
        playerInventory = GameObject.FindObjectOfType<Inventory>();
        lockedDoor = gameObject.GetComponent<DynamicObject>();
        lockedDoor.OnStartInteracting.AddListener(Unlock);
    }
    
    public void Unlock()
    {
        var item = playerInventory.characterItems.Find(x => x.itemName == requiredItem);
        bool hasItem = item != null;
        
        if (hasItem)
        {
            playerInventory.inventoryDisplay.RemoveItem(item);
            playerInventory.characterItems.Remove(item);
            lockedDoor.Unlock();
        }
        else
        {
            //Display iventory
            Debug.Log("IM HERE");
            GameObject.FindObjectOfType<CanvasController>().ToggleInventory();
        }
    }
}
