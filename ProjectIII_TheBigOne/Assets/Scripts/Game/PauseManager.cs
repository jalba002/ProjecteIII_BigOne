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
    }

    public void SetMusicVolume(float volume)
    {
        OptionsManager.Instance.SetMusicVolume(volume);
    }

    public void SetEffectsVolume(float volume)
    {
        OptionsManager.Instance.SetEffectsVolume(volume);
    }

    public void SetSensibility(float sensibility)
    {
        OptionsManager.Instance.SetSensibility(sensibility);
        GameManager.Instance.PlayerController.cameraController.m_Sensitivity = OptionsManager.Instance.sensitivity;
    }

    public void SetInvertMouse(bool invert)
    {
        OptionsManager.Instance.SetInvertMouse(invert);
        GameManager.Instance.PlayerController.cameraController.invertMouse = OptionsManager.Instance.invertedMouse;
    }

    public void SetBrightness(float bright)
    {
        OptionsManager.Instance.SetBrightness(bright);
    }
    public void UpdateOptions()
    {
        masterVolumeSlider.value = OptionsManager.Instance.masterVolume;
        musicVolumeSlider.value = OptionsManager.Instance.musicVolume;
        effectsVolumeSlider.value = OptionsManager.Instance.effectsVolume;

        sensibilitySlider.value = OptionsManager.Instance.sensitivity;
        invertToogle.isOn = OptionsManager.Instance.invertedMouse;

        brightnessSlider.value = OptionsManager.Instance.brightness;       
    }

    public void ActivateResumeButton(bool enabled)
    {
        resumeButton.gameObject.SetActive(enabled);
    }
}
