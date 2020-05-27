using System;
using UnityEngine;
using System.Collections.Generic;
using Player;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [Header("Item Collected")] public GameObject NotificationPrefab;
    public GameObject NotificationPanel;

    [Header("Quest List")] public GameObject ObjectivesUI;
    public GameObject ObjectivePrefab;

    [Header("Quest Track")] public GameObject PushObjectivesUI;
    public GameObject PushObjectivePrefab;

    [Header("Hint")] public GameObject HintNotification;

    private List<ObjectiveModel> objectives = new List<ObjectiveModel>();

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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ShowHint("metas are your friends", true); // true = upper
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

    public void PushObjectiveText(string text, float time, bool upper, int id)
    {
        ObjectiveModel objModel = new ObjectiveModel(id, text);

        GameObject objObject = Instantiate(ObjectivePrefab, ObjectivesUI.transform);

        objObject.transform.GetChild(0).GetComponent<Text>().text = text;

        objModel.objective = objObject;

        objectives.Add(objModel);

        GameObject obj = Instantiate(PushObjectivePrefab, PushObjectivesUI.transform);
        obj.GetComponent<Notification>().SetMessage(text, time, upper: upper);
    }

    public void ObjectiveCompleted(int id)
    {
        foreach (var obj in objectives)
        {
            if (obj.identifier == id)
            {
                obj.isCompleted = true;
                PushObjectiveCompleted(id, obj);
                Destroy(obj.objective);
            }
        }
    }

    public void PushObjectiveCompleted(int questID, ObjectiveModel quest)
    {
        GameObject obj = Instantiate(PushObjectivePrefab, PushObjectivesUI.transform);
        obj.GetComponent<Notification>().SetMessage(quest.objectiveText + " completed", 3, upper: false);
    }

    public void ShowHint(string text, bool upper)
    {
        HintNotification.SetActive(true);

        if (upper)
            HintNotification.transform.GetChild(0).GetComponent<Text>().text = text.ToUpper();
        else
            HintNotification.transform.GetChild(0).GetComponent<Text>().text = text;

        UIFade uIFade = UIFade.CreateInstance(HintNotification, "[UIFader] HintNotification");
        uIFade.ResetGraphicsColor();
        uIFade.ImageTextAlpha(0.8f, 1f);
        uIFade.FadeInOut(fadeOutTime: 3, fadeOutAfter: UIFade.FadeOutAfter.Time);
    }

    public bool CheckObjectiveIDList(int questID)
    {
        foreach (var obj in objectives)
        {
            if (obj.identifier == questID)
                return true;
        }
        return false;
    }

    public bool CheckEmtpyObjectiveList()
    {
        return objectives.Count == 0;
    }
}

public class ObjectiveModel
{
    public string objectiveText;
    public int identifier;

    public GameObject objective;
    public bool isCompleted;

    public ObjectiveModel(int id, string text)
    {
        identifier = id;
        objectiveText = text;
    }

    public ObjectiveModel(int id, bool completed)
    {
        identifier = id;
        isCompleted = completed;
        objectiveText = "";
    }

    public ObjectiveModel() { }
}