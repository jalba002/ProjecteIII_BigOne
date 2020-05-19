using Game.Inputs;
using UnityEngine;

namespace Characters.Brains
{
    public class PlayerBrain : Brain
    {
        public Inputs inputList;
        public bool InteractRelease { get; protected set; }
        public bool MouseInteract { get; protected set; }
        
        public bool ShowInventory { get; protected set; }

        public bool ShowPause { get; protected set; }

        public bool FlashlightToggle { get; protected set; }

        public bool MouseInteractRelease { get; protected set; }

        public Vector2 MouseInput { get; protected set; }
        public float MouseWheel { get; protected set; }

        public override void GetActions()
        {
            // All inputs.
            Interact = GetInputValue(inputList.interact);
            MouseInteract = GetInputValue(inputList.mouseInteract);
            Running = GetInputValue(inputList.running);
            FlashlightToggle = GetInputValue(inputList.flashlight);
            ShowInventory = GetInputValue(inputList.inventory);
            ShowPause = GetInputValue(inputList.pause);

            // On Release
            InteractRelease = Input.GetButtonUp(inputList.interact.name);
            MouseInteractRelease = Input.GetButtonUp(inputList.mouseInteract.name);

            // Raw mouse input.
            MouseWheel = Input.mouseScrollDelta.y;
            MouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            // Directional input control.
            Direction = new Vector3(
                Input.GetAxis(inputList.horizontal.name ?? "Horizontal"),
                0f,
                Input.GetAxis(inputList.forward.name ?? "Vertical")
            );
        }

        private bool GetInputValue(InputNames input)
        {
            return input.holdButton
                ? Input.GetButton(input.name)
                : Input.GetButtonDown(input.name);
        }
    }
}