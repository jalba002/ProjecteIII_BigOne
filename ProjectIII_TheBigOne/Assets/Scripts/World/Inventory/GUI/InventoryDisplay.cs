using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryDisplay : MonoBehaviour
{
    [Header("Gameobjects")]
    public Inventory inventoryRef;

    public GameObject inventoryParent;
    [Header("Settings")]

    [SerializeField] public List<InventorySlot> inventorySlotList = new List<InventorySlot>();
    public GameObject slotPrefab;
    public Transform slotGrid;
    public int numberOfSlots = 12;

    public GameObject inventoryWindow;
    public GameObject combineWindow;

    [Header("Combine Slots")]
    public InventorySlot combineSlot_1;
    public InventorySlot combineSlot_2;


    public InventoryItem selectedItem;
    private static InventorySlot _selectedSlot;

    public static InventorySlot selectedSlot
    {
        get { return _selectedSlot; }
        set
        {
            _selectedSlot = value;
            OnSlotSelected.Invoke();
        }
    }

    public Text selectedItemName;
    public Text selectedItemInfo;

    public static UnityEvent OnSlotSelected = new UnityEvent();

    public GameObject contextMenu;

    void Awake()
    {
        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotGrid);
            inventorySlotList.Add(instance.GetComponentInChildren<InventorySlot>());
        }

        inventoryRef = GameObject.FindObjectOfType<Inventory>();
        OnSlotSelected.AddListener(ConfigureContextMenu);
    }
    
    private void ConfigureContextMenu()
    {
        if (selectedSlot == null)
        {
            contextMenu.SetActive(false);
            return;
        }

        contextMenu.SetActive(true);
        contextMenu.transform.SetParent(selectedSlot.transform);
        contextMenu.GetComponent<RectTransform>().anchoredPosition =
            new Vector3(0 - selectedSlot.gameObject.GetComponent<RectTransform>().rect.size.x, 0, 0);
    }

    public void SetupSlot(int slot, InventoryItem item)
    {
        inventorySlotList[slot].Setup(item);
    }

    public void CopySlot(InventorySlot origin, InventorySlot copy)
    {
        InventoryItem i = copy.item;

        copy.Setup(origin.item);
        copy.SelectThisSlot();
        origin.Setup(i);
    }

    public void AddNewItem(InventoryItem item)
    {
        SetupSlot(inventorySlotList.FindIndex(i => i.item == null), item);
    }

    public void RemoveItem(InventoryItem item)
    {
        if (!item.isStackable)
        {
            SetupSlot(inventorySlotList.FindIndex(i => i.item == item), null);
        }
        else
        {
            int a = item.GetActualQuantity();
            if (a == 1)
            {
                SetupSlot(inventorySlotList.FindIndex(i => i.item == item), null);
            }
            else
            {
                // quantity is changed on the Inventory.cs, it's useless do it here.
            }
        }
    }

    public void RemoveButton()
    {
        Debug.Log("Removing button.");
        if (selectedSlot != null)
        {
            Debug.Log("Removing item is not null.");
            RemoveItem(selectedSlot.item);
            inventoryRef.RemoveItem(selectedSlot.item.itemName);

            selectedItem = null;
            selectedItemInfo.text = " ";
            selectedItemName.text = " ";
            selectedSlot.background.color = Color.white;
            //selectedSlot = null;
        }
    }

    public void UseButton()
    {
        Debug.Log("Using button.");
        if (selectedSlot != null)
        {
            Debug.Log("Use item is not null.");
            selectedSlot.item.UseItem();
            if (selectedSlot.item.destroyOnUse)
            {
                Debug.Log("Destroying item.");
                RemoveItem(selectedSlot.item);
                Debug.Log("Second Destroying.");
                inventoryRef.RemoveItem(selectedSlot.item.itemName);
                Debug.Log("Third Destroying.");
            }
        }
    }

    public void SwitchWindow(string window)
    {
        switch (window)
        {
            case "Inventory":
                inventoryWindow.SetActive(true);
                combineWindow.SetActive(false);
                break;
            case "Combine":
                inventoryWindow.SetActive(false);
                combineWindow.SetActive(true);
                break;
            default:
                break;
        }
    }

    public bool ToggleInventoryUI()
    {
        contextMenu.SetActive(false);
        inventoryParent.SetActive(!inventoryParent.activeSelf);
        return inventoryParent.activeSelf;
    }
}