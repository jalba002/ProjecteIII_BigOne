﻿using UnityEngine;

[CreateAssetMenu(fileName = "Data_InteractionDefaultSettings", menuName = "Tavaris/Data/InteractionSettings")]
public class InteractSettings : ScriptableObject
{
    [System.Serializable]
    public enum InteractionType
    {
        Drag,
        Pick,
        Inspect,
        Puzzle
    }

    public InteractionType interactionType;

    public string displayName;

    public Sprite previewImage;
    public Sprite displayImage;
}
