using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler    
{

    [SerializeField]
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public GameObject slot;
    public Image background;
    public Image spriteImage;            
    public InventoryItem item;            
    public Text itemNameText;
    public GameObject quantity;
    public Text quantityText;
    private InventoryDisplay inventoryDisplayRef;

    [HideInInspector] public GameObject selectedSlot;
    [HideInInspector] public InventoryItem selectedItem;
    [HideInInspector] public bool changeSlot = false;

    public bool isThisSlotToCombine = false;

    void Awake()                                                                            
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        rectTransform = gameObject.GetComponent<RectTransform>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();

        if (isThisSlotToCombine == false)
        {
            slot.GetComponent<RectTransform>().localScale *= canvas.scaleFactor;
        }

        spriteImage = GetComponent<Image>();                                                
        Setup(null);                                                                        
        inventoryDisplayRef = GameObject.FindGameObjectWithTag("InventoryDisplay").GetComponent<InventoryDisplay>();
    }
    

    public void Setup(InventoryItem item)               
    {
        this.item = item;                              

        if (this.item != null)                          
        {
            spriteImage.color = Color.white;            
            spriteImage.sprite = this.item.itemIcon;
            if (this.item.isStackable)
            {
                quantity.SetActive(true);
            }

            if (itemNameText != null)                    
            {
                itemNameText.text = item.itemName;      
            }
        }
        else                                                
        {
            spriteImage.color = Color.clear;
            quantity.SetActive(false);
            if (itemNameText != null)
            {
                itemNameText.text = null;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (item != null)
        {
            if (item.isStackable)
            {
                quantityText.text = item.GetActualQuantity().ToString();
            }
        }

        if (changeSlot)
        {
            
        }
    }

    
    public void SelectThisSlot()
    {
        if (item != null)
        {
            if (inventoryDisplayRef.selectedSlot != null)
            {
                inventoryDisplayRef.selectedSlot.background.color = Color.white;
            }
            background.color = Color.red;
            inventoryDisplayRef.selectedSlot = this;
            inventoryDisplayRef.selectedItem = item;
            inventoryDisplayRef.selectedItemName.text = item.itemName;
            inventoryDisplayRef.selectedItemName.gameObject.SetActive(true);
            inventoryDisplayRef.selectedItemInfo.text = item.itemDescription;
            inventoryDisplayRef.selectedItemInfo.gameObject.SetActive(true);
        }

    } 
    
    public void ResetSlot()
    {
        Setup(null);
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        SelectThisSlot();
        canvasGroup.blocksRaycasts = false;

        this.gameObject.transform.parent = null;
        this.gameObject.transform.SetParent(canvas.transform);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        this.gameObject.transform.SetParent(slot.transform);

        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;   
    }

    public void AddItemToCombine()
    {
        Setup(inventoryDisplayRef.selectedItem);
    }
    
}
