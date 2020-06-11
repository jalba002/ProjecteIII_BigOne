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

    [Header("MOUSE")]
    public float minSensibility = 0.2f;
    public float maxSensibility = 4f;
    [Range(0.2f, 4f)] public float sensibility = 1f;
    public bool invertedMouse = false;

    [Header("VOLUME")]
    public float minVolume = -80f;
    public float maxVolume = 0f;
    [Range(-80f, 0f)] public float masterVolume = 1f;
    [Range(-80f, 0f)] public float musicVolume = 1f;
    [Range(-80f, 0f)] public float effectsVolume = 1f;

    [Header("SCREEN")]
    public float minBrightness = -25f;
    public float maxBrightness = 25f;
    [Range(-25f, 25f)] public float brightness = -25f;
    public float brightnessMainMenu = -75f;

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
    public void SetMasterVolume(string vol)
    {
        float.TryParse(vol, out float a);
        if(a < minVolume)
        {
            a = minVolume;
        }
        else if (a > maxVolume)
        {
            a = maxVolume;
        }
        masterVolume = a;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        //set music volume on FMOD
    }
    public void SetMusicVolume(string vol)
    {
        float.TryParse(vol, out float a);
        if (a < minVolume)
        {
            a = minVolume;
        }
        else if (a > maxVolume)
        {
            a = maxVolume;
        }
        musicVolume = a;
    }

    public void SetEffectsVolume(float volume)
    {
        effectsVolume = volume;
        //set effects volume on FMOD
    }
    public void SetEffectsVolume(string vol)
    {
        float.TryParse(vol, out float a);
        if (a < minVolume)
        {
            a = minVolume;
        }
        else if (a > maxVolume)
        {
            a = maxVolume;
        }
        effectsVolume = a;
    }

    public void SetSensibility(float sens)
    {
        sensibility = sens;        
    }
    public void SetSensibility(string sens)
    {
        float.TryParse(sens, out float a);
        if (a < minSensibility)
        {
            a = minSensibility;
        }
        else if (a > maxSensibility)
        {
            a = maxSensibility;
        }
        sensibility = a;
    }

    public void SetInvertMouse(bool invert)
    {
        invertedMouse = invert;        
    }

    public void SetBrightness(float bright)
    {
        brightness = bright;
        brightnessMainMenu = bright - 50f;
        FindObjectOfType<PostProcessVolume>().profile.TryGetSettings(out ColorGrading cg);
        if (cg != null)
        {
            if (FindObjectOfType<MainMenuManager>() != null)
            {
                cg.brightness.value = brightnessMainMenu;
            }
            else
            {
                cg.brightness.value = brightness;
            }
        }
    }
    public void SetBrightness(string bright)
    {
        float.TryParse(bright, out float a);
        if (a < minBrightness)
        {
            a = minBrightness;
        }
        else if (a > maxBrightness)
        {
            a = maxBrightness;
        }
        SetBrightness(a);
    }

    public void UpdateOptions()
    {
        mainMenuManager.masterVolumeSlider.value = masterVolume;
        mainMenuManager.musicVolumeSlider.value = musicVolume;
        mainMenuManager.effectsVolumeSlider.value = effectsVolume;

        mainMenuManager.sensibilitySlider.value = sensibility;
        mainMenuManager.invertToogle.isOn = invertedMouse;

        mainMenuManager.brightnessSlider.value = brightness;       
    }
}