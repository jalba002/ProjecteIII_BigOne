﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Enemy;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private static GameManager m_Instance = null;

    public static GameManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (GameManager) FindObjectOfType(typeof(GameManager));
                if (m_Instance == null)
                {
                    m_Instance = (new GameObject("GameManager")).AddComponent<GameManager>();
                    m_Instance.transform.parent = null;
                }

                //DontDestroyOnLoad(m_Instance.gameObject);
            }

            return m_Instance;
        }
    }

    public GameSettings GameSettings;
    public PlayerController PlayerController;
    public CanvasController CanvasController;

    public string voidAmbientPath;
    ObjectTracker _objTracker;

    public void Awake()
    {
        if (GameSettings == null)
        {
            GameSettings = ScriptableObject.CreateInstance<GameSettings>();
        }
        else
        {
            GameSettings = Instantiate(GameSettings);
        }

        if (PlayerController == null)
        {
            PlayerController = FindObjectOfType<PlayerController>();
        }

        if (CanvasController == null)
        {
            CanvasController = FindObjectOfType<CanvasController>();
        }

        if (_objTracker == null)
        {
            _objTracker = FindObjectOfType<ObjectTracker>();
            _objTracker.StartObjectTracker();
        }
    }

    public void Start()
    {
        PlayStartupAnimation();
    }

    private void PlayStartupAnimation()
    {
        if (!GameSettings.playStartupAnimation) return;

        try
        {
            PlayerController.cameraController.attachedCamera.GetComponent<Animation>().Play();
            PlayerController.interactablesManager.enabled = false;
            PlayerController.cameraController.angleLocked = true;
            PlayerController.currentBrain.enabled = false;
            PlayerController.stateMachine.enabled = false;
        }
        catch (NullReferenceException)
        {
            Debug.LogWarning("Not playing startup Animation.");
        }
    }

    public void RestartWholeGame()
    {
        SoundManager.Instance.StopAllEvents(true);
        SoundManager.Instance.StopAllMovingEvents(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartGame();
    }

    public void StartGame()
    {
        // Hide all menus. (Inventory, Run, Pause Menu)
        // Resurrect player.
        // Set initial parameters?
        //SoundManager.Instance.PlayEvent(voidAmbientPath, PlayerController.transform, .2f);
        HideAllMenus();
        CanvasController.blackFade.gameObject.SetActive(false);

        PlayerController.Resurrect();

        //Object Tracker
        _objTracker.StartObjectTracker();
    }

    public void EndGame()
    {
        // Hide all menus. (Inventory, Run, Pause Menu)
        // Disable Player controls. (The player does this by itself)
        // Set player dead.
        HideAllMenus();
        CanvasController.blackFade.gameObject.SetActive(true);

        PlayerController.Kill();
    }

    public void WinGame()
    {
        try
        {
            Debug.Log("WINNING");
            //PlayerController.cameraController.attachedCamera.GetComponent<Animation>().Play();
            PlayerController.interactablesManager.enabled = false;
            PlayerController.cameraController.angleLocked = true;
            PlayerController.currentBrain.enabled = false;
            PlayerController.stateMachine.enabled = false;
            HideAllMenus();
            FindObjectOfType<ElevatorUnlocker>().elevatorAnimation.SetTrigger("Close");
        }
        catch (NullReferenceException)
        {
            Debug.LogWarning("NOT WINNING");
        }

        Debug.Log("Winning by death");
        Invoke(nameof(EndGame), 5f);
    }

    private void ReturnToMenu()
    {
        SoundManager.Instance.StopAllEvents(true);
        SoundManager.Instance.StopAllMovingEvents(true);
        SceneManager.LoadScene(0);
    }

    private void HideAllMenus()
    {
        try
        {
            CanvasController.pauseMenu.SetActive(false);
            CanvasController.inventory.SetActive(false);
        }
        catch (NullReferenceException)
        {
        }
    }

    public void ExitGame(string sceneName)
    {
        SoundManager.Instance.StopAllEvents(true);
        SoundManager.Instance.StopAllMovingEvents(true);
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame(int sceneID)
    {
        SoundManager.Instance.StopAllEvents(true);
        SoundManager.Instance.StopAllMovingEvents(true);
        SceneManager.LoadScene(sceneID);
    }
}