using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using CharacterController = Characters.Generic.CharacterController;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [Header("Inventory")] public GameObject NotificationPrefab;
    public GameObject NotificationPanel;

    [Header("Sliders")] public Slider lightingSlider;
    public Slider runningSlider;

    [Header("Crosshair")] public Sprite defaultCrosshair;
    public Sprite grabCrosshair;

    [Header("Components")] public Image UICrosshair;

    private FlashlightController flashlight;
    private PlayerController playerController;
    private State_Player_Walking playerWalking;

    public GameObject inventoryObject;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        flashlight = playerController.attachedFlashlight;
        playerWalking = FindObjectOfType<State_Player_Walking>();

        if(inventoryObject == null)
        {
            inventoryObject = GameObject.Find("Inventory");
        }

    }

    void Update()
    {
        
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddPickupMessage("key");
        }
#endif

        playerLightingUpdate();
        playerRunningUpdate();

        if (playerWalking.currentStamina >= playerController.characterProperties.maximumStamina)
            runningSlider.gameObject.SetActive(false);
        else
            runningSlider.gameObject.SetActive(true);
    }

    public void AddPickupMessage(string itemName)
    {
        GameObject Message = Instantiate(NotificationPrefab, NotificationPanel.transform);
        //Notifications.Add(Message);
        //Message.GetComponent<Notification>().SetMessage(string.Format("Picked up {0}", itemName));
        Message.GetComponent<Notification>().SetMessage(itemName + " COLLECTED");
    }

    public void playerLightingUpdate()
    {
        lightingSlider.value = flashlight.currentCharge / flashlight.maxCharge;
    }

    public void playerRunningUpdate()
    {
        runningSlider.value = playerWalking.currentStamina / playerController.characterProperties.maximumStamina;
    }

    public void ChangeCursorToDefault()
    {
        UICrosshair.sprite = defaultCrosshair;
        UICrosshair.rectTransform.localScale = new Vector3(.1f, .1f, 1f);
    }

    public void ChangeCursorToGrab()
    {
        UICrosshair.sprite = grabCrosshair;
        UICrosshair.rectTransform.localScale = new Vector3(.6f, .6f, 1f);
    }

    public void ToggleInventory()
    {
        Debug.Log(inventoryObject.activeInHierarchy);
        if (!inventoryObject.activeInHierarchy)
        {
            playerController.ToggleInventory();
        }
    }

}