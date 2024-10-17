using Tavaris.Dynamic;
using Tavaris.Interfaces;
using UnityEngine;

namespace Tavaris.Interactables
{
    public class ItemRequirements : MonoBehaviour
    {
        public string requiredItemID;
        private Door attachedDoor;

        private void Awake()
        {
            if (attachedDoor == null)
            {
                attachedDoor = gameObject.GetComponentInChildren<Door>();
            }

            attachedDoor.OnStartInteraction += () => { Unlock(attachedDoor); };
        }

        public void Unlock(ILockable lockable)
        {
            // Ask Game Manager if player has X in its inventory.
            if (GameManager.Instance.HasPlayerThisItemInInventory(requiredItemID))
            {
                lockable.Unlock();
                GameManager.Instance.RemoveItemFromPlayerInventory(requiredItemID);
            }
        }
    }
}