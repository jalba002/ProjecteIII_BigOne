using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using CharacterController = Characters.Generic.CharacterController;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsManager : MonoBehaviour
{
    private PlayerController playerController;
    public AudioMixer audioMixer;


    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ActualizeOptions();
    }

    public void SetMasterVolume(float volume)
    {     
        GameManager.Instance.OptionsData.masterVolume = volume;
    }
    public void SetMusicVolume(float volume)
    {
        GameManager.Instance.OptionsData.musicVolume = volume;
    }
    public void SetEffectsVolume(float volume)
    {
        GameManager.Instance.OptionsData.effectsVolume = volume;
    }

    public void SetSensibility(float sensibility)
    {
        GameManager.Instance.OptionsData.sensitivity = sensibility;
    }
    public void SetInvertMouse(bool invert)
    {
        GameManager.Instance.OptionsData.invertedMouse = invert;
    }

    public void ActualizeOptions()
    {
        audioMixer.SetFloat("MasterVolume", GameManager.Instance.OptionsData.masterVolume);
        audioMixer.SetFloat("MusicVolume", GameManager.Instance.OptionsData.musicVolume);
        audioMixer.SetFloat("EffectsVolume", GameManager.Instance.OptionsData.effectsVolume);
        try
        {
            FindObjectOfType<CameraController>().m_Sensitivity = GameManager.Instance.OptionsData.sensitivity;
            FindObjectOfType<CameraController>().invertMouse = GameManager.Instance.OptionsData.invertedMouse;
        }
        catch (System.Exception)
        {

            throw;
        }
        
    }

}
