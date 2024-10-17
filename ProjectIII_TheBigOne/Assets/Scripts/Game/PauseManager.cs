using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private CanvasController canvasController;

    [Header("Options")]
    public GameObject options;
    public GameObject controls;
    public GameObject volume;
    public GameObject mouse;
    public GameObject screen;

    [Header("Sliders")]
    public Slider masterVolumeSlider;
    public Slider effectsVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sensibilitySlider;
    public Toggle invertToogle; 
    public Slider brightnessSlider;

    [Header("Buttons")]
    public Button resumeButton;
    public Button restartButton;
    public Button optionsButton;
    public Button exitButton;
    public Button controlsButton;
    public Button volumeButton;
    public Button mouseButton;
    public Button screenButton;

    [Header("Text Inputs")]
    public InputField masterVolumeInput;
    public InputField musicVolumeInput;
    public InputField effectsVolumeInput;
    public InputField sensibilityInput;
    public InputField brightnessInput;


    private void Awake()
    {
        canvasController = FindObjectOfType<CanvasController>();
        UpdateOptions();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (canvasController == null)
        {
            canvasController = FindObjectOfType<CanvasController>();
        }
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
    }

    public void ResumeGame()
    {
        canvasController.ResumeGame();
    }

    public void ActivateOptions()
    {
        options.SetActive(true);
        controls.SetActive(false);
        volume.SetActive(false);
        mouse.SetActive(false);
        screen.SetActive(false);
    }
    public void DesactivateOptions()
    {
        options.SetActive(false);
        controls.SetActive(false);
        volume.SetActive(false);
        mouse.SetActive(false);
        screen.SetActive(false);
    }

    public void ActivateControls()
    {
        options.SetActive(true);
        controls.SetActive(true);
        volume.SetActive(false);
        mouse.SetActive(false);
        screen.SetActive(false);
    }
    public void ActivateVolume()
    {
        options.SetActive(true);
        controls.SetActive(false);
        volume.SetActive(true);
        mouse.SetActive(false);
        screen.SetActive(false);
    }
    public void ActivateMouse()
    {
        options.SetActive(true);
        controls.SetActive(false);
        volume.SetActive(false);
        mouse.SetActive(true);
        screen.SetActive(false);
    }
    public void ActivateScreen()
    {
        options.SetActive(true);
        controls.SetActive(false);
        volume.SetActive(false);
        mouse.SetActive(false);
        screen.SetActive(true);
    }

    public void SetMasterVolume(float volume)
    {
        OptionsManager.Instance.SetMasterVolume(volume);
        masterVolumeInput.text = OptionsManager.Instance.masterVolume.ToString();
    }
    public void SetMasterVolume(string volume)
    {
        OptionsManager.Instance.SetMasterVolume(volume);
        masterVolumeSlider.value = OptionsManager.Instance.masterVolume;
    }

    public void SetMusicVolume(float volume)
    {
        OptionsManager.Instance.SetMusicVolume(volume);
        musicVolumeInput.text = OptionsManager.Instance.musicVolume.ToString();
    }
    public void SetMusicVolume(string volume)
    {
        OptionsManager.Instance.SetMusicVolume(volume);
        musicVolumeSlider.value = OptionsManager.Instance.musicVolume;
    }

    public void SetEffectsVolume(float volume)
    {
        OptionsManager.Instance.SetEffectsVolume(volume);
        effectsVolumeInput.text = OptionsManager.Instance.effectsVolume.ToString();
    }
    public void SetEffectsVolume(string volume)
    {
        OptionsManager.Instance.SetEffectsVolume(volume);
        effectsVolumeSlider.value = OptionsManager.Instance.effectsVolume;
    }

    public void SetSensibility(float sensibility)
    {
        OptionsManager.Instance.SetSensibility(sensibility);
        GameManager.Player.cameraController.m_Sensitivity = OptionsManager.Instance.sensibility;
        sensibilityInput.text = OptionsManager.Instance.sensibility.ToString();
    }
    public void SetSensibility(string sensibility)
    {
        OptionsManager.Instance.SetSensibility(sensibility);
        GameManager.Player.cameraController.m_Sensitivity = OptionsManager.Instance.sensibility;
        sensibilitySlider.value = OptionsManager.Instance.sensibility;
    }

    public void SetInvertMouse(bool invert)
    {
        OptionsManager.Instance.SetInvertMouse(invert);
        GameManager.Player.cameraController.invertMouse = OptionsManager.Instance.invertedMouse;
    }

    public void SetBrightness(float bright)
    {
        OptionsManager.Instance.SetBrightness(bright);
        brightnessInput.text = OptionsManager.Instance.brightness.ToString();
    }
    public void SetBrightness(string bright)
    {
        OptionsManager.Instance.SetBrightness(bright);
        brightnessSlider.value = OptionsManager.Instance.brightness;
    }
    public void UpdateOptions()
    {
        masterVolumeSlider.value = OptionsManager.Instance.masterVolume;
        musicVolumeSlider.value = OptionsManager.Instance.musicVolume;
        effectsVolumeSlider.value = OptionsManager.Instance.effectsVolume;

        sensibilitySlider.value = OptionsManager.Instance.sensibility;
        invertToogle.isOn = OptionsManager.Instance.invertedMouse;

        brightnessSlider.value = OptionsManager.Instance.brightness;       
    }

    public void ActivateResumeButton(bool enabled)
    {
        resumeButton.gameObject.SetActive(enabled);
    }
}
