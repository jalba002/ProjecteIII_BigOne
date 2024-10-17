using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tavaris.UI
{
    public class InventoryDisplay : MonoBehaviour
    {
        public InventoryDisplay Instance;

        [Header("Gameobjects")] public PlayerInventory inventoryRef;

        public GameObject inventoryParent;
        [Header("Settings")] public List<InventorySlot> inventorySlotList = new List<InventorySlot>();
        public GameObject slotPrefab;
        public Transform slotGrid;
        public int numberOfSlots = 12;

        public GameObject inventoryWindow;

        private InventorySlot _selectedSlot;

        public CanvasController canvas { get; protected set; }

        public InventorySlot selectedSlot
        {
            get { return _selectedSlot; }
            set
            {
                _selectedSlot = value;
                OnSlotSelected.Invoke();
            }
        }

        //public Text selectedItemName;
        public TextMeshProUGUI selectedItemName;
        public Text selectedItemInfo;

        public static UnityEvent OnSlotSelected = new UnityEvent();

        public GameObject contextMenu;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            for (int i = 0; i < numberOfSlots; i++)
            {
                GameObject instance = Instantiate(slotPrefab);
                instance.transform.SetParent(slotGrid);
                inventorySlotList.Add(instance.GetComponentInChildren<InventorySlot>());
            }

            OnSlotSelected.AddListener(ConfigureContextMenu);

            canvas = FindObjectOfType<CanvasController>();
        }

        private void Start()
        {
            ToggleInventoryUI();
        }

        private void ConfigureContextMenu()
        {
            if (selectedSlot == null)
            {
                contextMenu.SetActive(false);
                return;
            }
            if (!selectedSlot.itemRef.isQuestItem)
            {
                contextMenu.transform.SetParent(selectedSlot.transform);
                contextMenu.SetActive(true);
                contextMenu.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(0 - selectedSlot.gameObject.GetComponent<RectTransform>().rect.size.x, 0, 0);
            }
            else
            {
                contextMenu.SetActive(false);
            }


        }

        public void RemoveButton()
        {
            if (selectedSlot != null)
            {
                Debug.Log("Removing item is not null.");
                if (!selectedSlot.itemRef.isQuestItem)
                {
                    SoundManager.Instance.PlaySound2D("event:/SFX/UI/Inventory/DiscardItem");
                    inventoryRef.RemoveItem(selectedSlot.itemRef);
                    RemoveText();
                    selectedSlot = null;
                }
                else
                {
                    Debug.Log("You can't discard this item");
                }
            }
        }

        public void RemoveText()
        {
            selectedItemInfo.text = " ";
            selectedItemName.text = " ";
        }

        public void UseButton()
        {
            //Debug.Log("Using button.");
            //if (selectedSlot != null)
            //{
            //    Debug.Log("Use item is not null.");
            //    selectedSlot.itemRef.UseItem();
            //    if (selectedSlot.item.destroyOnUse)
            //    {
            //        RemoveItem(selectedSlot.item);
            //        inventoryRef.RemoveItem(selectedSlot.item.itemName);
            //        selectedSlot = null;
            //    }

            //}
        }

        public bool ToggleInventoryUI()
        {
            if (selectedSlot != null)
                selectedSlot.UnselectThisSlot();
            //selectedSlot = null;        
            contextMenu.SetActive(false);
            if (!inventoryParent.activeSelf)
            {
                GameManager.Player.interactablesManager.ClearInteractable();
            }

            GameManager.Player.interactablesManager.enabled = inventoryParent.activeSelf;

            inventoryParent.SetActive(!inventoryParent.activeSelf);
            return inventoryParent.activeSelf;
        }
    }
}