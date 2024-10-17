using System;
using Tavaris.Interactable;
using Tavaris.Manager;
using Tavaris.States;
using UnityEngine;
using Cursor = UnityEngine.Cursor;

namespace Tavaris.Objects
{
    public class InteractablePuzzle : InteractableObject, IPuzzle
    {
        public Puzzle attachedPuzzle;

        public Transform cameraPosition;

        static CameraController playerCameraController;

        public Sprite puzzleCursor;

        void Awake()
        {
            //attachedPuzzle = GetComponent<Puzzle>();
        }

        void Start()
        {
            if (playerCameraController == null)
            {
                playerCameraController = FindObjectOfType<CameraController>();
            }
        }

        public override void UpdateInteractable()
        {
            base.UpdateInteractable();
            if (IsInteracting)
            {
                Interacting();
            }
        }

        public override bool Interact(bool interactEnable)
        {
            if (interactEnable)
            {
                if (!IsInteracting)
                {
                    StartInteract();
                    return true;
                }
            }
            else
            {
                if (IsInteracting)
                {
                    EndInteract();
                    return true;
                }
            }

            return false;
        }

        public void ForceEndInteraction()
        {
            Debug.LogWarning("Forced end of interaction!", this.gameObject);
            EndInteract();
            IsInteracting = false;
        }

        public override void StartInteract()
        {
            base.StartInteract();
            // Change camera position
            // Change player state to puzzle interaction.
            // Which blocks movement and camera input. Plus all necessary.
            // Show cursor to begin interacting.
            // Set IsInteracting to true.

            GameManager.Player.cameraController.StartLerpToPos(cameraPosition);
            GameManager.Player.stateMachine.SwitchState<State_Player_PuzzleInspect>();

            try
            {
                Cursor.SetCursor(puzzleCursor.texture, Vector3.zero, CursorMode.Auto);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message, this);
            }

            //playerCameraController.SetNewPosition(attachedPuzzle.gameObject.transform,true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            IsInteracting = true;
            attachedPuzzle.StartGame();
        }

        public override void Interacting()
        {
            base.Interacting();
            // Lerp camera position?
            // Read mouse inputs for clicks.
            // Raycast stuff to activate puzzle.
            // And much more!?...

            if (Input.GetMouseButtonDown(0))
            {
                InteractWithPuzzlePiece();
            }

            //Debug.Log("Interacting!");
        }

        private void InteractWithPuzzlePiece()
        {
            Ray mouseRay =
                GameManager.Player.cameraController.attachedCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out var hitInfo, 5f, GameManager.Instance.GameSettings.PuzzleElementsLayers))
            {
                try
                {
                    hitInfo.collider.gameObject.GetComponent<PuzzlePiece>()?.OnPuzzlePieceActivate.Invoke();
                    //Debug.DrawRay(mouseRay.origin, mouseRay.direction * hitInfo.distance, Color.blue, 3f);
                }
                catch (NullReferenceException)
                {
                    //Debug.DrawRay(mouseRay.origin, mouseRay.direction * hitInfo.distance, Color.red, 2f);
                }
            }
        }

        public override void EndInteract()
        {
            base.EndInteract();
            Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);
            // Restore camera position.
            // Restore controls to player.
            // Restore cursor to usual settings.
            // Set IsInteracting to false.

            EndInteractActions();
            attachedPuzzle.EndGame();
        }

        public void EndInteractActions()
        {
            playerCameraController.RestorePosition();
            GameManager.Player.stateMachine.SwitchState<State_Player_Walking>();
            IsInteracting = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}