using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{

    [SerializeField]
    public Image background;
    private Image spriteImage;            
    public InventoryItem item;            
    private Text itemNameText;            
    private InventoryDisplay inventoryDisplayRef;

    void Awake()                                                                            
    {
        spriteImage = GetComponent<Image>();                                                
        Setup(null);                                                                        
        itemNameText = GetComponentInChildren<Text>();
        inventoryDisplayRef = GameObject.FindGameObjectWithTag("InventoryDisplay").GetComponent<InventoryDisplay>();
    }

    public void Setup(InventoryItem item)               
    {
        this.item = item;                              

        if (this.item != null)                          
        {
            spriteImage.color = Color.white;            
            spriteImage.sprite = this.item.itemIcon;    

            if (itemNameText != null)                    
            {
                itemNameText.text = item.itemName;      
            }
        }
        else                                                
        {
            spriteImage.color = Color.clear;            
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
}
