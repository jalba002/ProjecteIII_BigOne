using System;
using UnityEngine;
using Player;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [Header("Inventory")] public GameObject NotificationPrefab;
    public GameObject NotificationPanel;

    [Header("PauseMenu")] public GameObject pauseMenu;
    public GameObject inventory;

    [Header("Sliders")] public Slider lightingSlider;
    public Slider runningSlider;

    [Header("Crosshair")] public Sprite inspectCrosshair;
    public Sprite grabCrosshair;
    public Sprite draggingCrosshair;

    [Header("Components")] public CrosshairController CrosshairController;
    public Image blackFade;

    private FlashlightController flashlight;
    private PlayerController playerController;
    private State_Player_Walking playerWalking;
    public PauseManager pauseManager;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        flashlight = playerController.attachedFlashlight;
        playerWalking = FindObjectOfType<State_Player_Walking>();

        if (CrosshairController == null)
            CrosshairController = FindObjectOfType<CrosshairController>();

        //_pauseManager = GetComponentInChildren<PauseManager>();
    }

    void Update()
    {
/*#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddPickupMessage("key");
        }
#endif*/

        PlayerLightingUpdate();
        PlayerRunningUpdate();

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

        SetupCursor();
    }

    public void SetupCursor()
    {
        if (GameManager.Instance.PlayerController.interactablesManager.CurrentInteractable != null)
        {
            switch (GameManager.Instance.PlayerController.interactablesManager.CurrentInteractable.interactionType)
            {
                case InteractableObject.InteractionType.Drag:
                case InteractableObject.InteractionType.Pick:
                    ChangeCursor(grabCrosshair, new Vector3(.6f, .6f, 1f));
                    break;
                case InteractableObject.InteractionType.Inspect:
                    ChangeCursor(inspectCrosshair, new Vector3(.6f, .6f, 1f));
                    break;
                case InteractableObject.InteractionType.Interact:
                    break;
                default:
                    ChangeCursor(CrosshairController.defaultCrosshair, new Vector3(0.1f, .1f, 1f));
                    throw new ArgumentOutOfRangeException();
            }
        }
        else if (GameManager.Instance.PlayerController.interactablesManager.CurrentInteractable.IsInteracting)
        {
            ChangeCursor(draggingCrosshair, new Vector3(.6f, .6f, 1f));
        }
        else 
        {
            ChangeCursor(CrosshairController.defaultCrosshair, new Vector3(.1f, .1f, 1f));
        }
    }

    public void AddPickupMessage(string itemName)
    {
        CustomPickupMessage(itemName + " COLLECTED");
    }

    public void CustomPickupMessage(string text)
    {
        GameObject Message = Instantiate(NotificationPrefab, NotificationPanel.transform);
        Message.GetComponent<Notification>().SetMessage(text);
    }

    public void PlayerLightingUpdate()
    {
        lightingSlider.value = flashlight.currentCharge / flashlight.maxCharge;
    }

    public void PlayerRunningUpdate()
    {
        runningSlider.value = playerWalking.currentStamina / playerController.characterProperties.maximumStamina;
    }

    public void ChangeCursor(Sprite newCursor, Vector3 newScale)
    {
        CrosshairController.SetNewCrosshair(newCursor);
        CrosshairController.UIImage.rectTransform.localScale = newScale;
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

            pauseManager.DesactivateOptions();
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