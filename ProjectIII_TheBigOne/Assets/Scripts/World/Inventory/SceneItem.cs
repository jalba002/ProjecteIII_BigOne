using Player;
using UnityEngine.Events;

public class SceneItem : InteractableObject, IPickable
{
    public string itemID;

    public UnityEvent OnItemPickup = new UnityEvent();
    
    private bool alreadyUsed = false;
    
    private void Start()
    {
        IsInteracting = false;
    }
    
    public override bool Interact(bool interactEnable)
    {
        if (alreadyUsed || IsInteracting || !interactEnable) return false;
        if (FindObjectOfType<PlayerController>().playerInventory.AddItem(itemID))
        {
            SoundManager.Instance.PlaySound2D("event:/SFX/Environment/Interactable/ItemPickUp");
            this.gameObject.SetActive(false);
            alreadyUsed = true;
            OnItemPickup.Invoke();
        }
        return true;
    }

    public override void OnStartInteract()
    {
        base.OnStartInteract();
        IsInteracting = true;
    }

    public override void OnInteracting()
    {
        base.OnInteracting();
        //throw new System.NotImplementedException();
    }

    public override void OnEndInteract()
    {
        base.OnEndInteract();
        IsInteracting = false;
    }
}