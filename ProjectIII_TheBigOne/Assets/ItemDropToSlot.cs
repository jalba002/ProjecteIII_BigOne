using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropToSlot : MonoBehaviour, IDropHandler
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        Vector3 actualPos = gameObject.GetComponent<RectTransform>().anchoredPosition;

        gameObject.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<InventorySlot>().slot.GetComponent<RectTransform>().anchoredPosition;
        eventData.pointerDrag.GetComponent<InventorySlot>().slot.GetComponent<RectTransform>().anchoredPosition = actualPos;       
    }
}
