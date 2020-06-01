using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitterCalls : MonoBehaviour
{
    public Transform doorPos;
    public Transform wallPos;
    public Transform scarejumpPos;

    public string doorSlamPath;
    public string breakingWallPath;
    public string scarejumpPath;

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
        SoundManager.Instance.PlaySoundAtLocation(breakingWallPath, wallPos.position, 1, 1, 20);
    }
    public void PlayScarejumpSound()
    {
        SoundManager.Instance.PlaySoundAtLocation(scarejumpPath, scarejumpPos.position, 1, 1, 20);
    }


}
