using UnityEngine;

public interface IInteractable
{
    string DisplayName { get; set; }
    GameObject attachedGameobject { get; } 

    bool Interact();

    void OnStartInteract();
    void OnInteracting();
    void OnEndInteract();
}
