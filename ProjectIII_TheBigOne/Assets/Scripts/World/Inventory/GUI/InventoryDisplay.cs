﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryDisplay : MonoBehaviour
{
    [HideInInspector]public Inventory inventoryRef;

    [SerializeField]

    public List<InventorySlot> inventorySlotList = new List<InventorySlot>();
    public GameObject slotPrefab;
    public Transform slotGrid;
    public int numberOfSlots = 12;

    public GameObject inventoryWindow;
    public GameObject combineWindow;

    public InventorySlot combineSlot_1;
    public InventorySlot combineSlot_2;
    

    public InventoryItem selectedItem;
    public InventorySlot selectedSlot;
    public Text selectedItemName;
    public Text selectedItemInfo;

    public GameObject contextMenu;

    void Awake()
    {
        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotGrid);            
            inventorySlotList.Add(instance.GetComponentInChildren<InventorySlot>());
        }
        inventoryRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryRef == null)
        {
            inventoryRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        }

        if (selectedSlot != null)
        {
            contextMenu.SetActive(true);
            contextMenu.transform.SetParent(selectedSlot.transform);
            contextMenu.GetComponent<RectTransform>().anchoredPosition = new Vector3(0 - selectedSlot.gameObject.GetComponent<RectTransform>().rect.size.x,0,0);
            
        }
        else
        {
            contextMenu.SetActive(false);
            
        }

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
        if (selectedItem != null && inventoryRef != null)
        {
            RemoveItem(selectedItem);
            inventoryRef.RemoveItem(selectedItem.itemName);

            selectedItem = null;
            selectedItemInfo.text = " ";
            selectedItemName.text = " ";
            selectedSlot.background.color = Color.white;
            selectedSlot = null;
            
        }
    }

    public void UseButton()
    {
        if (selectedItem != null && inventoryRef != null)
        {
            selectedItem.UseItem(inventoryRef.effectsManager);
            if (selectedItem.destroyOnUse)
            {
                RemoveItem(selectedItem);
                inventoryRef.RemoveItem(selectedItem.itemName);
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


    

    

    
}
