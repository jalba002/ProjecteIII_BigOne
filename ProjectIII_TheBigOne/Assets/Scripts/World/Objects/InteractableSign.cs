using System;
using UnityEngine;

namespace World.Objects
{
    public class InteractableSign : InteractableObject
    {
        private void Start()
        {
            if (displayName.Length <= 0)
                displayName = this.gameObject.name;
        }

        public override void UpdateInteractable()
        {
            GameManager.Instance.CanvasController.ChangeCursor(
                GameManager.Instance.CanvasController.CrosshairController.defaultCrosshair,
                new Vector3(0.1f, 0.1f, 1f));
        }
    }
}