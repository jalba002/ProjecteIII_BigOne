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

    //[Header("Audio")] private 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("EffectsVolume", volume);
    }

    public void SetSensibility(float sensibility)
    {
        FindObjectOfType<CameraController>().m_Sensitivity = sensibility;
    }
    public void SetInvertMouse(bool invert)
    {
        FindObjectOfType<CameraController>().invertMouse = invert;
    }

}
