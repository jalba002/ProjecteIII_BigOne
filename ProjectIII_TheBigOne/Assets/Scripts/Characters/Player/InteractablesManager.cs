using System;
using Enemy;
using Interfaces;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Characters.Player
{
    // Make a raycast.
    // Identify the object type.
    // Write the correct thing on UI 

    public class InteractablesManager : MonoBehaviour
    {
        [Header("Settings")] public LayerMask detectedLayers;
        public float detectionRange;

        [Header("Components")] public PlayerController attachedPlayer;
        public Text textDebug;

        public IInteractable CurrentInteractable { get; set; }


        public void Start()
        {
            attachedPlayer = GetComponent<PlayerController>();
        }

        public void Update()
        {
            if (!attachedPlayer.cameraController.angleLocked)
                AnalyzeElement(DetectElement());
        }

        private void AnalyzeElement(IInteractable detectedElement)
        {
            if (detectedElement == null)
            {
                CurrentInteractable = null;
                ShowText(false);
                return;
            }

            CurrentInteractable = detectedElement;
            ShowText(true);
        }

        private void ShowText(bool enable)
        {
            if (textDebug)
            {
                if (CurrentInteractable != null)
                    textDebug.text = CurrentInteractable.DisplayName;
                textDebug.gameObject.SetActive(enable);
            }
        }

        private IInteractable DetectElement()
        {
            RaycastHit hitInfo;
            Ray cameraRay =
                attachedPlayer.cameraController.attachedCamera.ViewportPointToRay(new Vector3(.5f, .5f, .5f));
            if (Physics.Raycast(cameraRay, out hitInfo, detectionRange, detectedLayers))
            {
                try
                {
                    return hitInfo.collider.gameObject.GetComponent<IInteractable>();
                }
                catch (MissingComponentException)
                {
                    //Debug.Log("Object hit was not Interactable.");
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}