using System;
using Enemy;
using FMOD.Studio;
using UnityEngine;

public class HeartbeatController : MonoBehaviour
{
    private EnemyController enemyController;

    private bool alreadyPlaying = false;

    private SoundManagerMovingSound movingSound;

    void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
        if (enemyController == null)
        {
            Debug.LogWarning("Heartbeat controller not as a child of enemy. DESTROYING");
            Destroy(this);
        }

        alreadyPlaying = false;

        movingSound = null;
    }

    private void Update()
    {
        try
        {
            if ((enemyController.currentBrain.IsChasingPlayer || enemyController.currentBrain.IsTrackingPlayer ||
                 enemyController.currentBrain.IsHearingPlayer) && !alreadyPlaying)
            {
                PlayNewSound();
                alreadyPlaying = true;
            }
            else if (alreadyPlaying && !(enemyController.currentBrain.IsChasingPlayer ||
                                         enemyController.currentBrain.IsTrackingPlayer ||
                                         enemyController.currentBrain.IsHearingPlayer))
            {
                SoundManager.Instance.StopMovingSound(movingSound);
                alreadyPlaying = false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Couldn't play sound!");
            Destroy(this);
        }
    }

    private void PlayNewSound()
    {
        if (movingSound != null)
            SoundManager.Instance.StopMovingSound(movingSound, true);
        SoundManager.Instance.PlayEvent(enemyController.terrorRadiusPath,
            enemyController.transform, 0f,
            enemyController.characterProperties.terrorRadiusRange, out movingSound);
    }

    public void StopHeartbeat()
    {
        try
        {
            SoundManager.Instance.StopMovingSound(movingSound, true);
        }
        catch (Exception e)
        {
        }
    }
}