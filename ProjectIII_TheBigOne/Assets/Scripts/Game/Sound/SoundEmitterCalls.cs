using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitterCalls : MonoBehaviour
{
    public Transform doorPos;
    public Transform wallPos;
    public Transform scarejumpPos;
    public Transform latchPanelDoorPos;

    public string doorSlamPath;
    public string breakingWallPath;
    public string hammerSlamPath;
    public string scarejumpPath;
    public string playerBreathingPath;

    public string latchPath;

    private void Start()
    {
        SoundManager.Instance.PlayEvent("event:/SFX/Environment/Room/VoidAmbient", GameManager.Instance.PlayerController.transform, 0.3f);


        if(doorPos == null)
        {
            GameObject.Find("DoorSlamSoundEmitter");
        }
        if (wallPos == null)
        {
            GameObject.Find("BreakingWallSoundEmitter");
        }
        if (scarejumpPos == null)
        {
            GameObject.Find("ScarejumpSoundEmitter");
        }
    }


    public void PlayDoorSlamSound()
    {
        SoundManager.Instance.PlaySoundAtLocation(doorSlamPath, doorPos.position, 1, 1, 20);
    }
    public void PlayBreakingWallSound()
    {
        SoundManager.Instance.PlayOneShotSound(breakingWallPath, wallPos.position);
        SoundManager.Instance.PlayOneShotSound(hammerSlamPath, wallPos.position);
    }
    public void PlayScarejumpSound()
    {
        SoundManager.Instance.PlayEvent(scarejumpPath, GameManager.Instance.PlayerController.transform);
        SoundManager.Instance.PlaySoundAtLocation(playerBreathingPath, GameManager.Instance.PlayerController.transform.position);
    }
    public void PlayPanelLatchSound()
    {
        SoundManager.Instance.PlaySoundAtLocation(latchPath, latchPanelDoorPos.position, 1, 1, 20);
    }


}
