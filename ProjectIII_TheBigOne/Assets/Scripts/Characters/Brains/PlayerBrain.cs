using Game.Inputs;
using UnityEngine;

namespace Characters.Brains
{
    public class PlayerBrain : Brain
    {
        public Inputs inputList;

        public override void GetActions()
        {
            // All inputs.
            Interact = GetInputValue(inputList.interact);
            MouseInteract = GetInputValue(inputList.mouseInteract);
            Running = GetInputValue(inputList.running);

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