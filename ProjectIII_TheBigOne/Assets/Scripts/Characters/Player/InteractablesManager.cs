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
        [System.Serializable]
        public enum InteractableType
        {
            Movable = 0,
            Pickable,
            Inspectable
        }

        public InteractableType interactType;

        public string[] HudText = new string[3]
        {
            "Drag",
            "Take",
            "Use"
        };

        [Header("Settings")] public LayerMask detectedLayers;
        public float detectionRange;

        [Header("Components")] public PlayerController attachedPlayer;
        public Text textDebug;

        [Header("Private Components")] private IInteractable CurrentInteractable;

        public void Start()
        {
            attachedPlayer = GetComponent<PlayerController>();
        }

        public void Update()
        {
            AnalyzeElement(DetectElement());
            InteractWithElement();
        }

        private void AnalyzeElement(IInteractable detectedElement)
        {
            if (detectedElement == null)
            {
                CurrentInteractable = null;
                textDebug.gameObject.SetActive(false);
                return;
            }
            CurrentInteractable = detectedElement;
            DetermineCurrentType(detectedElement);
            textDebug.gameObject.SetActive(true);
            textDebug.text = HudText[(int)interactType] + " " + CurrentInteractable.DisplayName;
        }

        private void DetermineCurrentType(IInteractable detectedElement)
        {
            if (detectedElement.attachedGameobject.GetComponent<IMovable>() != null)
            {
                interactType = InteractableType.Movable;
                return;
            }
            else if (detectedElement.attachedGameobject.GetComponent<IPickable>() != null)
            {
                interactType = InteractableType.Pickable;
                return;
            }
            else if (detectedElement.attachedGameobject.GetComponent<IInspectable>() != null)
            {
                interactType = InteractableType.Inspectable;
                return;
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
                    Debug.Log("Object hit was not Interactable.");
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private void InteractWithElement()
        {
            if (CurrentInteractable == null || this.isActiveAndEnabled == false) return;
            switch (interactType)
            {
                case InteractableType.Movable:
                case InteractableType.Pickable:
                    if (attachedPlayer.currentBrain.MouseInteract)
                    {
                        CurrentInteractable.Interact();
                    }

                    break;
                case InteractableType.Inspectable:
                    if (attachedPlayer.currentBrain.Interact)
                    {
                        CurrentInteractable.Interact();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        /*void InspectObjects()
        {
            if (objectInspector != null && objectInspector.isActiveAndEnabled)
            {
                if (attachedPlayer.currentBrain.Interact)
                {
                    if (objectInspector.Activate(attachedPlayer.cameraController.attachedCamera))
                    {
                        // Disable camera and allow the object inspector the use of mouse input.
                        bool enableStuff = objectInspector.GetEnabled();
                        attachedPlayer.cameraController.angleLocked = enableStuff;
                        if (enableStuff)
                        {
                            attachedPlayer.stateMachine.SwitchState<State_Player_Inspecting>();
                        }
                        else
                            attachedPlayer.stateMachine.SwitchState<State_Player_Walking>();
                    }
                }
            }
        }

        void InteractDoors()
        {
            if (simpleActivator != null && simpleActivator.isActiveAndEnabled)
            {
                if (currentBrain.MouseInteract)
                {
                    if (simpleActivator.Activate(cameraController.attachedCamera))
                        cameraController.angleLocked = true;
                }
                else if (currentBrain.MouseInteractRelease)
                {
                    if (simpleActivator.Deactivate())
                        cameraController.angleLocked = false;
                }
            }
        }

        private void UseFlashlight()
        {
            if (currentBrain.FlashlightToggle)
            {
                attachedFlashlight.ToggleFlashlight();
            }
        }*/
    }
}