using Player;

public class SceneItem : InteractableObject, IPickable
{
    public string itemID;
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
            this.gameObject.SetActive(false);
            alreadyUsed = true;
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