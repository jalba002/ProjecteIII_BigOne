using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class GameFinisher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            GameManager.Instance.EndGame();
        }
    }
}
