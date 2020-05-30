using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private OptionsManager m_OptionsManager;

    public GameObject b_options;
    public GameObject b_play;
    public GameObject b_exit;

    private List<GameObject> buttons = new List<GameObject>();

    public GameObject optionsmenu;

    private GameObject selectedOption;

    public string sceneName;

    public GameObject camera;
    public GameObject cameraPointOptions;
    public GameObject cameraPointNormal;

    public AudioMixer audioMixer;

    public float speed = 3;

    [Header("Sliders")]
    public Slider masterVolumeSlider;
    public Slider effectsVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sensibilitySlider;
    public Toggle invertToogle;
    public Slider brightnessSlider;

    [Header("Options")]
    public GameObject options;
    public GameObject controls;
    public GameObject volume;
    public GameObject mouse;
    public GameObject screen;

    // Start is called before the first frame update
    void Start()
    {
        UpdateOptions();
    }

    private void Awake()
    {
        buttons.Add(b_play);
        buttons.Add(b_options);
        buttons.Add(b_exit);

        selectedOption = b_play;

        optionsmenu.SetActive(false);
        b_options.SetActive(false);
        b_play.SetActive(false);
        b_exit.SetActive(false);

        selectedOption.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) && selectedOption != optionsmenu)
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) && selectedOption != optionsmenu)
        {
            MoveRight();
        }

       //Camera Movement
        Vector3 targetDirection = selectedOption.transform.position - camera.transform.position;
        float singleStep = speed * Time.deltaTime;    
        Vector3 newDirection = Vector3.RotateTowards(camera.transform.forward, targetDirection, singleStep, 0.0f);      
        camera.transform.rotation = Quaternion.LookRotation(newDirection);

        if (selectedOption == optionsmenu)
        {
            Vector3 forward = (cameraPointOptions.transform.position - camera.transform.position).normalized;
            float singleStep_2 = speed * Time.deltaTime;
            //Vector3 newDirection_2 = Vector3.MoveTowards(camera.transform.forward, targetDirection_2, singleStep_2);
            if (Vector3.Distance(cameraPointOptions.transform.position, camera.transform.position) > 0.05f)
            {
                camera.transform.position += forward * singleStep_2;
            }
            
        }
        else
        {
            Vector3 forward = (cameraPointNormal.transform.position - camera.transform.position).normalized;
            float singleStep_2 = speed * Time.deltaTime;
            //Vector3 newDirection_2 = Vector3.MoveTowards(camera.transform.forward, targetDirection_2, singleStep_2);
            if (Vector3.Distance(cameraPointNormal.transform.position, camera.transform.position) > 0.05f)
            {
                camera.transform.position += forward * singleStep_2;
            }
        }
        
    }

    public void MoveLeft()
    {
        if (selectedOption == buttons[2])
        {
            selectedOption = buttons[1];
            buttons[2].SetActive(false);
            buttons[1].SetActive(true);
        }
        else if(selectedOption == buttons[1])
        {
            selectedOption = buttons[0];
            buttons[1].SetActive(false);
            buttons[0].SetActive(true);
        }
        else
        {
            selectedOption = buttons[2];
            buttons[0].SetActive(false);
            buttons[2].SetActive(true);
        }
        
    }
    public void MoveRight()
    {
        if (selectedOption == buttons[2])
        {
            selectedOption = buttons[0];
            buttons[2].SetActive(false);
            buttons[0].SetActive(true);
        }
        else if (selectedOption == buttons[1])
        {
            selectedOption = buttons[2];
            buttons[1].SetActive(false);
            buttons[2].SetActive(true);
        }
        else
        {
            selectedOption = buttons[1];
            buttons[0].SetActive(false);
            buttons[1].SetActive(true);
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ChangeOptions(bool b)
    {
        if (b)
        {
            optionsmenu.SetActive(true);
            b_options.SetActive(false);
            selectedOption = optionsmenu;
        }
        else
        {
            optionsmenu.SetActive(false);
            b_options.SetActive(true);
            selectedOption = b_options;
        }
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
        //GameManager.Instance.PlayerController.cameraController.m_Sensitivity = OptionsManager.Instance.sensitivity;
    }

    public void SetInvertMouse(bool invert)
    {
        OptionsManager.Instance.SetInvertMouse(invert);
        //GameManager.Instance.PlayerController.cameraController.invertMouse = OptionsManager.Instance.invertedMouse;
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

}
