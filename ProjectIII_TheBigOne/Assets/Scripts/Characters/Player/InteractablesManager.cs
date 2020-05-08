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

        [Header("Private Components")] private IInteractable CurrentInteractable;
        private ObjectInspector objectInspector;
        private SimpleActivator simpleActivator;

        public void Start()
        {
            attachedPlayer = GetComponent<PlayerController>();
            objectInspector = GetComponent<ObjectInspector>();
            simpleActivator = GetComponent<SimpleActivator>();
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
            textDebug.gameObject.SetActive(true);
            textDebug.text = CurrentInteractable.DisplayName;
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

        private void InteractWithElement()
        {
            if (CurrentInteractable == null || !isActiveAndEnabled) return;
            
            if (InteractDoors()) return;
            if (InspectObjects()) return;
        }


        bool InspectObjects()
        {
            if (objectInspector != null && objectInspector.isActiveAndEnabled)
            {
                if (attachedPlayer.currentBrain.Interact)
                {
                    if (objectInspector.Activate(CurrentInteractable))
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

                        return true;
                    }
                }
            }

            return false;
        }

        bool InteractDoors()
        {
            if (simpleActivator != null && simpleActivator.isActiveAndEnabled)
            {
                if (attachedPlayer.currentBrain.MouseInteract)
                {
                    if (simpleActivator.Activate(CurrentInteractable))
                        attachedPlayer.cameraController.angleLocked = true;
                }
                else if (attachedPlayer.currentBrain.MouseInteractRelease)
                {
                    if (simpleActivator.Deactivate())
                        attachedPlayer.cameraController.angleLocked = false;
                }

                return true;
            }
            return false;
        }
    }
}