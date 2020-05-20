using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private CanvasController canvasController;

    public GameObject options;
    public GameObject controls;
    public GameObject volume;
    public GameObject mouse;
    public GameObject screen;

    private void Awake()
    {
        canvasController = FindObjectOfType<CanvasController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (canvasController == null)
        {
            canvasController = FindObjectOfType<CanvasController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canvasController == null)
        {
            canvasController = FindObjectOfType<CanvasController>();
        }
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
}
