﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Player;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
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

        public bool CanInteract;

        public void Start()
        {
            attachedPlayer = GetComponent<PlayerController>();
        }

        public void Update()
        {
            CanInteract = registeredInteractables.Count > 0 && CurrentInteractable != null;

            //Debug.Log(CurrentInteractable?.DisplayName ?? "No current interactable.");

            if (CurrentInteractable != null || registeredInteractables.Count > 0)
                AnalyzeElement(DetectElement());
        }

        private void AnalyzeElement(IInteractable detectedElement)
        {
            //Debug.Log("Detected element is: " + detectedElement);
            if (detectedElement == CurrentInteractable) return;
            
            if (detectedElement != null)
            {
                if (CurrentInteractable != null)
                {
                    Debug.Log("Stage 1: Detected & Current");
                    if (!CurrentInteractable.IsInteracting)
                    {
                        Debug.Log("Stage 2: Detected & Current & Not Interacting");
                        CurrentInteractable = detectedElement;
                    }
                }
                else
                {
                    Debug.Log("Stage 3: Detected & No Current");
                    CurrentInteractable = detectedElement;
                }
            }
            else
            {
                Debug.Log("Stage 4: No Detected");
                if (CurrentInteractable != null)
                {
                    Debug.Log("Entering null/non-null");
                    if (!CurrentInteractable.IsInteracting)
                    {
                        Debug.Log("Deleting Interactable non-used");
                        CurrentInteractable = null;
                    }
                }
            }

            if (textDebug)
            {
                textDebug.text = CurrentInteractable?.DisplayName ?? "";
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