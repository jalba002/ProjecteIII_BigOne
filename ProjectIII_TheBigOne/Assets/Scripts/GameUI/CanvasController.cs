﻿using System;
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

    [Header("Sliders")] public Image lightBulb;
    public Slider runningSlider;

    [Header("Components")] public CrosshairController CrosshairController;
    public RectTransform UILight;
    public RectTransform UIRun;
    public Image blackFade;

    private FlashlightController flashlight;
    private State_Player_Walking playerWalking;
    public PauseManager pauseManager;

    private PlayerController _playerController;
    private bool _islightBulbNotNull;

    private static UIFade currentUIFade;

    private void Start()
    {
        _islightBulbNotNull = lightBulb != null;
        _playerController = GameManager.Instance.PlayerController;

        if (CrosshairController == null)
            CrosshairController = FindObjectOfType<CrosshairController>();

        flashlight = _playerController.attachedFlashlight;

        ToggleHud(false);
        //_pauseManager = GetComponentInChildren<PauseManager>();
    }

    void Update()
    {
        PlayerLightingUpdate();
        PlayerRunningUpdate();

        if (!_playerController.IsDead)
            TooglePauseMenu(false);


        if (Input.GetKeyDown(KeyCode.Escape) && _playerController.playerInventory.inventoryDisplay.gameObject.transform
                .parent.gameObject.activeSelf)
        {
            _playerController.ToggleInventory();
        }

        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ShowHint("metas are your friends", true); // true = upper
        }*/
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
        //lightingSlider.value = flashlight.currentCharge / flashlight.maxCharge;
        if (_islightBulbNotNull)
            lightBulb.fillAmount = flashlight.currentCharge / flashlight.maxCharge;
    }

    public void PlayerRunningUpdate()
    {
        if (!GameManager.Instance.GameSettings.showHud) return;

        if (_playerController.currentStamina >= _playerController.characterProperties.maximumStamina)
            runningSlider.gameObject.SetActive(false);
        else
            runningSlider.gameObject.SetActive(true);

        runningSlider.value = _playerController.currentStamina / _playerController.characterProperties.maximumStamina;
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
            _playerController.stateMachine.enabled = false;
            _playerController.interactablesManager.enabled = false;
            
            try
            {
                if (_playerController.interactablesManager.CurrentInteractable.interactionType !=
                    InteractableObject.InteractionType.Inspect)
                {
                    _playerController.interactablesManager.ClearInteractable();
                }
            }
            catch (NullReferenceException)
            {
            }

            _playerController.objectInspector.enabled = false;
            _playerController.cameraController.angleLocked = true;
            _playerController.cameraController.cursorLock = false;
            Cursor.visible = true;

            //Something about enemy?
            pauseManager.ActivateResumeButton(false);
            pauseMenu.SetActive(true);
            pauseManager.resumeButton.Select();
            pauseManager.DesactivateOptions();
            return;
        }
        else if (!pauseManager.isActiveAndEnabled && _playerController.currentBrain.ShowPause
        ) //(GameManager.Instance.PlayerController.currentBrain.ShowPause && GameManager.Instance.PlayerController.stateMachine.lastState is State_Player_Interacting)
        {
            _playerController.stateMachine.enabled = false;
            _playerController.interactablesManager.enabled = false;

            try
            {
                if (_playerController.interactablesManager.CurrentInteractable.interactionType !=
                    InteractableObject.InteractionType.Inspect)
                {
                    _playerController.interactablesManager.ClearInteractable();
                }
            }
            catch (NullReferenceException)
            {
            }

            _playerController.objectInspector.enabled = false;
            _playerController.cameraController.angleLocked = true;
            _playerController.cameraController.cursorLock = false;
            Cursor.visible = true;

            //Something about enemy?
            pauseManager.ActivateResumeButton(true);
            pauseMenu.SetActive(true);
            pauseManager.resumeButton.Select();
            pauseManager.DesactivateOptions();
            return;
        }
        else if (pauseManager.isActiveAndEnabled && _playerController.currentBrain.ShowPause
        ) //(GameManager.Instance.PlayerController.currentBrain.ShowPause /*|| forceEnable*/)
        {
            _playerController.stateMachine.enabled = true;
            _playerController.interactablesManager.enabled = true;

            try
            {
                if (_playerController.interactablesManager.CurrentInteractable.interactionType !=
                    InteractableObject.InteractionType.Inspect)
                {
                    _playerController.interactablesManager.ClearInteractable();
                }
            }
            catch (NullReferenceException)
            {
            }

            _playerController.objectInspector.enabled = true;

            if (!(_playerController.stateMachine.GetCurrentState is State_Player_Inspecting))
                _playerController.cameraController.angleLocked = false;

            _playerController.cameraController.cursorLock = true;
            Cursor.visible = false;

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
        _playerController.cameraController.angleLocked = !enabled;
        _playerController.cameraController.cursorLock = enabled;
        Cursor.visible = !enabled;
        _playerController.stateMachine.enabled = enabled;
        _playerController.interactablesManager.enabled = enabled;
        _playerController.objectInspector.enabled = enabled;

        //Something about enemy?

        pauseMenu.SetActive(!enabled);
    }

    public void PushObjectiveText(string text, float time, bool upper, int id)
    {
        //play new objective sound
        SoundManager.Instance.PlaySound2D("event:/SFX/UI/Inventory/NewQuest");

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
                //Play objective completed sound
                SoundManager.Instance.PlaySound2D("event:/SFX/UI/Inventory/CompletedQuest");
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

    public void ShowHint(string text, bool upper, float fadeTime = 3f,
        UIFade.FadeOutAfter fadeType = UIFade.FadeOutAfter.Time)
    {
        HintNotification.SetActive(true);

        Text hintNotificationText = HintNotification.transform.GetChild(0).GetComponent<Text>();

        if (upper)
            hintNotificationText.text = text.ToUpper();
        else
            hintNotificationText.text = text;

        UIFade uIFade = UIFade.CreateInstance(HintNotification, "[UIFader] HintNotification");
        uIFade.ResetGraphicsColor();
        uIFade.ImageTextAlpha(0.8f, 1f);
        uIFade.FadeInOut(fadeOutTime: fadeTime, fadeOutAfter: fadeType);
    }

    public void DisplayInteractableHint(string text, float fadeTime = 1f)
    {
        Text hintNotificationText = HintNotification.transform.GetChild(0).GetComponent<Text>();

        hintNotificationText.text = text.ToUpper();

        if (currentUIFade == null)
        {
            currentUIFade = UIFade.CreateInstance(HintNotification, "[UIFader] HintNotification", false);
            currentUIFade.ResetGraphicsColor();
            currentUIFade.ImageTextAlpha(0.8f, 1f);
            currentUIFade.FadeInOut(fadeOutTime: fadeTime, fadeOutAfter: UIFade.FadeOutAfter.Bool);
        }
        else
        {
            currentUIFade.fadeOut = false;
            currentUIFade.FadeInOut(fadeOutTime: fadeTime, fadeOutAfter: UIFade.FadeOutAfter.Bool);
        }
    }

    public void HideInteractableHint()
    {
        UIFade currentUIFade = UIFade.FindUIFader(HintNotification);
        if (currentUIFade != null)
        {
            currentUIFade.fadeOut = true;
            return;
        }
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

    #region HUD

    public void ToggleHud(bool enableHud)
    {
        if (UILight != null)
        {
            UILight.gameObject.SetActive(enableHud);
        }

        if (UIRun != null)
        {
            UIRun.gameObject.SetActive(enableHud);
        }
    }

    #endregion
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

    public ObjectiveModel()
    {
    }
}