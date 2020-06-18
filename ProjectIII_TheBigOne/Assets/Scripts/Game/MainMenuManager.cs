using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    private OptionsManager m_OptionsManager;

    public GameObject b_options;
    public GameObject b_play;
    public GameObject b_exit;

    private List<GameObject> buttons = new List<GameObject>();

    public GameObject optionsmenu;

    private GameObject selectedOption;

    public int sceneIDToLoad = 1;

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

    [Header("Text Inputs")]
    public InputField masterVolumeInput;
    public InputField musicVolumeInput;
    public InputField effectsVolumeInput;
    public InputField sensibilityInput;
    public InputField brightnessInput;

    [Header("Buttons")]
    public Button controlsButton;
    public Button volumeButton;
    public Button mouseButton;
    public Button screenButton;

    [Header("Others")]
    public GameObject computer;
    public Material closedComputer;
    public Material openingComputer;
    public Material openedComputer;
    public GameObject lightComputer;
    private float cooldown;
    private float maxCooldown = 0.2f;

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
                if (computer.GetComponent<MeshRenderer>().material != openingComputer && cooldown > maxCooldown)
                {
                    computer.GetComponent<MeshRenderer>().material = openingComputer;
                    lightComputer.SetActive(true);
                }
                else if (cooldown <= maxCooldown)
                {
                    cooldown += Time.deltaTime;
                }

            }
            else
            {
                if (computer.GetComponent<MeshRenderer>().material != openedComputer)
                {
                    computer.GetComponent<MeshRenderer>().material = openedComputer;
                    if (!optionsmenu.activeInHierarchy)
                    {
                        ActivateOptions();
                        optionsmenu.SetActive(true);
                        lightComputer.SetActive(true);
                    }


                }
            }

            //escape

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ChangeOptions(false);
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
                if (cooldown < maxCooldown)
                {
                    computer.GetComponent<MeshRenderer>().material = openingComputer;
                }
                else
                {
                    computer.GetComponent<MeshRenderer>().material = closedComputer;
                    lightComputer.SetActive(false);
                }
            }
            else
            {
                if (computer.GetComponent<MeshRenderer>().material != closedComputer)
                {
                    computer.GetComponent<MeshRenderer>().material = closedComputer;
                    lightComputer.SetActive(false);
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (selectedOption == b_play)
                {
                    PlayGame();
                }
                else if (selectedOption == b_options)
                {
                    ChangeOptions(true);
                }
                else
                {

                }
            }
        }

    }

    public void SelectOptionWithButton(int option)
    {
        if (option == 0)
        {
            selectedOption = buttons[0];
            buttons[1].SetActive(false);
            buttons[2].SetActive(false);
            buttons[0].SetActive(true);
        }
        else if (option == 1)
        {
            selectedOption = buttons[1];
            buttons[2].SetActive(false);
            buttons[0].SetActive(false);
            buttons[1].SetActive(true);
        }
        else
        {
            selectedOption = buttons[2];
            buttons[0].SetActive(false);
            buttons[1].SetActive(false);
            buttons[2].SetActive(true);
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
        else if (selectedOption == buttons[1])
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
        //SceneManager.LoadScene(sceneIDToLoad);
        FindObjectOfType<LevelTransitionController>().BlackFadeOut();
    }
    public void ChangeOptions(bool b)
    {
        if (b)
        {
            //optionsmenu.SetActive(true);
            //ActivateOptions();
            b_options.SetActive(false);
            selectedOption = optionsmenu;
            cooldown = 0;
        }
        else
        {
            optionsmenu.SetActive(false);
            b_options.SetActive(true);
            b_play.SetActive(false);
            b_exit.SetActive(false);
            selectedOption = b_options;
            cooldown = 0;
        }
    }

    public void ActivateOptions()
    {
        options.SetActive(true);
        controls.SetActive(false);
        volume.SetActive(false);
        mouse.SetActive(false);
        screen.SetActive(false);

        controlsButton.Select();
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
        //Cambiar a un valor entre 0 y 1
        masterVolumeInput.text = masterVolumeSlider.value.ToString();

        //masterVolumeInput.text = ((OptionsManager.Instance.masterVolume - OptionsManager.Instance.minVolume) / (OptionsManager.Instance.maxVolume - OptionsManager.Instance.minVolume)).ToString();
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
        //musicVolumeInput.text = ((OptionsManager.Instance.musicVolume - OptionsManager.Instance.minVolume) / (OptionsManager.Instance.maxVolume - OptionsManager.Instance.minVolume)).ToString();
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
        //effectsVolumeInput.text = ((OptionsManager.Instance.effectsVolume - OptionsManager.Instance.minVolume) / (OptionsManager.Instance.maxVolume - OptionsManager.Instance.minVolume)).ToString();
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
        sensibilityInput.text = OptionsManager.Instance.sensibility.ToString();
        //GameManager.Instance.PlayerController.cameraController.m_Sensitivity = OptionsManager.Instance.sensitivity;
    }
    public void SetSensibility(string sensibility)
    {
        OptionsManager.Instance.SetSensibility(sensibility);
        //GameManager.Instance.PlayerController.cameraController.m_Sensitivity = OptionsManager.Instance.sensibility;
        sensibilitySlider.value = OptionsManager.Instance.sensibility;
    }

    public void SetInvertMouse(bool invert)
    {
        OptionsManager.Instance.SetInvertMouse(invert);
        //GameManager.Instance.PlayerController.cameraController.invertMouse = OptionsManager.Instance.invertedMouse;
    }

    public void SetBrightness(float bright)
    {
        OptionsManager.Instance.SetBrightness(bright);
        /*
        string t = ((OptionsManager.Instance.brightness - OptionsManager.Instance.minBrightness) / (OptionsManager.Instance.maxBrightness - OptionsManager.Instance.minBrightness)).ToString();
        Debug.Log(t);


        brightnessInput.text = t;
        */
        brightnessInput.text = bright.ToString();
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

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
