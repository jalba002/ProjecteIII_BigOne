using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour
{
    public GameObject b_options;
    public GameObject b_play;
    public GameObject b_exit;

    private List<GameObject> buttons = new List<GameObject>();

    public GameObject optionsmenu;

    private GameObject selectedOption;

    public string sceneName;

    public GameObject camera;

    public AudioMixer audioMixer;

    public float speed = 3;

    // Start is called before the first frame update
    void Start()
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
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }

       //Camera Movement
        Vector3 targetDirection = selectedOption.transform.position - camera.transform.position;
        float singleStep = speed * Time.deltaTime;    
        Vector3 newDirection = Vector3.RotateTowards(camera.transform.forward, targetDirection, singleStep, 0.0f);      
        camera.transform.rotation = Quaternion.LookRotation(newDirection);
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

    


}
