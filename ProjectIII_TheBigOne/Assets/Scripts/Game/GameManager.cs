using System;
using Tavaris.Entities;
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
                m_Instance = (GameManager)FindObjectOfType(typeof(GameManager));
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
    public static PlayerController Player { get; private set; }
    public CanvasController CanvasController;
    public PlayerInventory PlayerInventory { get; private set; } = new PlayerInventory();

    public string voidAmbientPath;

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

        if (Player == null)
        {
            Player = FindObjectOfType<PlayerController>();
        }

        if (CanvasController == null)
        {
            CanvasController = FindObjectOfType<CanvasController>();
        }
    }

    public void RestartWholeGame()
    {
        SoundManager.Instance.StopAllEvents(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //StartGame();
    }

    public void StartGame()
    {
        // Hide all menus. (Inventory, Run, Pause Menu)
        // Resurrect player.
        // Set initial parameters?
        //SoundManager.Instance.PlayEvent(voidAmbientPath, PlayerController.transform, .2f);
        HideAllMenus();
        CanvasController.blackFade.gameObject.SetActive(false);

        Player.Resurrect();
    }

    public void EndGame()
    {
        // Hide all menus. (Inventory, Run, Pause Menu)
        // Disable Player controls. (The player does this by itself)
        // Set player dead.
        HideAllMenus();
        CanvasController.blackFade.gameObject.SetActive(true);

        Player.Kill();
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
        SceneManager.LoadScene(sceneName);
    }
    #region Inventory
    public void RemoveItemFromPlayerInventory(string id)
    {
        PlayerInventory.RemoveItem(id);
    }
    public bool HasPlayerThisItemInInventory(string id)
    {
        return PlayerInventory.HasItem(id);
    }
    public bool GiveItemToPlayer(string itemID)
    {
        return PlayerInventory.GiveItem(itemID);
    }
    #endregion
}