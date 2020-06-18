using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    private CanvasController canvasController;

    [Header("Options")]
    public GameObject options;
    public GameObject controls;
    //public GameObject volume;
    public GameObject mouse;
    public GameObject screen;

    [Header("Sliders")]   
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
    public InputField sensibilityInput;
    public InputField brightnessInput;

    [Header("Scenes")]
    public int sceneIDExitGame;

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
        //volume.SetActive(false);
        mouse.SetActive(false);
        screen.SetActive(false);

        controlsButton.Select();
    }
    public void DesactivateOptions()
    {
        options.SetActive(false);
        controls.SetActive(false);
        //volume.SetActive(false);
        mouse.SetActive(false);
        screen.SetActive(false);
    }

    public void ActivateControls()
    {
        options.SetActive(true);
        controls.SetActive(true);
        //volume.SetActive(false);
        mouse.SetActive(false);
        screen.SetActive(false);
    }
    public void ActivateVolume()
    {
        options.SetActive(true);
        controls.SetActive(false);
        //volume.SetActive(true);
        mouse.SetActive(false);
        screen.SetActive(false);
    }
    public void ActivateMouse()
    {
        options.SetActive(true);
        controls.SetActive(false);
        //volume.SetActive(false);
        mouse.SetActive(true);
        screen.SetActive(false);
    }
    public void ActivateScreen()
    {
        options.SetActive(true);
        controls.SetActive(false);
        //volume.SetActive(false);
        mouse.SetActive(false);
        screen.SetActive(true);
    }
   
    public void SetSensibility(float sensibility)
    {
        OptionsManager.Instance.SetSensibility(sensibility);
        GameManager.Instance.PlayerController.cameraController.m_Sensitivity = OptionsManager.Instance.sensibility;
        sensibilityInput.text = OptionsManager.Instance.sensibility.ToString();
    }
    public void SetSensibility(string sensibility)
    {
        OptionsManager.Instance.SetSensibility(sensibility);
        GameManager.Instance.PlayerController.cameraController.m_Sensitivity = OptionsManager.Instance.sensibility;
        sensibilitySlider.value = OptionsManager.Instance.sensibility;
    }

    public void SetInvertMouse(bool invert)
    {
        OptionsManager.Instance.SetInvertMouse(invert);
        GameManager.Instance.PlayerController.cameraController.invertMouse = OptionsManager.Instance.invertedMouse;
    }

    public void SetHudHidden(bool hidden)
    {
        GameManager.Instance.GameSettings.showHud = !hidden;
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

        sensibilitySlider.value = OptionsManager.Instance.sensibility;
        invertToogle.isOn = OptionsManager.Instance.invertedMouse;

        brightnessSlider.value = OptionsManager.Instance.brightness;       
    }

    public void ActivateResumeButton(bool enabled)
    {
        resumeButton.gameObject.SetActive(enabled);
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartWholeGame();
    }
    public void ExitGame()
    {
        GameManager.Instance.ExitGame(sceneIDExitGame);
    }
}
