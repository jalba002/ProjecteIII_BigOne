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

    [Header("MOUSE")] public float minSensibility = 0.2f;
    public float maxSensibility = 4f;
    [Range(0.2f, 4f)] public float sensibility = 1f;
    public bool invertedMouse = false;

    [Header("VOLUME")] public float minVolume = 0f;
    public float maxVolume = 100f;
    [Range(0f, 100f)] public float masterVolume = 100f;
    [Range(0f, 100f)] public float musicVolume = 100f;
    [Range(0f, 100f)] public float effectsVolume = 100f;

    [Header("SCREEN")] public float minBrightness = 0;
    public float maxBrightness = 1;
    [Range(0, 1f)] public float brightness = 0.5f;
    public float brightnessMainMenu = 0.5f;


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
        SoundManager.Instance.SetChannelVolume("General", volume);
        //set master volume on FMOD
    }

    public void SetMasterVolume(string vol)
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

        masterVolume = Mathf.Lerp(-80, 0, a / 100f);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        SoundManager.Instance.SetChannelVolume("Music", volume);
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

        musicVolume = Mathf.Lerp(-80, 0, a / 100f);
    }

    public void SetEffectsVolume(float volume)
    {
        effectsVolume = volume;
        Debug.Log(volume);
        SoundManager.Instance.SetChannelVolume("Sound", volume);
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

        effectsVolume = Mathf.Lerp(-80, 0, a / 100f);
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
        brightnessMainMenu = bright;
        FindObjectOfType<PostProcessVolume>().profile.TryGetSettings(out ColorGrading cg);
        if (cg != null)
        {
            if (FindObjectOfType<MainMenuManager>() != null)
            {
                cg.brightness.value = brightnessMainMenu * 25;
            }
            else
            {
                cg.brightness.value = brightness * 25;
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