using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void EnablePlayerMovement()
    {
        GameManager.Instance.PlayerController.EnableStateMachine();
        GameManager.Instance.PlayerController.cameraController.angleLocked = false;
        GameManager.Instance.PlayerController.currentBrain.enabled = true;
        GameManager.Instance.PlayerController.interactablesManager.enabled = true;
    }
}