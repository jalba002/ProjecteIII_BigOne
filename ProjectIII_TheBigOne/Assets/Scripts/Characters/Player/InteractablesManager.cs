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
        [Header("Interactions")] public float sphereCastRange = 1.5f;
        public Transform sphereCastPosition;
        public LayerMask sphereCastLayerMask;
        private List<InteractableObject> registeredInteractables = new List<InteractableObject>();

        [Header("Settings")] public LayerMask detectedLayers;

        [Header("Components")] public PlayerController attachedPlayer;
        public Text textDebug;

        public GameObject aimedObject;
        public InteractableObject CurrentInteractable { get; set; }

        public bool CanInteract;

        public float raycastCooldownPerCheck = 0.1f;
        private float currentRaycastCooldown;

        public void Start()
        {
            attachedPlayer = GetComponent<PlayerController>();
            ClearInteractable();
            currentRaycastCooldown = raycastCooldownPerCheck;
        }

        public void Update()
        {
            if (!enabled)
            {
                // TODO Add more stuff to stop?
                return;
            }

            registeredInteractables = CheckForItems(sphereCastPosition, sphereCastRange, sphereCastLayerMask);
            CanInteract = registeredInteractables.Count > 0 && CurrentInteractable != null;

            // Debug.Log(CurrentInteractable?.DisplayName ?? "No current interactable.");
            if (currentRaycastCooldown > 0f)
            {
                currentRaycastCooldown -= Time.deltaTime;
            }

            aimedObject = DetectElement();
            TreatInteractables();
        }

        private void TreatInteractables()
        {
            if (registeredInteractables.Count > 0 || CurrentInteractable != null)
            {
                // UpdateInteractables(registeredInteractables);

                if (CurrentInteractable != null)
                    CurrentInteractable.UpdateInteractable();


                if (currentRaycastCooldown <= 0f)
                {
                    InteractableObject collidedInteractable;
                    collidedInteractable = registeredInteractables.Find(x => x.gameObject == aimedObject);

                    AnalyzeElement(collidedInteractable);

                    currentRaycastCooldown = raycastCooldownPerCheck;
                }
            }
        }

        private void UpdateInteractables(List<InteractableObject> interactablesList)
        {
            foreach (InteractableObject interactable in interactablesList)
            {
                interactable.UpdateInteractable();
            }
        }

        private void AnalyzeElement(InteractableObject detectedElement)
        {
            //Debug.Log("Detected element is: " + detectedElement);
            if (detectedElement == CurrentInteractable) return;

            if (detectedElement != null)
            {
                if (CurrentInteractable != null)
                {
                    if (!CurrentInteractable.IsInteracting)
                    {
                        CurrentInteractable = detectedElement;
                    }
                }
                else
                {
                    CurrentInteractable = detectedElement;
                }
            }
            else
            {
                if (CurrentInteractable != null)
                {
                    if (!CurrentInteractable.IsInteracting)
                    {
                        CurrentInteractable = null;
                    }
                }
            }

            if (textDebug)
            {
                textDebug.text = CurrentInteractable != null ? CurrentInteractable.displayName : "";
            }
        }


        private GameObject DetectElement()
        {
            Ray cameraRay =
                attachedPlayer.cameraController.attachedCamera.ViewportPointToRay(new Vector3(.5f, .5f, .5f));
            if (Physics.Raycast(cameraRay, out var hitInfo,
                sphereCastRange, detectedLayers))
            {
                Debug.DrawRay(cameraRay.origin, cameraRay.direction * hitInfo.distance, Color.green, 1f);
                return hitInfo.collider.gameObject;
            }
            else
            {
                return null;
            }
        }

        public void ClearInteractable()
        {
            if (CurrentInteractable != null)
                CurrentInteractable.OnEndInteract();

            CurrentInteractable = null;
            if (textDebug)
                textDebug.text = "";
        }

        public List<InteractableObject> CheckForItems(Transform castPosition, float castRange, LayerMask castLayer)
        {
            Collider[] objects = Physics.OverlapSphere(castPosition.position, castRange, castLayer);
            List<InteractableObject> objectList = new List<InteractableObject>();
            foreach (Collider objectDetected in objects)
            {
                InteractableObject objectFound =
                    ObjectTracker.interactablesList.Find(x => x.gameObject == objectDetected.gameObject);
                if (objectFound != null)
                {
                    objectList.Add(objectFound);
                }
            }

            return objectList;
        }
    }
}