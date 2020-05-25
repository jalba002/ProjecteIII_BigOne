using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    private static OptionsManager m_Instance = null;

    public static OptionsManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (OptionsManager) FindObjectOfType(typeof(OptionsManager));
                if (m_Instance == null)
                {
                    m_Instance = (new GameObject("PauseManager")).AddComponent<OptionsManager>();
                }

                DontDestroyOnLoad(m_Instance.gameObject);
            }

            return m_Instance;
        }
    }
    
    private static OptionsData _optionsData;

    public static OptionsData optionsData
    {
        get
        {
            if (_optionsData == null)
            {
                _optionsData = ScriptableObject.CreateInstance<OptionsData>();
            }

            return _optionsData;
        }
    }


    public void Start()
    {
        UpdateOptions();
    }

    public void SetMasterVolume(float volume)
    {
        optionsData.masterVolume = volume;
        UpdateOptions();
    }

    public void SetMusicVolume(float volume)
    {
        optionsData.musicVolume = volume;
        UpdateOptions();
    }

    public void SetEffectsVolume(float volume)
    {
        optionsData.effectsVolume = volume;
        UpdateOptions();
    }

    public void SetSensibility(float sensibility)
    {
        optionsData.sensitivity = sensibility;
        UpdateOptions();
    }

    public void SetInvertMouse(bool invert)
    {
        optionsData.invertedMouse = invert;
        UpdateOptions();
    }

    public void UpdateOptions()
    {
        Debug.Log("Updated Options");
        AudioManager.Instance.audioMixer.SetFloat("MasterVolume", optionsData.masterVolume);
        AudioManager.Instance.audioMixer.SetFloat("MusicVolume", optionsData.musicVolume);
        AudioManager.Instance.audioMixer.SetFloat("EffectsVolume", optionsData.effectsVolume);
        try
        {
            GameManager.Instance.PlayerController.cameraController.m_Sensitivity =
                optionsData.sensitivity;
            GameManager.Instance.PlayerController.cameraController.invertMouse =
                optionsData.invertedMouse;
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}