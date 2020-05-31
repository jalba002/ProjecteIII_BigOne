using UnityEngine;
using System.Collections.Generic;
using Aura2API;
using UnityEngine.Rendering.PostProcessing;

public class OptionsManager : MonoBehaviour
{
    private static OptionsManager m_Instance = null;

    public static OptionsManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<OptionsManager>();
                if (m_Instance == null)
                {
                    m_Instance = new GameObject("OptionsManager").AddComponent<OptionsManager>();
                    DontDestroyOnLoad(m_Instance);
                }
            }
            return m_Instance;
        }
        set { m_Instance = value; }
    }

    private MainMenuManager mainMenuManager;

    [Header("MOUSE")] [Range(0.1f, 10.0f)] public float sensitivity = 1f;
    public bool invertedMouse = false;

    [Header("VOLUME")] [Range(-80f, 0f)] public float masterVolume = 1f;
    [Range(-80f, 0f)] public float musicVolume = 1f;
    [Range(-80f, 0f)] public float effectsVolume = 1f;

    [Header("SCREEN")] [Range(0f, 30f)] public float brightness = 1f;

    private void InstantiateElement()
    {
        if (m_Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void Awake()
    {
        InstantiateElement();
        mainMenuManager = FindObjectOfType<MainMenuManager>();
    }
    

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        //set master volume on FMOD
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        //set music volume on FMOD
    }

    public void SetEffectsVolume(float volume)
    {
        effectsVolume = volume;
        //set effects volume on FMOD
    }

    public void SetSensibility(float sensibility)
    {
        sensitivity = sensibility;        
    }

    public void SetInvertMouse(bool invert)
    {
        invertedMouse = invert;        
    }

    public void SetBrightness(float bright)
    {
        brightness = bright;
        FindObjectOfType<PostProcessVolume>().profile.TryGetSettings(out ColorGrading cg);
        if (cg != null)
        {
            cg.brightness.value = brightness;
        }
    }

    public void UpdateOptions()
    {
        mainMenuManager.masterVolumeSlider.value = masterVolume;
        mainMenuManager.musicVolumeSlider.value = musicVolume;
        mainMenuManager.effectsVolumeSlider.value = effectsVolume;

        mainMenuManager.sensibilitySlider.value = sensitivity;
        mainMenuManager.invertToogle.isOn = invertedMouse;

        mainMenuManager.brightnessSlider.value = brightness;       
    }
}