using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryDisplay : MonoBehaviour
{

    [SerializeField]

    public List<InventorySlot> inventorySlotList = new List<InventorySlot>();
    public GameObject slotPrefab;
    public Transform slotGrid;
    public int numberOfSlots = 12;
    public InventoryItem selectedItem;
    public InventorySlot selectedSlot;
    public Text selectedItemName;
    public Text selectedItemInfo;

    void Awake()
    {
        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotGrid);            
            inventorySlotList.Add(instance.GetComponentInChildren<InventorySlot>());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void SetupSlot(int slot, InventoryItem item)
    {
        inventorySlotList[slot].Setup(item);
    }

    public void AddNewItem(InventoryItem item)
    {
        SetupSlot(inventorySlotList.FindIndex(i => i.item == null), item);
    }

    public void RemoveItem(InventoryItem item)
    {
        SetupSlot(inventorySlotList.FindIndex(i => i.item == item), null);
    }
    
}
