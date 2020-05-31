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
    private State_Player_Walking playerWalking;
    public PauseManager pauseManager;

    private void Start()
    {
        flashlight = GameManager.Instance.PlayerController.attachedFlashlight;
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

        if (playerWalking.currentStamina >= GameManager.Instance.PlayerController.characterProperties.maximumStamina)
            runningSlider.gameObject.SetActive(false);
        else
            runningSlider.gameObject.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.PlayerController.playerInventory.inventoryDisplay.gameObject.transform
                .parent.gameObject.activeSelf)
        {
            GameManager.Instance.PlayerController.ToggleInventory();
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
        runningSlider.value = playerWalking.currentStamina / GameManager.Instance.PlayerController.characterProperties.maximumStamina;
    }

    public void ChangeCursor(Sprite newCursor, Vector3 newScale)
    {
        CrosshairController.SetNewCrosshair(newCursor);
        CrosshairController.UIImage.rectTransform.localScale = newScale;
    }

    public void TooglePauseMenu(bool forceEnable)
    {
        if (forceEnable)
        {
            GameManager.Instance.PlayerController.cameraController.angleLocked = true;
            GameManager.Instance.PlayerController.cameraController.cursorLock = false;
            Cursor.visible = true;
            GameManager.Instance.PlayerController.stateMachine.enabled = false;
            GameManager.Instance.PlayerController.interactablesManager.enabled = false;
            GameManager.Instance.PlayerController.interactablesManager.ClearInteractable();
            GameManager.Instance.PlayerController.objectInspector.enabled = false;

            //Something about enemy?
            pauseManager.ActivateResumeButton(false);
            pauseMenu.SetActive(true);
            pauseManager.DesactivateOptions();
            return;
        }
        else if (!pauseManager.isActiveAndEnabled && GameManager.Instance.PlayerController.currentBrain.ShowPause)//(GameManager.Instance.PlayerController.currentBrain.ShowPause && GameManager.Instance.PlayerController.stateMachine.lastState is State_Player_Interacting)
        {
            GameManager.Instance.PlayerController.cameraController.angleLocked = true;
            GameManager.Instance.PlayerController.cameraController.cursorLock = false;
            Cursor.visible = true;
            GameManager.Instance.PlayerController.stateMachine.enabled = false;
            GameManager.Instance.PlayerController.interactablesManager.enabled = false;
            GameManager.Instance.PlayerController.interactablesManager.ClearInteractable();
            GameManager.Instance.PlayerController.objectInspector.enabled = false;

            //Something about enemy?
            pauseManager.ActivateResumeButton(true);
            pauseMenu.SetActive(true);
            pauseManager.DesactivateOptions();
            return;
        }
        else if (pauseManager.isActiveAndEnabled && GameManager.Instance.PlayerController.currentBrain.ShowPause)//(GameManager.Instance.PlayerController.currentBrain.ShowPause /*|| forceEnable*/)
        {
            GameManager.Instance.PlayerController.cameraController.angleLocked = false;
            GameManager.Instance.PlayerController.cameraController.cursorLock = true;
            Cursor.visible = false;
            GameManager.Instance.PlayerController.stateMachine.enabled = true;
            GameManager.Instance.PlayerController.interactablesManager.enabled = true;
            GameManager.Instance.PlayerController.interactablesManager.ClearInteractable();
            GameManager.Instance.PlayerController.objectInspector.enabled = true;

            //Something about enemy?
            //pauseManager.ActivateResumeButton(true);
            pauseMenu.SetActive(false);
            pauseManager.DesactivateOptions();
            return;
            /*
            var enabled = pauseMenu.activeInHierarchy;
            GameManager.Instance.PlayerController.cameraController.angleLocked = !enabled;
            GameManager.Instance.PlayerController.cameraController.cursorLock = enabled;
            Cursor.visible = !enabled;
            GameManager.Instance.PlayerController.stateMachine.enabled = enabled;
            GameManager.Instance.PlayerController.interactablesManager.enabled = enabled;
            GameManager.Instance.PlayerController.interactablesManager.ClearInteractable();
            GameManager.Instance.PlayerController.objectInspector.enabled = enabled;

            //Something about enemy?
            pauseManager.ActivateResumeButton(true);
            pauseMenu.SetActive(!enabled);
            pauseManager.DesactivateOptions();
            */
        }
        
    }

    public void ResumeGame()
    {
        var enabled = true;
        GameManager.Instance.PlayerController.cameraController.angleLocked = !enabled;
        GameManager.Instance.PlayerController.cameraController.cursorLock = enabled;
        Cursor.visible = !enabled;
        GameManager.Instance.PlayerController.stateMachine.enabled = enabled;
        GameManager.Instance.PlayerController.interactablesManager.enabled = enabled;
        GameManager.Instance.PlayerController.objectInspector.enabled = enabled;

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