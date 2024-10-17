using UnityEngine.Events;

namespace Tavaris.Interactable
{
    public class SceneItem : InteractableObject, IPickable
    {
        public string itemID;

        public UnityEvent OnItemPickup = new UnityEvent();

        private bool alreadyUsed = false;


        public override bool Interact(bool interactEnable)
        {
            if (alreadyUsed || IsInteracting || !interactEnable) return false;
            if (GameManager.Instance.GiveItemToPlayer(itemID))
            {
                SoundManager.Instance.PlaySound2D("event:/SFX/Environment/Interactable/ItemPickUp");
                this.gameObject.SetActive(false);
                alreadyUsed = true;
                OnItemPickup.Invoke();
            }
            return true;
        }

        public override void StartInteract()
        {
            base.StartInteract();
        }

        public override void Interacting()
        {
            base.Interacting();
            //throw new System.NotImplementedException();
        }

        public override void EndInteract()
        {
            base.EndInteract();
        }
    }
}