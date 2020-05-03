using System.Security.Cryptography;
using Player;
using UnityEngine;

public class SceneItem : MonoBehaviour, IInteractable
{
    public string itemName;
    private bool alreadyUsed = false;

    public bool Interact()
    {
        if (alreadyUsed) return false;
        if (FindObjectOfType<PlayerController>().playerInventory.AddItem(itemName))
        {
            this.gameObject.SetActive(false);
            alreadyUsed = true;
        }
        return false;
    }

    public void OnStartInteract()
    {
        //throw new System.NotImplementedException();
    }

    public void OnInteracting()
    {
        //throw new System.NotImplementedException();
    }

    public void OnEndInteract()
    {
        //throw new System.NotImplementedException();
    }
}