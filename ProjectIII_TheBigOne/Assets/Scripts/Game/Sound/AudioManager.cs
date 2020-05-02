using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance = null;

    public static AudioManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlaySoundAtLocation(string route, Vector3 position, float volume = 1, float minRange = 1, float maxRange = 10)
    {
        GameObject sound = new GameObject();
        AudioSource s = sound.AddComponent<AudioSource>();

        //Sound properties
        sound.transform.position = position;
        s.spatialBlend = 1;
        s.clip = Resources.Load<AudioClip>(route);
        
        if(s.clip == null)
        {
            Debug.LogWarning("Invalid path for audio clip");
        }

        s.volume = volume;
        s.minDistance = minRange;
        s.maxDistance = maxRange;
        s.rolloffMode = AudioRolloffMode.Linear;


        sound.transform.parent = GameObject.Find("Sound").transform;
        sound.name = s.clip.name;
        //Play
        s.Play();

        StartCoroutine(Utils.DestroyAfterTime(sound, s.clip.length));
    }

    public void PlayLoopingSound(string route, float volume)
    {
        GameObject sound = new GameObject();
        AudioSource s = sound.AddComponent<AudioSource>();

        //Sound properties
        s.clip = Resources.Load<AudioClip>(route);

        if (s.clip == null)
        {
            Debug.LogWarning("Invalid path for audio clip");
        }

        s.volume = volume;

        sound.transform.parent = GameObject.Find("Sound").transform;
        sound.name = s.clip.name;
        //Play
        s.Play();

        StartCoroutine(Utils.DestroyAfterTime(sound, s.clip.length));
    }

    public void PlaySound2D(string route)
    {
        GameObject sound = new GameObject();
        AudioSource s = sound.AddComponent<AudioSource>();
        s.spatialBlend = 0;
        s.clip = Resources.Load<AudioClip>(route);
        if (s.clip == null)
        {
            Debug.LogWarning("Invalid path for audio clip");
        }
       
        s.Play();

        sound.transform.parent = GameObject.Find("Sound").transform;
        sound.name = s.clip.name;

        StartCoroutine(Utils.DestroyAfterTime(sound, s.clip.length));
    }
}
