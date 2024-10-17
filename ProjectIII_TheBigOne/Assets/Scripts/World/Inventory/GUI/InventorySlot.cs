using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tavaris.UI
{
    public class InventorySlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public ItemData itemRef { get; private set; }

        [HideInInspector] public static ItemData selectedItem;

        void Awake()
        {
            
        }

        public void Setup()
        {
           
        }

        public void SelectThisSlot()
        {
            SoundManager.Instance.PlaySound2D("event:/SFX/UI/Inventory/SelectItem");
        }

        public void UnselectThisSlot()
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //canvasGroup.blocksRaycasts = false;
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //canvasGroup.blocksRaycasts = true;
            //this.gameObject.transform.SetParent(slot.transform);

            //rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        }

        public void OnDrag(PointerEventData eventData)
        {
            //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            transform.position = new Vector2(eventData.position.x, eventData.position.y);
        }
    }
}