using System;
using System.Collections.Generic;
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
        public List<IInteractable> registeredInteractables = new List<IInteractable>();

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
            if (registeredInteractables.Count > 0)
                AnalyzeElement(DetectElement());
            else if (textDebug.isActiveAndEnabled)
            {
                textDebug.gameObject.SetActive(false);
            }
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
            Ray cameraRay =
                attachedPlayer.cameraController.attachedCamera.ViewportPointToRay(new Vector3(.5f, .5f, .5f));
            if (Physics.Raycast(cameraRay, out var hitInfo, detectionRange, detectedLayers))
            {
                try
                {
                    Debug.DrawRay(cameraRay.origin, cameraRay.direction * hitInfo.distance, Color.green, 1f);
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