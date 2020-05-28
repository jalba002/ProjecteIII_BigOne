using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FearSystemManager : MonoBehaviour
{
    public PostProcessVolume PostProcessVolume;

    private ChromaticAberration chromaticAberration = null;
    private EnemyBrain enemyBrain;

    private bool enableSystem = false;

    private void Start()
    {
        enemyBrain = FindObjectOfType<EnemyBrain>();
        if (enemyBrain == null) enableSystem = false;
        PostProcessVolume.profile.TryGetSettings(out chromaticAberration);
        SetupChromatic();
    }

    private void SetupChromatic()
    {
        if (enableSystem == false)
        {
            Debug.LogWarning("Fear System not enabled. Enemy not in scene.");
            return;
        }
        
        if (chromaticAberration == null)
        {
            Debug.LogError("Chromatic Aberration feedback unavailable.");
            return;
        }

        chromaticAberration.enabled.value = true;
        chromaticAberration.intensity.value = 1;
    }

    public void UpdateVisuals()
    {
        // TODO Analyze different variables
        // Apply different camera effects for giving feedback
        // Depending on time chased and senses the enemy has against you.
        // Like, if player is being chased, the visuals turn black-ish.
    }
}