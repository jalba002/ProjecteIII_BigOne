using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OptionsData : ScriptableObject
{
    [Range(0.1f, 10.0f)] public float sensitivity = 1f;
    public bool invertedMouse = false;
    [Range(-80f, 0f)] public float masterVolume = 1f;
    [Range(-80f, 0f)] public float musicVolume = 1f;
    [Range(-80f, 0f)] public float effectsVolume = 1f;
}
