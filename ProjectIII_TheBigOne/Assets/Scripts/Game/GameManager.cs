using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_Instance = null;
    public static GameManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (GameManager) FindObjectOfType(typeof(GameManager ));
                if (m_Instance == null)
                {
                    m_Instance = (new GameObject("GameManager")).AddComponent<GameManager>();
                }
                DontDestroyOnLoad(m_Instance.gameObject);
            }
            return m_Instance;
        }
    }

    public GameSettings GameSettings;
    public PlayerController PlayerController;
    
    public void Awake()
    {
        if (GameSettings == null)
        {
            GameSettings = ScriptableObject.CreateInstance<GameSettings>();
        }

        if (PlayerController == null)
        {
            PlayerController = FindObjectOfType<PlayerController>();
        }
    }
    
    
}
