using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager m_Instance = null;
    public static AudioManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (AudioManager) FindObjectOfType(typeof(AudioManager ));
                if (m_Instance == null)
                {
                    m_Instance = (new GameObject("AudioManager")).AddComponent<AudioManager>();
                }
                DontDestroyOnLoad(m_Instance.gameObject);
            }
            return m_Instance;
        }
    }

    private static GameObject SoundParent { get; set; }

    public static void PlaySoundAtLocation(string route, Vector3 position, float volume = 1, float minRange = 1,
        float maxRange = 10)
    {
        GameObject sound = new GameObject();
        AudioSource s = sound.AddComponent<AudioSource>();

        //Sound properties
        sound.transform.position = position;
        s.spatialBlend = 1;
        s.clip = Resources.Load<AudioClip>(route);

        if (s.clip == null)
        {
            Debug.LogWarning("Invalid path for audio clip");
        }

        s.volume = volume;
        s.minDistance = minRange;
        s.maxDistance = maxRange;
        s.rolloffMode = AudioRolloffMode.Linear;


        Parent(sound);
        sound.name = s.clip.name;
        //Play
        s.Play();

        GameManager.Instance.StartCoroutine(Utils.DestroyAfterTime(sound, s.clip.length));
    }

    public static void PlayLoopingSound(string route, float volume)
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

        Parent(sound);
        sound.name = s.clip.name;
        //Play
        s.Play();

        GameManager.Instance.StartCoroutine(Utils.DestroyAfterTime(sound, s.clip.length));
    }

    public static void PlaySound2D(string route)
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

        Parent(sound);
        sound.name = s.clip.name;

        GameManager.Instance.StartCoroutine(Utils.DestroyAfterTime(sound, s.clip.length));
    }

    private static void Parent(GameObject objectToParent)
    {
        if (SoundParent == null)
        {
            SoundParent = new GameObject("Sounds");
        }

        objectToParent.transform.parent = SoundParent.transform;
    }
}