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
    public string latchPath;

    private void Start()
    {
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
        SoundManager.Instance.PlayOneShotSound(scarejumpPath, scarejumpPos.position);
    }
    public void PlayPanelLatchSound()
    {
        SoundManager.Instance.PlaySoundAtLocation(latchPath, latchPanelDoorPos.position, 1, 1, 20);
    }


}
