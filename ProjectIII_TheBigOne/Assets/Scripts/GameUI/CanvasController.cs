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

    [Header("PauseMenu")] public GameObject pauseMenu;
    public GameObject inventory;

    [Header("Sliders")] public Slider lightingSlider;
    public Slider runningSlider;

    [Header("Crosshair")] public Sprite defaultCrosshair;
    public Sprite grabCrosshair;

    [Header("Components")] public Image UICrosshair;
    public Image blackFade;

    private FlashlightController flashlight;
    private PlayerController playerController;
    private State_Player_Walking playerWalking;
    private PauseManager _pauseManager;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        flashlight = playerController.attachedFlashlight;
        playerWalking = FindObjectOfType<State_Player_Walking>();
        _pauseManager = GetComponentInChildren<PauseManager>();
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
        if (!GameManager.Instance.PlayerController.IsDead)
            TooglePauseMenu(false);

        if (playerWalking.currentStamina >= playerController.characterProperties.maximumStamina)
            runningSlider.gameObject.SetActive(false);
        else
            runningSlider.gameObject.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Escape) && playerController.playerInventory.inventoryDisplay.gameObject.transform
                .parent.gameObject.activeSelf)
        {
            playerController.ToggleInventory();
        }
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

    public void TooglePauseMenu(bool forceEnable)
    {
        if (playerController.currentBrain.ShowPause || forceEnable)
        {
            var enabled = pauseMenu.activeInHierarchy;
            playerController.cameraController.angleLocked = !enabled;
            playerController.cameraController.cursorLock = enabled;
            Cursor.visible = !enabled;
            playerController.stateMachine.enabled = enabled;
            playerController.interactablesManager.enabled = enabled;
            playerController.interactablesManager.ClearInteractable();
            playerController.objectInspector.enabled = enabled;

            //Something about enemy?

            pauseMenu.SetActive(!enabled);

            _pauseManager.DesactivateOptions();
        }
    }

    public void ResumeGame()
    {
        var enabled = true;
        playerController.cameraController.angleLocked = !enabled;
        playerController.cameraController.cursorLock = enabled;
        Cursor.visible = !enabled;
        playerController.stateMachine.enabled = enabled;
        playerController.interactablesManager.enabled = enabled;
        playerController.objectInspector.enabled = enabled;

        //Something about enemy?

        pauseMenu.SetActive(!enabled);
    }
}