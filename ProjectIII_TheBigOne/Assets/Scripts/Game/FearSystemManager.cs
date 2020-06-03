using System;
using System.Collections;
using System.Collections.Concurrent;
using Aura2API;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FearSystemManager : MonoBehaviour
{
    #region Settings

    [System.Serializable]
    public struct ChromaticSettings
    {
        public float maximumAberration;
        public float chromaticVariationPerSecond;
    }

    [System.Serializable]
    public struct VignetteSettings
    {
        public float maximumVignette;
        public float minimumVignette;
        public float vignetteVariationPerSecond;
    }

    #endregion

    [Header("Components")] public PostProcessVolume PostProcessVolume;

    [Header("Settings")] public ChromaticSettings chromaticSettings;
    public VignetteSettings vignetteSettings;
    private float currentAberrationMaxValue = 0f;
    private float currentVignetteMaxValue = 0f;

    private ChromaticAberration chromaticAberration = null;
    private Vignette Vignette;
    private EnemyBrain enemyBrain;

    private Coroutine resetEffects;

    private void Start()
    {
        enemyBrain = FindObjectOfType<EnemyBrain>();
        
        PostProcessVolume.profile.TryGetSettings(out chromaticAberration);
        PostProcessVolume.profile.TryGetSettings(out Vignette);
        
        Setup();
    }

    private void Setup()
    {
        if (chromaticAberration == null || Vignette == null)
        {
            Debug.LogError("Some of the effects are missing! Cancelling visual feedback.");
            return;
        }

        chromaticAberration.enabled.value = true;
        chromaticAberration.intensity.value = 0f;
        currentAberrationMaxValue = 0f;
        currentVignetteMaxValue = 0f;
        ApplyVisuals();
        //resetEffects = RestartCoroutine(resetEffects, ReduceValues());
    }

    private void Update()
    {
        if (enemyBrain.IsHearingPlayer || enemyBrain.IsChasingPlayer)
        {
            UpdateVisuals();
        }
    }
    

    public void UpdateVisuals()
    {
        // TODO Analyze different variables
        // Apply different camera effects for giving feedback
        // Depending on time chased and senses the enemy has against you.
        // Like, if player is being chased, the visuals turn black-ish.
        float clampedDistance = Mathf.Clamp(enemyBrain.DistanceToPlayer,
            0f,
            enemyBrain.selfCharacter.characterProperties.hearingMaxRange);

        float calculatedValue = Mathf.Abs(
            (clampedDistance / enemyBrain.selfCharacter.characterProperties.hearingMaxRange) -
            1f);

        currentAberrationMaxValue = calculatedValue * chromaticSettings.maximumAberration;
        currentVignetteMaxValue = Mathf.Lerp(vignetteSettings.minimumVignette, vignetteSettings.maximumVignette,
            calculatedValue);

        ApplyVisuals();
    }

    private void ApplyVisuals()
    {
        chromaticAberration.intensity.value = currentAberrationMaxValue;
        Vignette.intensity.value = currentVignetteMaxValue;
    }

    private Coroutine RestartCoroutine(Coroutine coroutineHolder, IEnumerator newCoroutine)
    {
        if (coroutineHolder != null)
            StopCoroutine(coroutineHolder);
        return StartCoroutine(newCoroutine);
    }

    private IEnumerator ReduceValues()
    {
        while (true)
        {
            if (!enemyBrain.IsHearingPlayer)
            {
                yield return new WaitForSeconds(2.0f);
                while (chromaticAberration.intensity.value > 0f || Vignette.intensity.value > 0f)
                {
                    chromaticAberration.intensity.value -=
                        chromaticSettings.chromaticVariationPerSecond * Time.deltaTime;
                    Vignette.intensity.value -= vignetteSettings.vignetteVariationPerSecond * Time.deltaTime;

                    chromaticAberration.intensity.value = Mathf.Max(0f, chromaticAberration.intensity.value);
                    Vignette.intensity.value = Mathf.Max(0f, Vignette.intensity.value);
                }
            }
        }
    }
}